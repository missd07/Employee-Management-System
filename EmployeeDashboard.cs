using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EmpMS;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Microsoft.VisualBasic.ApplicationServices;
using System.Drawing.Printing;
using System.Collections;
using Guna.UI2.WinForms.Suite;

namespace EmpMS
{
	public partial class EmployeeDashboard : BaseForm
	{
		public EmployeeDashboard(Login loginForm)
		{
			InitializeComponent();
			_loginForm = loginForm;
			this.FormClosed += (s, args) => _loginForm.Show();
			ApplyColorPalette();
			this.Load += EmployeeDashboard_Load;
			Logoutbtn.Click += Logoutbtn_Click;
			Payslipbtn.Click += Payslipbtn_Click;
			InitializeTooltips();
			this.components = new System.ComponentModel.Container();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
		}
		private Login _loginForm;
		private void EmployeeDashboard_Load(object sender, EventArgs e)
		{
			InitializeCharts();
			AttendancePlot();
			LoadEmployeeData();
			SetDate();
		}
		private readonly IColorPalette palette = new SpiritedAwayBathhousePalette();
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int CurrentEmpId { get; set; }
		private void ClockInbtn_Click(object sender, EventArgs e)
		{
			this.ClockInbtn.BorderRadius = 20;
			try
			{
				using (OleDbConnection con = GetConnection())
				{
					con.Open();
					string checkQuery = "SELECT COUNT(*) FROM AttendanceTbl WHERE EmpID = ? AND [Date] = ? AND TimeOut IS NULL";
					using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, con))
					{
						checkCmd.Parameters.Add("@EmpID", OleDbType.Integer).Value = CurrentEmpId;
						checkCmd.Parameters.Add("@Date", OleDbType.Date).Value = DateTime.Today.Date;
						int existingRecords = (int)checkCmd.ExecuteScalar();
						if (existingRecords > 0)
						{
							MessageBox.Show("You have already clocked in today!");
							return;
						}
					}
					DateTime now = DateTime.Now;
					string status = now.TimeOfDay.Hours >= 9 ? "Late" : "Present";

					string insertQuery = "INSERT INTO AttendanceTbl (EmpID, [Date], TimeIn, Status) VALUES (?, ?, ?, ?)";
					using (OleDbCommand cmd = new OleDbCommand(insertQuery, con))
					{
						cmd.Parameters.Add("@EmpID", OleDbType.Integer).Value = CurrentEmpId;
						cmd.Parameters.Add("@Date", OleDbType.Date).Value = DateTime.Today.Date;
						cmd.Parameters.Add("@TimeIn", OleDbType.Date).Value = now;
						cmd.Parameters.Add("@Status", OleDbType.VarChar, 50).Value = status ?? "";

						cmd.ExecuteNonQuery();
						MessageBox.Show($"Clocked In Successfully! Status: {status}");
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		private void ClockOutbtn_Click(object sender, EventArgs e)
		{
			this.ClockOutbtn.BorderRadius = 20;
			try
			{
				using (OleDbConnection con = GetConnection())
				{
					con.Open();

					string getRecordQuery = @"SELECT TOP 1 TimeIn, Status FROM AttendanceTbl WHERE EmpID = ? AND [Date] = ? AND TimeOut IS NULL ORDER BY TimeIn DESC";

					OleDbCommand getCmd = new OleDbCommand(getRecordQuery, con);
					getCmd.Parameters.Add("@EmpID", OleDbType.Integer).Value = CurrentEmpId;
					getCmd.Parameters.Add("@Date", OleDbType.Date).Value = DateTime.Today.Date;

					DateTime timeIn = DateTime.MinValue;
					string currentStatus = null;

					using (OleDbDataReader reader = getCmd.ExecuteReader())
					{
						if (reader.Read())
						{
							timeIn = reader.GetDateTime(reader.GetOrdinal("TimeIn"));
							currentStatus = reader.GetString(reader.GetOrdinal("Status"));
						}
					}

					if (timeIn == DateTime.MinValue)
					{
						MessageBox.Show("No active clock-in found.");
						return;
					}

					TimeSpan hoursWorked = DateTime.Now - timeIn;

					string finalStatus = currentStatus;
					if (hoursWorked.TotalHours < 4)
					{
						finalStatus = "Half Day";
					}

					string updateQuery = @"UPDATE AttendanceTbl SET TimeOut = ?, Status = ? WHERE EmpID = ? AND [Date] = ? AND TimeOut IS NULL";
					OleDbCommand cmd = new OleDbCommand(updateQuery, con);
					cmd.Parameters.Add("@TimeOut", OleDbType.Date).Value = DateTime.Now;
					cmd.Parameters.Add("@Status", OleDbType.VarChar).Value = finalStatus;
					cmd.Parameters.Add("@EmpID", OleDbType.Integer).Value = CurrentEmpId;
					cmd.Parameters.Add("@Date", OleDbType.Date).Value = DateTime.Today.Date;

					int rows = cmd.ExecuteNonQuery();

					if (rows > 0)
						MessageBox.Show($"Clocked Out Successfully! Final Status: {finalStatus}");
					else
						MessageBox.Show("No active clock-in found.");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		private void RequestLeaveBtn_Click(object sender, EventArgs e)
		{
			this.RequestLeaveBtn.BorderRadius = 10;
			RequestforLeave leaveForm = new RequestforLeave();
			leaveForm.CurrentEmpId = CurrentEmpId;
			leaveForm.Show();
		}
		private void ViewAttbtn_Click(object sender, EventArgs e)
		{
			this.ViewAttbtn.BorderRadius = 10;
			if (CurrentEmpId > 0)
			{
				View_Attendance viewForm = new View_Attendance(CurrentEmpId);
				viewForm.Show();
			}
			else
			{
				MessageBox.Show("No employee ID found. Please log in again.");
			}
		}
		private void AttendancePlot()
		{
			AttendancePv.Model = AttendancePlotModel();
		}
		private PlotModel AttendancePlotModel()
		{
			var plotModel = new PlotModel
			{
				Title = "Attendance Status (Last 7 Days)",
				TitleColor = OxyColor.FromRgb(palette.Accent.R, palette.Accent.G, palette.Accent.B),
				PlotAreaBorderColor = OxyColor.FromRgb(palette.Accent.R, palette.Accent.G, palette.Accent.B)
			};
			var records = FetchAttendanceData();

			var statusGroups = records
				.GroupBy(r => r.Status)
				.Select(g => new { Status = g.Key, Count = g.Count() })
				.ToList();

			var categoryAxis = new CategoryAxis
			{
				Position = AxisPosition.Left,
				Title = "Status",
				AxislineColor = OxyColor.FromRgb(palette.Accent.R, palette.Accent.G, palette.Accent.B),
				TextColor = OxyColor.FromRgb(palette.Accent.R, palette.Accent.G, palette.Accent.B)
			};

			var valueAxis = new LinearAxis
			{
				Position = AxisPosition.Bottom,
				Title = "Days",
				AxislineColor = OxyColor.FromRgb(palette.Accent.R, palette.Accent.G, palette.Accent.B),
				TextColor = OxyColor.FromRgb(palette.Accent.R, palette.Accent.G, palette.Accent.B)
			};
			plotModel.Axes.Add(categoryAxis);
			plotModel.Axes.Add(valueAxis);
			var barSeries = new BarSeries
			{
				FillColor = OxyColor.FromRgb(palette.Highlight.R, palette.Highlight.G, palette.Highlight.B),
				StrokeColor = OxyColor.FromRgb(palette.Primary.R, palette.Primary.G, palette.Primary.B),
				StrokeThickness = 1
			};
			foreach (var group in statusGroups)
			{
				barSeries.Items.Add(new BarItem { Value = group.Count });
				categoryAxis.Labels.Add(group.Status);
			}
			plotModel.Series.Add(barSeries);
			return plotModel;
		}
		private List<AttendanceRecord> FetchAttendanceData()
		{
			var records = new List<AttendanceRecord>();
			try
			{
				using (OleDbConnection con = GetConnection())
				{
					con.Open();
					string query = @"SELECT [Date], TimeIn, Status FROM AttendanceTbl WHERE EmpID = ? AND [Date] >= ? ORDER BY [Date] DESC";

					OleDbCommand cmd = new OleDbCommand(query, con);
					cmd.Parameters.AddWithValue("@EmpID", CurrentEmpId);
					cmd.Parameters.AddWithValue("@Date", DateTime.Today.AddDays(-7).Date);

					using (OleDbDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							DateTime date;
							if (!DateTime.TryParse(reader["Date"].ToString(), out date))
							{
								MessageBox.Show($"Skipping invalid date: {reader["Date"]}");
								continue;
							}

							DateTime timeInDate = reader.GetDateTime(reader.GetOrdinal("TimeIn"));

							records.Add(new AttendanceRecord
							{
								Date = date.Date,
								TimeIn = timeInDate.TimeOfDay,
								Status = reader.GetString(reader.GetOrdinal("Status"))
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error loading attendance data: {ex.Message}");
			}
			return records;
		}
		private class AttendanceRecord
		{
			public DateTime Date { get; set; }
			public TimeSpan TimeIn { get; set; }
			public string Status { get; set; }
		}
		private void Profilebtn_Click(object sender, EventArgs e)
		{
			Profile profileForm = new Profile(CurrentEmpId, _loginForm);
			profileForm.Show();
			this.Hide();
		}
		private void LoadEmployeeData()
		{
			try
			{
				using (OleDbConnection con = GetConnection())
				{
					con.Open();
					string query = "SELECT FirstName, LastName, Position, ProfilePicture FROM EmployeeTbl WHERE EmpID = ?";
					OleDbCommand cmd = new OleDbCommand(query, con);
					cmd.Parameters.Add("@EmpID", OleDbType.Integer).Value = CurrentEmpId;

					using (OleDbDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							string firstName = reader["FirstName"].ToString();
							string lastName = reader["LastName"].ToString();
							string position = reader["Position"].ToString();

							NameLbl.Text = $"{firstName} {lastName}";
							LastNameLbl.Text = lastName;
							PosLbl.Text = position;
							IDLbl.Text = CurrentEmpId.ToString();

							if (reader["ProfilePicture"] != DBNull.Value)
							{
								byte[] imageData = (byte[])reader["ProfilePicture"];
								using (MemoryStream ms = new MemoryStream(imageData))
								{
									ProfilePb.Image = Image.FromStream(ms);
								}
							}
							else
							{
								ProfilePb.Image = null;
							}
						}
						else
						{
							MessageBox.Show("No employee found with this ID.");
							this.Close();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error loading employee data: {ex.Message}");
				this.Close();
			}
		}
		private void SetDate()
		{
			DateTime now = DateTime.Now;
			MDYLbl.Text = DateTime.Now.ToString("MMMM dd, yyyy");
		}
		private void InitializeCharts()
		{
			AllPv.Model = AllPlotModel();
			PresentPv.Model = StatusPlotModel("Present");
			HDPv.Model = StatusPlotModel("Half Day");
			AbsentPv.Model = StatusPlotModel("Absent");
		}
		private PlotModel AllPlotModel()
		{
			var plotModel = new PlotModel { Title = "Total Attendance This Month" };
			var pieSeries = new PieSeries { StrokeThickness = 2.0, InsideLabelPosition = 0.8 };

			int totalDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
			int attendedDays = FetchAttendanceCount(DateTime.Today.AddMonths(0).Date, "ALL");

			pieSeries.Slices.Add(new PieSlice("Attended", attendedDays) { IsExploded = true });
			pieSeries.Slices.Add(new PieSlice("Remaining", totalDays - attendedDays));

			plotModel.Series.Add(pieSeries);
			return plotModel;
		}

		private PlotModel StatusPlotModel(string status)
		{
			var plotModel = new PlotModel { Title = $"{status} Days" };
			var barSeries = new BarSeries
			{
				FillColor = OxyColor.FromRgb(palette.Highlight.R, palette.Highlight.G, palette.Highlight.B)
			};

			int count = FetchAttendanceCount(DateTime.Today.AddMonths(0).Date, status);
			barSeries.Items.Add(new BarItem(count));

			plotModel.Axes.Add(new CategoryAxis { Position = AxisPosition.Left, Labels = { status } });
			plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0, AbsoluteMinimum = 0 });
			plotModel.Series.Add(barSeries);

			return plotModel;
		}
		private int FetchAttendanceCount(DateTime startDate, string status)
		{
			int count = 0;
			try
			{
				using (OleDbConnection con = GetConnection())
				{
					con.Open();
					string query = @"SELECT COUNT(*) FROM AttendanceTbl WHERE EmpID = ? AND [Date] >= ? AND [Date] <= ?";

					if (status != "ALL") query += " AND Status = ?";

					OleDbCommand cmd = new OleDbCommand(query, con);
					cmd.Parameters.AddWithValue("@EmpID", CurrentEmpId);
					cmd.Parameters.AddWithValue("@Start", startDate);
					cmd.Parameters.AddWithValue("@End", DateTime.Today);

					if (status != "ALL")
						cmd.Parameters.AddWithValue("@Status", status);

					count = (int)cmd.ExecuteScalar();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error fetching data: {ex.Message}");
			}
			return count;
		}
		private void Logoutbtn_Click(object sender, EventArgs e)
		{
			this.Logoutbtn.BorderRadius = 5;
			this.Close(); // Close the dashboard
			_loginForm.Show(); // Show the original login form
		}
		private void Payslipbtn_Click(object sender, EventArgs e)
		{
			this.Payslipbtn.BorderRadius = 15;
			if (string.IsNullOrEmpty(IDLbl.Text))
			{
				MessageBox.Show("Employee ID not found.");
				return;
			}
			int empId = int.Parse(IDLbl.Text);
			PrintPaySlip(empId);
		}
		private void PrintPaySlip(int empId)
		{
			var payrollData = GetLatestPayrollData(empId);
			if (payrollData == null)
			{
				MessageBox.Show("No payroll data found.");
				return;
			}

			PrintDocument pd = new PrintDocument();
			pd.PrintPage += (s, ev) => GeneratePaySlipContent(ev.Graphics, empId, payrollData);

			PrintPreviewDialog ppd = new PrintPreviewDialog();
			ppd.Document = pd;
			ppd.ShowDialog();
		}
		public class PayrollData
		{
			public decimal GrossPay { get; set; }
			public decimal NetPay { get; set; }
			public decimal SSS { get; set; }
			public decimal PhilHealth { get; set; }
			public decimal PagIBIG { get; set; }
			public decimal Tax { get; set; }
			public DateTime PeriodStart { get; set; }
			public DateTime PeriodEnd { get; set; }
			public string EmployeeName { get; set; }
			public string Position { get; set; }
			public decimal BasicPay { get; set; }
			public decimal Allowance { get; set; }
			public decimal OvertimePay { get; set; }
			public decimal TotalDeductions { get; set; }
		}
		private PayrollData GetLatestPayrollData(int empId)
		{
			PayrollData data = null;
			using (var connection = GetConnection())
			{
				try
				{
					connection.Open();
					string query = @"SELECT TOP 1 
                            PR.GrossPay, 
                            PR.NetPay, 
                            PR.SSS, 
                            PR.PhilHealth, 
                            PR.PagIBIG, 
                            PR.WithholdingTax AS Tax, 
                            PR.PayPeriodStart, 
                            PR.PayPeriodEnd, 
                            E.FirstName + ' ' + E.LastName AS EmployeeName, 
                            E.Position, 
                            PR.BaseSalary AS BasicPay, 
                            PR.Allowance, 
                            (PR.OvertimeHours * (PR.BaseSalary / 160) * 1.25) AS OvertimePay, 
                            (PR.SSS + PR.PhilHealth + PR.PagIBIG + PR.WithholdingTax) AS TotalDeductions 
                            FROM PayRollTbl PR 
                            INNER JOIN EmployeeTbl E ON PR.EmpID = E.EmpID 
                            WHERE PR.EmpID = @EmpID 
                            ORDER BY PR.PayPeriodEnd DESC";

					using (OleDbCommand cmd = new OleDbCommand(query, connection))
					{
						cmd.Parameters.AddWithValue("@EmpID", empId);
						using (OleDbDataReader reader = cmd.ExecuteReader())
						{
							if (reader.Read())
							{
								data = new PayrollData
								{
									GrossPay = Convert.ToDecimal(reader["GrossPay"]),
									NetPay = Convert.ToDecimal(reader["NetPay"]),
									SSS = Convert.ToDecimal(reader["SSS"]),
									PhilHealth = Convert.ToDecimal(reader["PhilHealth"]),
									PagIBIG = Convert.ToDecimal(reader["PagIBIG"]),
									Tax = Convert.ToDecimal(reader["Tax"]),
									PeriodStart = Convert.ToDateTime(reader["PayPeriodStart"]),
									PeriodEnd = Convert.ToDateTime(reader["PayPeriodEnd"]),
									EmployeeName = reader["EmployeeName"].ToString(),
									Position = reader["Position"].ToString(),
									BasicPay = Convert.ToDecimal(reader["BasicPay"]),
									Allowance = reader["Allowance"] != DBNull.Value ?
											   Convert.ToDecimal(reader["Allowance"]) : 0,
									OvertimePay = Convert.ToDecimal(reader["OvertimePay"]),
									TotalDeductions = Convert.ToDecimal(reader["TotalDeductions"])
								};
							}
							else
							{
								MessageBox.Show("No payroll records found for this employee.");
							}
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Error loading payroll data: {ex.Message}");
				}
			}
			return data;
		}
		private void GeneratePaySlipContent(Graphics g, int empId, PayrollData data)
		{
			Font headerFont = new Font("Arial", 20, FontStyle.Bold);
			Font subHeaderFont = new Font("Arial", 14, FontStyle.Bold);
			Font textFont = new Font("Arial", 12);
			float yPos = 50;
			float leftMargin = 100;

			// Header
			g.DrawString("COMPANY PAYSLIP", headerFont, Brushes.Black, leftMargin, yPos);
			yPos += 40;

			// Employee Info
			g.DrawString($"Employee ID: {empId}", textFont, Brushes.Black, leftMargin, yPos);
			yPos += 25;
			g.DrawString($"Employee Name: {data.EmployeeName}", textFont, Brushes.Black, leftMargin, yPos);
			yPos += 25;
			g.DrawString($"Position: {data.Position}", textFont, Brushes.Black, leftMargin, yPos);
			yPos += 25;
			g.DrawString($"Pay Period: {data.PeriodStart:MMM dd} - {data.PeriodEnd:MMM dd, yyyy}",textFont, Brushes.Black, leftMargin, yPos);
			yPos += 40;

			// Earnings Section
			g.DrawString("EARNINGS", subHeaderFont, Brushes.Black, leftMargin, yPos);
			yPos += 30;
			g.DrawString($"Basic Pay: {data.BasicPay:N2}", textFont, Brushes.Black, leftMargin, yPos);
			yPos += 25;
			g.DrawString($"Allowances: {data.Allowance:N2}", textFont, Brushes.Black, leftMargin, yPos);
			yPos += 25;
			g.DrawString($"Overtime Pay: {data.OvertimePay:N2}", textFont, Brushes.Black, leftMargin, yPos);
			yPos += 40;

			// Deductions Section
			g.DrawString("DEDUCTIONS", subHeaderFont, Brushes.Black, leftMargin, yPos);
			yPos += 30;
			g.DrawString($"SSS: {data.SSS:N2}", textFont, Brushes.Black, leftMargin, yPos);
			yPos += 25;
			g.DrawString($"PhilHealth: {data.PhilHealth:N2}", textFont, Brushes.Black, leftMargin, yPos);
			yPos += 25;
			g.DrawString($"Pag-IBIG: {data.PagIBIG:N2}", textFont, Brushes.Black, leftMargin, yPos);
			yPos += 25;
			g.DrawString($"Withholding Tax: {data.Tax:N2}", textFont, Brushes.Black, leftMargin, yPos);
			yPos += 40;

			// Summary
			g.DrawString("SUMMARY", subHeaderFont, Brushes.Black, leftMargin, yPos);
			yPos += 30;
			g.DrawString($"Gross Pay: {data.GrossPay:N2}", textFont, Brushes.Black, leftMargin, yPos);
			yPos += 25;
			g.DrawString($"Total Deductions: {data.TotalDeductions:N2}", textFont, Brushes.Black, leftMargin, yPos);
			yPos += 25;

			// Net Pay Highlight
			g.DrawString($"NET PAY: {data.NetPay:N2}", new Font("Arial", 14, FontStyle.Bold),Brushes.Black, leftMargin, yPos);
			yPos += 50;

			// Footer Note
			g.DrawString("This is a system-generated payslip. For any inquiries, please contact HR.",new Font("Arial", 10, FontStyle.Italic),Brushes.Gray, leftMargin, yPos);
		}
		private void InitializeTooltips()
		{
			toolTip1.SetToolTip(ClockInbtn, "Click to clock in for the day");
			toolTip1.SetToolTip(ClockOutbtn, "Click to clock out");
			toolTip1.SetToolTip(RequestLeaveBtn, "Request a leave");
			toolTip1.SetToolTip(ViewAttbtn, "View your attendance records");
			toolTip1.SetToolTip(Logoutbtn, "Log out of the system");
			toolTip1.SetToolTip(Payslipbtn, "View and print your payslip");
			toolTip1.SetToolTip(Profilebtn, "View and edit your profile");
		}
		private void ApplyColorPalette()
		{
			// Main panels
			this.BackColor = palette.Background;
			Sidepnl.BackColor = palette.Background;

			// Side panel buttons
			Logoutbtn.FillColor = palette.Primary;
			Logoutbtn.ForeColor = Color.White;
			Logoutbtn.HoverState.FillColor = palette.Highlight;
			Logoutbtn.HoverState.ForeColor = palette.Accent;

			Profilebtn.FillColor = palette.Primary;
			Profilebtn.ForeColor = Color.White;
			Profilebtn.HoverState.FillColor = palette.Highlight;
			Profilebtn.HoverState.ForeColor = palette.Accent;

			Homebtn.FillColor = palette.Primary;
			Homebtn.ForeColor = Color.White;
			Homebtn.HoverState.FillColor = palette.Highlight;
			Homebtn.HoverState.ForeColor = palette.Accent;

			// Top panel buttons
			RequestLeaveBtn.FillColor = palette.Highlight;
			RequestLeaveBtn.ForeColor = Color.White;
			RequestLeaveBtn.HoverState.FillColor = palette.Accent;
			RequestLeaveBtn.HoverState.ForeColor = palette.Primary;

			ClockOutbtn.FillColor = palette.Highlight;
			ClockOutbtn.ForeColor = Color.White;
			ClockOutbtn.HoverState.FillColor = palette.Accent;
			ClockOutbtn.HoverState.ForeColor = palette.Primary;

			ViewAttbtn.FillColor = palette.Highlight;
			ViewAttbtn.ForeColor = Color.White;
			ViewAttbtn.HoverState.FillColor = palette.Accent;
			ViewAttbtn.HoverState.ForeColor = palette.Primary;

			ClockInbtn.FillColor = palette.Highlight;
			ClockInbtn.ForeColor = Color.White;
			ClockInbtn.HoverState.FillColor = palette.Accent;
			ClockInbtn.HoverState.ForeColor = palette.Primary;

			// Labels
			IDLbl.ForeColor = palette.Accent;
			PosLbl.ForeColor = palette.Accent;
			label1.ForeColor = palette.Accent;
			NameLbl.ForeColor = palette.Accent;
			plbl.ForeColor = palette.Accent;
			labelname.ForeColor = palette.Accent;
		}
	}
}