using EmpMS;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Forms;

namespace EmpMS
{
	public partial class PayRoll : BaseForm
	{
		public PayRoll(Home home)
		{
			InitializeComponent();
			this.Opacity = 0;
			InitializeCalculations();
			LoadEmployees();
			RefreshBtn.Click += RefreshBtn_Click;
			SearchBtn.Click += SearchBtn_Click;
			ApplyColorPalette();
			this.home = home;

			calculationTimer = new System.Windows.Forms.Timer();
			calculationTimer.Interval = 500;
			calculationTimer.Tick += (s, e) =>
			{
				calculationTimer.Stop();
				CalculatePay();
			};
			BaseSalaryTb.KeyPress += NumericTextBox_KeyPress;
			DWTb.KeyPress += IntegerTextBox_KeyPress;
			OverHrsTb.KeyPress += NumericTextBox_KeyPress;
			AllowanceTb.KeyPress += NumericTextBox_KeyPress;
			TardinessTb.KeyPress += IntegerTextBox_KeyPress;
			AbsencesTb.KeyPress += IntegerTextBox_KeyPress;
			UnpaidLeavesTb.KeyPress += IntegerTextBox_KeyPress;
		}
		private System.Windows.Forms.Timer calculationTimer;
		private readonly IColorPalette palette = new SpiritedAwayBathhousePalette();
		private Home home;
		private const decimal overtimeRate = 1.50m; // Updated for 2025 special holidays
		private const decimal restDayRate = 1.30m;  // New 2025 rest day rate
		private const decimal holidayRate = 1.30m;  // New 2025 regular holiday rate
		private void PayRoll_Load(object sender, EventArgs e)
		{
			LoadWorkSchedules();
			InitializeCalculations();
			LoadPayrollData();
			ResetFields();

			BaseSalaryTb.Text = "";
			DWTb.Text = "";
			OverHrsTb.Text = "";
			AllowanceTb.Text = "";
			TardinessTb.Text = "";
			AbsencesTb.Text = "";
			UnpaidLeavesTb.Text = "";

			PayPeriodStartDtp.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
			PayPeriodEndDtp.Value = PayPeriodStartDtp.Value.AddMonths(1).AddDays(-1);
			this.Opacity = 1;
		}
		public class ComboboxItem
		{
			public string Text { get; set; }
			public int Value { get; set; }
			public ComboboxItem(string text, int value) => (Text, Value) = (text, value);
			public override string ToString() => Text;
		}
		private void ResetFields()
		{
			BaseSalaryTb.Text = "";
			DWTb.Text = "";
			OverHrsTb.Text = "";
			AllowanceTb.Text = "";
			TardinessTb.Text = "";
			AbsencesTb.Text = "";
			UnpaidLeavesTb.Text = "";
			PayPeriodStartDtp.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
			PayPeriodEndDtp.Value = PayPeriodStartDtp.Value.AddMonths(1).AddDays(-1);
		}
		private void LoadEmployees()
		{
			EmpCb.Items.Clear();
			string query = "SELECT EmpID, FirstName, LastName FROM EmployeeTbl";
			using (OleDbConnection con = GetConnection())
			using (OleDbCommand cmd = new OleDbCommand(query, con))
			{
				con.Open();
				OleDbDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					int empId = Convert.ToInt32(reader["EmpID"]);
					string fullName = $"{reader["FirstName"]} {reader["LastName"]}";
					EmpCb.Items.Add(new ComboboxItem(fullName, empId));
				}
			}
		}
		private void LoadWorkSchedules()
		{
			WorkScheduleCb.Items.AddRange(new object[] { "Monthly", "Weekly" });
			if (WorkScheduleCb.Items.Count > 0) WorkScheduleCb.SelectedIndex = 0;
		}
		private void AggregateAndFillAttendanceFields()
		{
			if (!(EmpCb.SelectedItem is ComboboxItem selectedItem)) return;

			int empId = selectedItem.Value;
			DateTime periodStart = PayPeriodStartDtp.Value.Date;
			DateTime periodEnd = PayPeriodEndDtp.Value.Date;

			int daysWorked;
			decimal overtimeHours;
			int tardinessMinutes;
			int absences;
			int unpaidLeaves;

			AggregateAttendanceData(empId, periodStart, periodEnd,
				out daysWorked, out overtimeHours, out tardinessMinutes, out absences, out unpaidLeaves);

			if (string.IsNullOrWhiteSpace(DWTb.Text))
				DWTb.Text = daysWorked.ToString();
			if (string.IsNullOrWhiteSpace(OverHrsTb.Text))
				OverHrsTb.Text = overtimeHours.ToString("N2");
			if (string.IsNullOrWhiteSpace(TardinessTb.Text))
				TardinessTb.Text = tardinessMinutes.ToString();
			if (string.IsNullOrWhiteSpace(AbsencesTb.Text))
				AbsencesTb.Text = absences.ToString();
			if (string.IsNullOrWhiteSpace(UnpaidLeavesTb.Text))
				UnpaidLeavesTb.Text = unpaidLeaves.ToString();
		}
		private void AggregateAttendanceData(int empId, DateTime periodStart, DateTime periodEnd,
			out int daysWorked, out decimal overtimeHours, out int tardinessMinutes, out int absences, out int unpaidLeaves)
		{
			daysWorked = 0; overtimeHours = 0; tardinessMinutes = 0; absences = 0; unpaidLeaves = 0;
			var scheduledStart = new TimeSpan(9, 0, 0); var scheduledEnd = new TimeSpan(18, 0, 0);

			string attQuery = @"SELECT [Date], TimeIn, TimeOut, Status FROM AttendanceTbl
                                WHERE EmpID = ? AND [Date] BETWEEN ? AND ?";
			using (var con = GetConnection())
			using (var cmd = new OleDbCommand(attQuery, con))
			{
				cmd.Parameters.AddWithValue("?", empId);
				cmd.Parameters.AddWithValue("?", periodStart);
				cmd.Parameters.AddWithValue("?", periodEnd);
				con.Open();
				var reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					string status = reader["Status"].ToString();
					var timeIn = reader["TimeIn"] as DateTime?;
					var timeOut = reader["TimeOut"] as DateTime?;

					if (status == "Present" || status == "Late" || status == "Half Day") daysWorked++;
					if (status == "Absent") absences++;

					if (status == "Late" && timeIn.HasValue)
						tardinessMinutes += (int)(timeIn.Value.TimeOfDay - scheduledStart).TotalMinutes;

					if (timeOut.HasValue && timeOut.Value.TimeOfDay > scheduledEnd)
						overtimeHours += (decimal)(timeOut.Value.TimeOfDay - scheduledEnd).TotalHours;
				}
			}
			string leaveQuery = @"SELECT StartDate, EndDate FROM LeaveRequestsTbl
                                  WHERE EmpID = ? AND Status = 'Approved' AND Type = 'Unpaid'
                                  AND ((StartDate <= ? AND EndDate >= ?) OR (StartDate <= ? AND EndDate >= ?))";
			using (var con = GetConnection())
			using (var cmd = new OleDbCommand(leaveQuery, con))
			{
				cmd.Parameters.AddWithValue("?", empId);
				cmd.Parameters.AddWithValue("?", periodEnd);
				cmd.Parameters.AddWithValue("?", periodStart);
				cmd.Parameters.AddWithValue("?", periodEnd);
				cmd.Parameters.AddWithValue("?", periodStart);
				con.Open();
				var reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					var start = Convert.ToDateTime(reader["StartDate"]);
					var end = Convert.ToDateTime(reader["EndDate"]);
					unpaidLeaves += (end - start).Days + 1;
				}
			}
		}
		private void InitializeCalculations()
		{
			EmpCb.SelectedIndexChanged += (s, e) => AggregateAndFillAttendanceFields();
			PayPeriodStartDtp.ValueChanged += (s, e) => AggregateAndFillAttendanceFields();
			PayPeriodEndDtp.ValueChanged += (s, e) => AggregateAndFillAttendanceFields();
			WorkScheduleCb.SelectedIndexChanged += WorkScheduleCb_SelectedIndexChanged;
		}
		private void CalculateValues(object sender, EventArgs e)
		{
			calculationTimer.Stop();
			calculationTimer.Start();
		}
		private void LoadPayrollData()
		{
			try
			{
				using (var conn = GetConnection())
				{
					conn.Open();
					var da = new OleDbDataAdapter("SELECT * FROM PayRollTbl", conn);
					var dt = new DataTable();
					da.Fill(dt);
					PayrollDGV.DataSource = dt;
				}
			}
			catch (Exception ex) { MessageBox.Show("Error loading data: " + ex.Message); }
		}
		private void ClearFormFields()
		{
			BaseSalaryTb.Clear(); WorkScheduleCb.SelectedIndex = -1;
			DWTb.Clear(); OverHrsTb.Clear(); AllowanceTb.Clear();
			TardinessTb.Clear(); AbsencesTb.Clear(); UnpaidLeavesTb.Clear();
			SSSTb.Clear(); PhilHealthTb.Clear(); PagIBIGTb.Clear(); TaxTb.Clear();
			DeducTb.Clear(); GrossPayTb.Clear(); NetPayTb.Clear();
		}
		private void SaveBtn_Click_1(object sender, EventArgs e)
		{
			try
			{
				if (!ValidateInputs()) return;
				CalculatePay();
				// Safe parsing with default values
				decimal baseSalary = decimal.Parse(BaseSalaryTb.Text, CultureInfo.InvariantCulture);
				int daysWorked = int.Parse(DWTb.Text);
				decimal overtimeHours = decimal.Parse(OverHrsTb.Text, CultureInfo.InvariantCulture);
				decimal allowance = decimal.Parse(AllowanceTb.Text, CultureInfo.InvariantCulture);
				int tardiness = int.Parse(TardinessTb.Text);
				int absences = int.Parse(AbsencesTb.Text);
				int unpaidLeaves = int.Parse(UnpaidLeavesTb.Text);

				using (OleDbConnection conn = GetConnection())
				{
					conn.Open();
					string query = @"INSERT INTO PayRollTbl (
					EmpID, PayPeriodStart, PayPeriodEnd, BaseSalary, WorkSchedule,
					DaysWorked, OvertimeHours, Allowance, Deductions, SSS, PhilHealth,
					PagIBIG, WithholdingTax, Tardiness, Absences, UnpaidLeaves, GrossPay, NetPay
					) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
					OleDbCommand cmd = new OleDbCommand(query, conn);
					int empId = ((ComboboxItem)EmpCb.SelectedItem).Value;
					cmd.Parameters.AddWithValue("EmpID", empId);
					cmd.Parameters.AddWithValue("PayPeriodStart", PayPeriodStartDtp.Value.Date);
					cmd.Parameters.AddWithValue("PayPeriodEnd", PayPeriodEndDtp.Value.Date);
					cmd.Parameters.AddWithValue("BaseSalary", decimal.Parse(BaseSalaryTb.Text));
					cmd.Parameters.AddWithValue("WorkSchedule", WorkScheduleCb.SelectedItem.ToString());
					cmd.Parameters.AddWithValue("DaysWorked", int.Parse(DWTb.Text));
					cmd.Parameters.AddWithValue("OvertimeHours", decimal.Parse(OverHrsTb.Text));
					cmd.Parameters.AddWithValue("Allowance", decimal.Parse(AllowanceTb.Text));
					cmd.Parameters.AddWithValue("Deductions", decimal.Parse(DeducTb.Text));
					cmd.Parameters.AddWithValue("SSS", decimal.Parse(SSSTb.Text));
					cmd.Parameters.AddWithValue("PhilHealth", decimal.Parse(PhilHealthTb.Text));
					cmd.Parameters.AddWithValue("PagIBIG", decimal.Parse(PagIBIGTb.Text));
					cmd.Parameters.AddWithValue("WithholdingTax", decimal.Parse(TaxTb.Text));
					cmd.Parameters.AddWithValue("Tardiness", int.Parse(TardinessTb.Text));
					cmd.Parameters.AddWithValue("Absences", int.Parse(AbsencesTb.Text));
					cmd.Parameters.AddWithValue("UnpaidLeaves", int.Parse(UnpaidLeavesTb.Text));
					cmd.Parameters.AddWithValue("GrossPay", decimal.Parse(GrossPayTb.Text));
					cmd.Parameters.AddWithValue("NetPay", decimal.Parse(NetPayTb.Text));

					cmd.ExecuteNonQuery();
					MessageBox.Show("Record saved successfully!");
					LoadPayrollData();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error saving record: " + ex.Message);
			}
		}
		private void UpdateBtn_Click_1(object sender, EventArgs e)
		{
			if (PayrollDGV.SelectedRows.Count == 0) return;

			try
			{
				DataGridViewRow row = PayrollDGV.SelectedRows[0];
				int payrollID = Convert.ToInt32(row.Cells["PayrollID"].Value);

				if (!ValidateInputs()) return;
				CalculatePay();

				using (OleDbConnection conn = GetConnection())
				{
					conn.Open();
					string query = @"UPDATE PayRollTbl SET EmpID = ?, PayPeriodStart = ?, PayPeriodEnd = ?, BaseSalary = ?, WorkSchedule = ?, DaysWorked = ?,OvertimeHours = ?, Allowance = ?, Deductions = ?, SSS = ?, PhilHealth = ?, PagIBIG = ?, WithholdingTax = ?,Tardiness = ?, Absences = ?, UnpaidLeaves = ?, GrossPay = ?, NetPay = ? WHERE PayrollID = ?";
					OleDbCommand cmd = new OleDbCommand(query, conn);
					cmd.Parameters.AddWithValue("EmpID", ((ComboboxItem)EmpCb.SelectedItem).Value);
					cmd.Parameters.AddWithValue("PayPeriodStart", PayPeriodStartDtp.Value.Date);
					cmd.Parameters.AddWithValue("PayPeriodEnd", PayPeriodEndDtp.Value.Date);
					cmd.Parameters.AddWithValue("BaseSalary", decimal.Parse(BaseSalaryTb.Text));
					cmd.Parameters.AddWithValue("WorkSchedule", WorkScheduleCb.SelectedItem.ToString());
					cmd.Parameters.AddWithValue("DaysWorked", int.Parse(DWTb.Text));
					cmd.Parameters.AddWithValue("OvertimeHours", decimal.Parse(OverHrsTb.Text));
					cmd.Parameters.AddWithValue("Allowance", decimal.Parse(AllowanceTb.Text));
					cmd.Parameters.AddWithValue("Deductions", decimal.Parse(DeducTb.Text));
					cmd.Parameters.AddWithValue("SSS", decimal.Parse(SSSTb.Text));
					cmd.Parameters.AddWithValue("PhilHealth", decimal.Parse(PhilHealthTb.Text));
					cmd.Parameters.AddWithValue("PagIBIG", decimal.Parse(PagIBIGTb.Text));
					cmd.Parameters.AddWithValue("WithholdingTax", decimal.Parse(TaxTb.Text));
					cmd.Parameters.AddWithValue("Tardiness", int.Parse(TardinessTb.Text));
					cmd.Parameters.AddWithValue("Absences", int.Parse(AbsencesTb.Text));
					cmd.Parameters.AddWithValue("UnpaidLeaves", int.Parse(UnpaidLeavesTb.Text));
					cmd.Parameters.AddWithValue("GrossPay", decimal.Parse(GrossPayTb.Text));
					cmd.Parameters.AddWithValue("NetPay", decimal.Parse(NetPayTb.Text));
					cmd.Parameters.AddWithValue("PayrollID", payrollID);
					cmd.ExecuteNonQuery();
					MessageBox.Show("Record updated successfully!");
					LoadPayrollData();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error updating record: " + ex.Message);
			}
		}
		private void BackPb_Click(object sender, EventArgs e)
		{
			home.Show(); this.Hide();
		}
		private void DeleteBtn_Click(object sender, EventArgs e)
		{
			if (PayrollDGV.SelectedRows.Count == 0) return;
			try
			{
				int payrollID = Convert.ToInt32(PayrollDGV.SelectedRows[0].Cells["PayrollID"].Value);
				using (OleDbConnection conn = GetConnection())
				{
					conn.Open();
					string query = "DELETE FROM PayRollTbl WHERE PayrollID = ?";
					OleDbCommand cmd = new OleDbCommand(query, conn);
					cmd.Parameters.AddWithValue("PayrollID", payrollID);
					cmd.ExecuteNonQuery();
					MessageBox.Show("Record deleted successfully!");
					LoadPayrollData();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error deleting record: " + ex.Message);
			}
		}
		private bool ValidateInputs()
		{
			var errors = new List<string>();

			// Employee validation
			if (EmpCb.SelectedIndex == -1)
				errors.Add("Employee must be selected");

			// Base Salary validation
			if (string.IsNullOrWhiteSpace(BaseSalaryTb.Text))
				errors.Add("Base Salary is required");
			else if (!decimal.TryParse(BaseSalaryTb.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
				errors.Add("Base Salary must be a valid number");

			// Work Schedule validation
			if (WorkScheduleCb.SelectedIndex == -1)
				errors.Add("Work Schedule must be selected");

			// Days Worked validation
			if (string.IsNullOrWhiteSpace(DWTb.Text))
				errors.Add("Days Worked is required");
			else if (!int.TryParse(DWTb.Text, out int dw) || dw < 0)
				errors.Add("Days Worked must be a positive integer");

			// Overtime validation
			if (string.IsNullOrWhiteSpace(OverHrsTb.Text))
				errors.Add("Overtime Hours is required");
			else if (!decimal.TryParse(OverHrsTb.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
				errors.Add("Overtime Hours must be a valid number");

			// Allowance validation
			if (string.IsNullOrWhiteSpace(AllowanceTb.Text))
				errors.Add("Allowance is required");
			else if (!decimal.TryParse(AllowanceTb.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
				errors.Add("Allowance must be a valid number");

			if (errors.Count > 0)
			{
				MessageBox.Show("Validation Errors:\n- " + string.Join("\n- ", errors),
							  "Input Validation Failed",
							  MessageBoxButtons.OK,
							  MessageBoxIcon.Error);
				return false;
			}
			return true;
		}
		// SSS, PhilHealth, PagIBIG, and Tax calculation methods
		private decimal CalculateSSS(decimal monthlySalary)
		{
			if (monthlySalary <= 3500) return 145.00m;
			if (monthlySalary <= 4000) return 170.00m;
			if (monthlySalary <= 4500) return 195.00m;
			if (monthlySalary <= 5000) return 220.00m;
			if (monthlySalary <= 5500) return 245.00m;
			if (monthlySalary <= 6000) return 270.00m;
			if (monthlySalary <= 6500) return 295.00m;
			if (monthlySalary <= 7000) return 320.00m;
			if (monthlySalary <= 7500) return 345.00m;
			if (monthlySalary <= 8000) return 370.00m;
			if (monthlySalary <= 8500) return 395.00m;
			if (monthlySalary <= 9000) return 420.00m;
			if (monthlySalary <= 9500) return 445.00m;
			if (monthlySalary <= 10000) return 470.00m;
			if (monthlySalary <= 10500) return 495.00m;
			if (monthlySalary <= 11000) return 520.00m;
			if (monthlySalary <= 11500) return 545.00m;
			if (monthlySalary <= 12000) return 570.00m;
			if (monthlySalary <= 12500) return 595.00m;
			if (monthlySalary <= 13000) return 620.00m;
			if (monthlySalary <= 13500) return 645.00m;
			if (monthlySalary <= 14000) return 670.00m;
			if (monthlySalary <= 14500) return 695.00m;
			if (monthlySalary <= 15000) return 720.00m;
			if (monthlySalary <= 15500) return 745.00m;
			if (monthlySalary <= 16000) return 770.00m;
			if (monthlySalary <= 16500) return 795.00m;
			if (monthlySalary <= 17000) return 820.00m;
			if (monthlySalary <= 17500) return 845.00m;
			if (monthlySalary <= 18000) return 870.00m;
			if (monthlySalary <= 18500) return 895.00m;
			if (monthlySalary <= 19000) return 920.00m;
			if (monthlySalary <= 19500) return 945.00m;
			return 970.00m;
		}
		private decimal CalculatePhilHealth(decimal monthlySalary)
		{
			return Math.Min(monthlySalary, 50000) * 0.05m;
		}
		private decimal CalculatePagIBIG(decimal monthlySalary)
		{
			return Math.Min(monthlySalary * 0.02m, 200.00m);
		}
		private decimal CalculateWithholdingTax(decimal taxableIncome)
		{
			taxableIncome /= 1000;
			if (taxableIncome <= 25) return 0;
			if (taxableIncome <= 50) return (taxableIncome - 25) * 0.15m * 1000;
			if (taxableIncome <= 100) return (taxableIncome - 50) * 0.20m * 1000 + 3750;
			if (taxableIncome <= 200) return (taxableIncome - 100) * 0.25m * 1000 + 13750;
			if (taxableIncome <= 400) return (taxableIncome - 200) * 0.30m * 1000 + 38750;
			return (taxableIncome - 400) * 0.35m * 1000 + 98750;
		}
		private void CalculatePay()
		{
			try
			{
				if (!ValidateInputs()) return;

				// Safe parsing with default values
				decimal baseSalary = decimal.Parse(BaseSalaryTb.Text, CultureInfo.InvariantCulture);
				int daysWorked = int.Parse(DWTb.Text);
				decimal overtimeHours = decimal.Parse(OverHrsTb.Text, CultureInfo.InvariantCulture);
				decimal allowance = decimal.Parse(AllowanceTb.Text, CultureInfo.InvariantCulture);

				string schedule = WorkScheduleCb.SelectedItem?.ToString() ?? "Monthly";

				decimal effectiveMonthlySalary = schedule == "Weekly" ? baseSalary * 4.33m : baseSalary;

				decimal dailyRate = schedule == "Weekly" ? baseSalary / 5 : baseSalary / 22;
				decimal hourlyRate = dailyRate / 8;
				decimal overtimePay = overtimeHours * hourlyRate * overtimeRate;

				decimal grossPay = schedule == "Weekly"
					? baseSalary + overtimePay + allowance
					: (daysWorked * dailyRate) + overtimePay + allowance;

				decimal sss = CalculateSSS(effectiveMonthlySalary);
				decimal philhealth = CalculatePhilHealth(effectiveMonthlySalary);
				decimal pagibig = CalculatePagIBIG(effectiveMonthlySalary);
				decimal taxableIncome = grossPay - allowance;
				decimal tax = CalculateWithholdingTax(taxableIncome);

				int tardiness = int.TryParse(TardinessTb.Text, out int t) ? t : 0;
				int absences = int.TryParse(AbsencesTb.Text, out int ab) ? ab : 0;
				int unpaidLeaves = int.TryParse(UnpaidLeavesTb.Text, out int u) ? u : 0;
				decimal tardinessDeduction = (tardiness / 60m) * hourlyRate;
				decimal absenceDeduction = absences * dailyRate;
				decimal unpaidLeaveDeduction = unpaidLeaves * dailyRate;

				decimal totalDeductions = tardinessDeduction + absenceDeduction + unpaidLeaveDeduction + sss + philhealth + pagibig + tax;
				decimal netPay = grossPay - totalDeductions;

				if (netPay < 0)
				{
					MessageBox.Show("Deductions exceed earnings! Review inputs:\n" +
								   "- Ensure base salary matches the selected work schedule.\n" +
								   "- For weekly schedules, base salary should be weekly pay.\n" +
								   "- For monthly schedules, base salary should be monthly pay.",
								   "Calculation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				SSSTb.Text = sss.ToString("N2");
				PhilHealthTb.Text = philhealth.ToString("N2");
				PagIBIGTb.Text = pagibig.ToString("N2");
				TaxTb.Text = tax.ToString("N2");
				DeducTb.Text = totalDeductions.ToString("N2");
				GrossPayTb.Text = grossPay.ToString("N2");
				NetPayTb.Text = netPay.ToString("N2");

			}
			catch (Exception ex)
			{
				MessageBox.Show("Error calculating payroll: " + ex.Message);
			}
		}
		private void PayrollDGV_CellClick_1(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || PayrollDGV.Rows[e.RowIndex].IsNewRow) return;

			DataGridViewRow row = PayrollDGV.Rows[e.RowIndex];

			string GetCellValue(string columnName)
			{
				if (row.Cells[columnName] == null) return "";
				var value = row.Cells[columnName].Value;
				return (value == null || Convert.IsDBNull(value)) ? "" : value.ToString();
			}
			int empId = 0;
			if (!string.IsNullOrEmpty(GetCellValue("EmpID")))
				int.TryParse(GetCellValue("EmpID"), out empId);

			bool found = false;
			foreach (ComboboxItem item in EmpCb.Items)
			{
				if (item.Value == empId)
				{
					EmpCb.SelectedItem = item;
					found = true;
					break;
				}
			}
			if (!found) EmpCb.SelectedIndex = -1;
			BaseSalaryTb.Text = GetCellValue("BaseSalary");
			WorkScheduleCb.SelectedItem = GetCellValue("WorkSchedule");
			DWTb.Text = GetCellValue("DaysWorked");
			OverHrsTb.Text = GetCellValue("OvertimeHours");
			AllowanceTb.Text = GetCellValue("Allowance");
			TardinessTb.Text = GetCellValue("Tardiness");
			AbsencesTb.Text = GetCellValue("Absences");
			UnpaidLeavesTb.Text = GetCellValue("UnpaidLeaves");
			SSSTb.Text = GetCellValue("SSS");
			PhilHealthTb.Text = GetCellValue("PhilHealth");
			PagIBIGTb.Text = GetCellValue("PagIBIG");
			TaxTb.Text = GetCellValue("WithholdingTax");
			DeducTb.Text = GetCellValue("Deductions");
			GrossPayTb.Text = GetCellValue("GrossPay");
			NetPayTb.Text = GetCellValue("NetPay");
			CalculatePay();
		}
		private void ResetDatePickers()
		{
			PayPeriodStartDtp.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
			PayPeriodEndDtp.Value = PayPeriodStartDtp.Value.AddMonths(1).AddDays(-1);
		}
		private void RefreshBtn_Click(object sender, EventArgs e)
		{
			var result = MessageBox.Show("Are you sure you want to refresh? All unsaved changes will be lost.", "Confirm Refresh", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (result == DialogResult.Yes)
			{
				LoadPayrollData();
				LoadEmployees();
				ClearFormFields();
				EmpCb.SelectedIndex = -1;
				ResetDatePickers();
				AggregateAndFillAttendanceFields();
			}
		}
		private void SearchEmployees(string searchTerm)
		{
			EmpCb.Items.Clear();

			if (string.IsNullOrWhiteSpace(searchTerm))
			{
				LoadEmployees(); // Reload all employees if search is empty
				return;
			}

			string query = @"SELECT EmpID, FirstName, LastName FROM EmployeeTbl WHERE CStr(EmpID) LIKE ? OR ([FirstName] & ' ' & [LastName]) LIKE ?";

			using (OleDbConnection con = GetConnection())
			using (OleDbCommand cmd = new OleDbCommand(query, con))
			{
				cmd.Parameters.AddWithValue("Search1", "%" + searchTerm + "%");
				cmd.Parameters.AddWithValue("Search2", "%" + searchTerm + "%");

				con.Open();
				using (OleDbDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						int empId = Convert.ToInt32(reader["EmpID"]);
						string fullName = $"{reader["FirstName"]} {reader["LastName"]}";
						EmpCb.Items.Add(new ComboboxItem(fullName, empId));
					}
				}
			}
		}
		private void SearchBtn_Click(object sender, EventArgs e)
		{
			string searchTerm = SearchTb.Text.Trim();
			SearchEmployees(searchTerm);
		}
		private void ClearSearchBtn_Click(object sender, EventArgs e)
		{
			SearchTb.Clear();
			LoadEmployees();
		}
		private void PrintBtn_Click(object sender, EventArgs e)
		{
			PrintDocument pd = new PrintDocument();
			using (Font headerFont = new Font("Arial", 16, FontStyle.Bold))
			using (Font subHeaderFont = new Font("Arial", 12, FontStyle.Bold))
			using (Font bodyFont = new Font("Arial", 10))
			using (Font netPayFont = new Font("Arial", 14, FontStyle.Bold))
			using (Font italicFont = new Font("Arial", 8, FontStyle.Italic))
			{
				pd.PrintPage += (s, ev) =>
				{
					float y = 60;
					float leftMargin = 80;
					float spacing = 25;

					// Header
					ev.Graphics.DrawString("Company Payroll Report", headerFont, Brushes.Black, leftMargin, y);
					y += spacing * 2;

					// Employee Info
					string employeeName = (EmpCb.SelectedItem as ComboboxItem)?.Text ?? "N/A";
					ev.Graphics.DrawString($"Employee: {employeeName}", bodyFont, Brushes.Black, leftMargin, y); y += spacing;
					ev.Graphics.DrawString($"Pay Period: {PayPeriodStartDtp.Value:MMM dd, yyyy} - {PayPeriodEndDtp.Value:MMM dd, yyyy}", bodyFont, Brushes.Black, leftMargin, y); y += spacing;
					ev.Graphics.DrawString($"Work Schedule: {WorkScheduleCb.SelectedItem?.ToString() ?? "N/A"}", bodyFont, Brushes.Black, leftMargin, y); y += spacing * 2;

					// Earnings
					ev.Graphics.DrawString("EARNINGS", subHeaderFont, Brushes.Black, leftMargin, y); y += spacing;
					ev.Graphics.DrawString($"Base Salary: ₱ {BaseSalaryTb.Text}", bodyFont, Brushes.Black, leftMargin, y); y += spacing;
					ev.Graphics.DrawString($"Days Worked: {DWTb.Text}", bodyFont, Brushes.Black, leftMargin, y); y += spacing;
					ev.Graphics.DrawString($"Overtime Hours: {OverHrsTb.Text}", bodyFont, Brushes.Black, leftMargin, y); y += spacing;
					ev.Graphics.DrawString($"Allowance: ₱ {AllowanceTb.Text}", bodyFont, Brushes.Black, leftMargin, y); y += spacing;
					ev.Graphics.DrawString($"Gross Pay: ₱ {GrossPayTb.Text}", bodyFont, Brushes.Black, leftMargin, y); y += spacing * 2;

					// Deductions
					ev.Graphics.DrawString("DEDUCTIONS", subHeaderFont, Brushes.Black, leftMargin, y); y += spacing;
					ev.Graphics.DrawString($"Tardiness: {TardinessTb.Text} min", bodyFont, Brushes.Black, leftMargin, y); y += spacing;
					ev.Graphics.DrawString($"Absences: {AbsencesTb.Text}", bodyFont, Brushes.Black, leftMargin, y); y += spacing;
					ev.Graphics.DrawString($"Unpaid Leaves: {UnpaidLeavesTb.Text}", bodyFont, Brushes.Black, leftMargin, y); y += spacing;
					ev.Graphics.DrawString($"SSS: ₱ {SSSTb.Text}", bodyFont, Brushes.Black, leftMargin, y); y += spacing;
					ev.Graphics.DrawString($"PhilHealth: ₱ {PhilHealthTb.Text}", bodyFont, Brushes.Black, leftMargin, y); y += spacing;
					ev.Graphics.DrawString($"Pag-IBIG: ₱ {PagIBIGTb.Text}", bodyFont, Brushes.Black, leftMargin, y); y += spacing;
					ev.Graphics.DrawString($"Withholding Tax: ₱ {TaxTb.Text}", bodyFont, Brushes.Black, leftMargin, y); y += spacing;
					ev.Graphics.DrawString($"Total Deductions: ₱ {DeducTb.Text}", bodyFont, Brushes.Black, leftMargin, y); y += spacing * 2;

					// Net Pay
					ev.Graphics.DrawString("NET PAY", subHeaderFont, Brushes.Black, leftMargin, y); y += spacing;
					ev.Graphics.DrawString($"₱ {NetPayTb.Text}", netPayFont, Brushes.Black, leftMargin, y); y += spacing * 2;

					// Footer
					ev.Graphics.DrawString("This is a system-generated payroll statement. For concerns, contact HR.",
						italicFont, Brushes.Gray, leftMargin, y);
				};

				printPreviewDialog1.Document = pd;
				printPreviewDialog1.ShowDialog();
			}
		}
		private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			// Safe cast with null check
			if (!(sender is System.Windows.Forms.TextBox textBox)) return;

			// Handle null/empty text safely
			string currentText = textBox.Text ?? "";
			bool isDecimal = currentText.Contains(".");

			if (!char.IsControl(e.KeyChar))
		
	{
				bool isDigit = char.IsDigit(e.KeyChar);
				bool isDecimalPoint = e.KeyChar == '.' && !isDecimal;

				if (!isDigit && !isDecimalPoint)
				{
					e.Handled = true;
				}
			}
		}
		private void IntegerTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			// Safe cast with null check
			if (!(sender is System.Windows.Forms.TextBox textBox)) return;

			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
			{
				e.Handled = true;
			}
		}
		private void WorkScheduleCb_SelectedIndexChanged(object sender, EventArgs e)
		{
			string newSchedule = WorkScheduleCb.SelectedItem?.ToString();
			if (newSchedule != "Weekly" && newSchedule != "Monthly") return;

			if (decimal.TryParse(BaseSalaryTb.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal currentBase))
			{
				if (newSchedule == "Weekly")
				{
					// Monthly → Weekly
					BaseSalaryTb.Text = (currentBase / 4.33m).ToString("N2");
				}
				else
				{
					// Weekly → Monthly
					BaseSalaryTb.Text = (currentBase * 4.33m).ToString("N2");
				}
			}
		}
		private void ApplyColorPalette()
		{
			// Form background
			this.BackColor = palette.Background;

			// GroupBoxes
			EmpInfoGb.BackColor = palette.Background;
			groupBox1.BackColor = palette.Background;
			groupBox2.BackColor = palette.Background;

			// Labels
			label1.ForeColor = palette.Accent;
			label2.ForeColor = palette.Accent;
			label3.ForeColor = palette.Accent;
			label4.ForeColor = palette.Accent;
			label5.ForeColor = palette.Accent;
			label6.ForeColor = palette.Accent;
			label7.ForeColor = palette.Accent;
			label8.ForeColor = palette.Accent;
			label9.ForeColor = palette.Accent;
			label10.ForeColor = palette.Accent;
			label11.ForeColor = palette.Accent;
			label12.ForeColor = palette.Accent;
			label14.ForeColor = palette.Accent;
			label15.ForeColor = palette.Accent;
			SSSValueLbl.ForeColor = palette.Accent;
			PagIBIGValueLbl.ForeColor = palette.Accent;
			PhilHealthValueLbl.ForeColor = palette.Accent;
			TaxValueLbl.ForeColor = palette.Accent;

			// TextBoxes
			BaseSalaryTb.BackColor = palette.Secondary;
			AllowanceTb.BackColor = palette.Secondary;
			DWTb.BackColor = palette.Secondary;
			OverHrsTb.BackColor = palette.Secondary;
			DeducTb.BackColor = palette.Secondary;
			AbsencesTb.BackColor = palette.Secondary;
			TardinessTb.BackColor = palette.Secondary;
			UnpaidLeavesTb.BackColor = palette.Secondary;
			GrossPayTb.BackColor = palette.Secondary;
			NetPayTb.BackColor = palette.Secondary;
			SSSTb.BackColor = palette.Secondary;
			PhilHealthTb.BackColor = palette.Secondary;
			TaxTb.BackColor = palette.Secondary;
			PagIBIGTb.BackColor = palette.Secondary;
			SearchTb.BackColor = palette.Secondary;
			// ComboBoxes
			EmpCb.FillColor = palette.Secondary;
			WorkScheduleCb.FillColor = palette.Secondary;
			// DataGridView
			PayrollDGV.BackgroundColor = palette.Background;
			PayrollDGV.GridColor = palette.Secondary;
			// Buttons
			SaveBtn.FillColor = palette.Primary;
			SaveBtn.ForeColor = Color.White;
			SaveBtn.HoverState.FillColor = palette.Highlight;
			SaveBtn.HoverState.ForeColor = palette.Accent;

			DeleteBtn.FillColor = palette.Primary;
			DeleteBtn.ForeColor = Color.White;
			DeleteBtn.HoverState.FillColor = palette.Highlight;
			DeleteBtn.HoverState.ForeColor = palette.Accent;

			UpdateBtn.FillColor = palette.Primary;
			UpdateBtn.ForeColor = Color.White;
			UpdateBtn.HoverState.FillColor = palette.Highlight;
			UpdateBtn.HoverState.ForeColor = palette.Accent;

			PrintBtn.FillColor = palette.Primary;
			PrintBtn.ForeColor = Color.White;
			PrintBtn.HoverState.FillColor = palette.Highlight;
			PrintBtn.HoverState.ForeColor = palette.Accent;

			RefreshBtn.FillColor = palette.Accent;
			RefreshBtn.ForeColor = palette.Primary;
			RefreshBtn.HoverState.FillColor = palette.Highlight;
			RefreshBtn.HoverState.ForeColor = Color.White;

			SearchBtn.FillColor = palette.Accent;
			SearchBtn.ForeColor = palette.Primary;
			SearchBtn.HoverState.FillColor = palette.Highlight;
			SearchBtn.HoverState.ForeColor = Color.White;

			ClearSearchBtn.FillColor = palette.Accent;
			ClearSearchBtn.ForeColor = palette.Primary;
			ClearSearchBtn.HoverState.FillColor = palette.Highlight;
			ClearSearchBtn.HoverState.ForeColor = Color.White;
		}
	}
}