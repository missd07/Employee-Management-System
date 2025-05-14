using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace EmpMS
{
	public partial class Attendance : BaseForm
	{
		private readonly IColorPalette palette = new SpiritedAwayBathhousePalette();
		private Home home;
		public Attendance(Home home)
		{
			InitializeComponent();
			ApplyColorPalette();
			this.Opacity = 0; // Add this line
			this.home = home;
			this.FormClosed += Attendance_FormClosed;
			BackPB.Click += BackPB_Click;
			AttSearchBtn.Click += AttSearchBtn_Click;
			AttClearSearchBtn.Click += AttClearSearchBtn_Click;
			LTSearchBtn.Click += LTSearchBtn_Click;
			LTClearSearchBtn.Click += LTClearSearchBtn_Click;
		}
		private void Attendance_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (home != null && !home.IsDisposed)
				home.Show();
		}
		private void Attendance_Load(object sender, EventArgs e)
		{
			FillEmployee();
			FillStatus();
			FillTime();
			DisplayAttendance();
			DisplayLeaves();
			this.Opacity = 1;
		}
		private void FillEmployee()
		{
			string query = "SELECT EmpID, FirstName, LastName FROM EmployeeTbl";
			using (OleDbConnection con = GetConnection())
			{
				using (OleDbCommand cmd = new OleDbCommand(query, con))
				{
					try
					{
						con.Open();
						OleDbDataReader reader = cmd.ExecuteReader();
						EmpCb.Items.Clear();
						LEmpCb.Items.Clear();

						while (reader.Read())
						{
							int empId = Convert.ToInt32(reader["EmpID"]);
							string firstName = reader["FirstName"].ToString();
							string lastName = reader["LastName"].ToString();
							string employeeName = $"{firstName} {lastName} ({empId})";

							var item = new ComboboxItem(employeeName, empId);
							EmpCb.Items.Add(item);
							LEmpCb.Items.Add(item);
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show("Error loading employee IDs: " + ex.Message);
					}
				}
			}
		}
		private void FillStatus()
		{
			StsCb.Items.Clear();
			StsCb.Items.Add("Present");
			StsCb.Items.Add("Absent");
			StsCb.Items.Add("Late");
			StsCb.Items.Add("Half Day");
		}
		private void FillTime()
		{
			TInAMPMCb.Items.Clear();
			TOutAMPMCb.Items.Clear();

			TInAMPMCb.Items.Add("AM");
			TInAMPMCb.Items.Add("PM");
			TOutAMPMCb.Items.Add("AM");
			TOutAMPMCb.Items.Add("PM");
		}
		private void DisplayAttendance(string searchTerm = null)
		{
			try
			{
				string query = "SELECT AttendanceTbl.*, EmployeeTbl.FirstName, EmployeeTbl.LastName " +
							   "FROM AttendanceTbl " +
							   "INNER JOIN EmployeeTbl ON AttendanceTbl.EmpID = EmployeeTbl.EmpID";

				if (!string.IsNullOrEmpty(searchTerm))
				{
					query += " WHERE (EmployeeTbl.EmpID LIKE ? OR EmployeeTbl.FirstName LIKE ? OR EmployeeTbl.LastName LIKE ?)";
				}

				using (OleDbConnection con = GetConnection())
				{
					using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, con))
					{
						if (!string.IsNullOrEmpty(searchTerm))
						{
							string pattern = $"%{searchTerm}%";
							adapter.SelectCommand.Parameters.AddWithValue("?", pattern);
							adapter.SelectCommand.Parameters.AddWithValue("?", pattern);
							adapter.SelectCommand.Parameters.AddWithValue("?", pattern);
						}

						DataTable dt = new DataTable();
						adapter.Fill(dt);
						AttendanceDGV.DataSource = dt;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error loading attendance: " + ex.Message);
			}
		}
		private DateTime CombineTime(string timeText, string ampm)
		{
			string timeString = $"{timeText} {ampm}";
			return DateTime.ParseExact(timeString, "hh:mm tt", CultureInfo.InvariantCulture);
		}
		private void Clear()
		{
			EmpCb.SelectedIndex = -1;
			DateDtp.Value = DateTime.Now;
			StsCb.SelectedIndex = -1;
			TInTb.Text = "";
			TInAMPMCb.SelectedIndex = -1;
			TOutTb.Text = "";
			TOutAMPMCb.SelectedIndex = -1;
		}
		public class ComboboxItem
		{
			public string Text { get; set; }
			public int Value { get; set; }

			public ComboboxItem(string text, int value)
			{
				Text = text;
				Value = value;
			}
			public override string ToString()
			{
				return Text;
			}
		}
		private void SaveBtn_Click(object sender, EventArgs e)
		{
			if (EmpCb.SelectedIndex == -1 || StsCb.SelectedIndex == -1 || string.IsNullOrEmpty(TInTb.Text) || TInAMPMCb.SelectedIndex == -1 || string.IsNullOrEmpty(TOutTb.Text) || TOutAMPMCb.SelectedIndex == -1)
			{
				MessageBox.Show("Please fill in all required fields");
				return;
			}

			if (!IsValidTimeFormat(TInTb.Text) || !IsValidTimeFormat(TOutTb.Text))
			{
				MessageBox.Show("Invalid time format. Use hh:mm format (e.g., 09:00)");
				return;
			}

			try
			{
				DateTime timeIn = CombineTime(TInTb.Text, TInAMPMCb.SelectedItem.ToString());
				DateTime timeOut = CombineTime(TOutTb.Text, TOutAMPMCb.SelectedItem.ToString());

				string query = @"INSERT INTO AttendanceTbl (EmpID, [Date], [TimeIn], [TimeOut], Status) VALUES (@EmpID, @Date, @TimeIn, @TimeOut, @Status)";

				using (OleDbConnection con = GetConnection())
				using (OleDbCommand cmd = new OleDbCommand(query, con))
				{
					cmd.Parameters.AddWithValue("@EmpID", ((ComboboxItem)EmpCb.SelectedItem).Value);
					cmd.Parameters.AddWithValue("@Date", DateDtp.Value.Date.ToString("yyyy-MM-dd"));
					cmd.Parameters.Add("@TimeIn", OleDbType.DBTimeStamp).Value = timeIn;
					cmd.Parameters.Add("@TimeOut", OleDbType.DBTimeStamp).Value = timeOut;
					cmd.Parameters.AddWithValue("@Status", StsCb.SelectedItem.ToString());

					con.Open();
					cmd.ExecuteNonQuery();
					MessageBox.Show("Attendance saved successfully");
					DisplayAttendance();
					Clear();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error saving attendance: " + ex.Message);
			}
		}
		private void UpdateBtn_Click(object sender, EventArgs e)
		{
			if (AttendanceDGV.SelectedRows.Count == 0)
			{
				MessageBox.Show("Please select a record to update.");
				return;
			}

			if (EmpCb.SelectedIndex == -1 ||
				StsCb.SelectedIndex == -1 ||
				string.IsNullOrEmpty(TInTb.Text) ||
				TInAMPMCb.SelectedIndex == -1 ||
				string.IsNullOrEmpty(TOutTb.Text) ||
				TOutAMPMCb.SelectedIndex == -1)
			{
				MessageBox.Show("Please fill in all required fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			if (!IsValidTimeFormat(TInTb.Text) || !IsValidTimeFormat(TOutTb.Text))
			{
				MessageBox.Show("Invalid time format. Use hh:mm format (e.g., 09:00)");
				return;
			}
			try
			{
				DateTime timeIn = CombineTime(TInTb.Text, TInAMPMCb.SelectedItem.ToString());
				DateTime timeOut = CombineTime(TOutTb.Text, TOutAMPMCb.SelectedItem.ToString());

				int attendanceId = Convert.ToInt32(AttendanceDGV.SelectedRows[0].Cells["AttendanceID"].Value);

				string query = @"UPDATE AttendanceTbl SET EmpID = @EmpID, [Date] = @Date, TimeIn = @TimeIn, TimeOut = @TimeOut, Status = @Status WHERE AttendanceID = @AttendanceID";

				using (OleDbConnection con = GetConnection())
				using (OleDbCommand cmd = new OleDbCommand(query, con))
				{
					cmd.Parameters.AddWithValue("@EmpID", ((ComboboxItem)EmpCb.SelectedItem).Value);
					cmd.Parameters.Add("@Date", OleDbType.Date).Value = DateDtp.Value.Date;
					cmd.Parameters.Add("@TimeIn", OleDbType.DBTimeStamp).Value = timeIn;
					cmd.Parameters.Add("@TimeOut", OleDbType.DBTimeStamp).Value = timeOut;
					cmd.Parameters.AddWithValue("@Status", StsCb.SelectedItem.ToString());
					cmd.Parameters.AddWithValue("@AttendanceID", attendanceId);

					con.Open();
					int rowsAffected = cmd.ExecuteNonQuery();
					if (rowsAffected > 0)
					{
						MessageBox.Show("Attendance updated!");
						DisplayAttendance();
						Clear();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error updating: {ex.Message}");
			}
		}
		private void DeleteBtn_Click(object sender, EventArgs e)
		{
			if (AttendanceDGV.SelectedRows.Count == 0)
			{
				MessageBox.Show("Please select a record to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			try
			{
				int attendanceId = Convert.ToInt32(AttendanceDGV.SelectedRows[0].Cells["AttendanceID"].Value);
				string query = "DELETE FROM AttendanceTbl WHERE AttendanceID = @AttendanceID";

				using (OleDbConnection con = GetConnection())
				using (OleDbCommand cmd = new OleDbCommand(query, con))
				{
					cmd.Parameters.AddWithValue("@AttendanceID", attendanceId);

					con.Open();
					int rowsAffected = cmd.ExecuteNonQuery();

					if (rowsAffected > 0)
					{
						MessageBox.Show("Attendance record deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
						DisplayAttendance();
						Clear();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error deleting attendance: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		private bool IsValidTimeFormat(string time)
		{
			return Regex.IsMatch(time, @"^(0[1-9]|1[0-2]):[0-5][0-9]$");
		}
		private void AttendanceDGV_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0) return;

			try
			{
				DataGridViewRow row = AttendanceDGV.Rows[e.RowIndex];

				// Handle EmpID
				object empIdObj = row.Cells["EmpID"].Value;
				if (empIdObj != DBNull.Value)
				{
					int empId = Convert.ToInt32(empIdObj);
					foreach (ComboboxItem item in EmpCb.Items)
					{
						if (item.Value == empId)
						{
							EmpCb.SelectedItem = item;
							break;
						}
					}
				}
				else
				{
					EmpCb.SelectedIndex = -1;
				}

				// Handle Date
				object dateObj = row.Cells["Date"].Value;
				DateDtp.Value = dateObj != DBNull.Value ? Convert.ToDateTime(dateObj) : DateTime.Now;

				// Handle Status
				object statusObj = row.Cells["Status"].Value;
				if (statusObj != DBNull.Value)
				{
					StsCb.SelectedItem = statusObj.ToString();
				}
				else
				{
					StsCb.SelectedIndex = -1;
				}

				// Handle TimeIn
				object timeInObj = row.Cells["TimeIn"].Value;
				if (timeInObj != DBNull.Value)
				{
					ParseTime(Convert.ToDateTime(timeInObj), TInTb, TInAMPMCb);
				}
				else
				{
					TInTb.Text = "";
					TInAMPMCb.SelectedIndex = -1;
				}

				// Handle TimeOut
				object timeOutObj = row.Cells["TimeOut"].Value;
				if (timeOutObj != DBNull.Value)
				{
					ParseTime(Convert.ToDateTime(timeOutObj), TOutTb, TOutAMPMCb);
				}
				else
				{
					TOutTb.Text = "";
					TOutAMPMCb.SelectedIndex = -1;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error loading record: {ex.Message}");
			}
		}
		private void ParseTime(DateTime time, Guna.UI2.WinForms.Guna2TextBox timeTextBox, ComboBox ampmComboBox)
		{
			timeTextBox.Text = time.ToString("hh:mm");
			ampmComboBox.SelectedItem = time.ToString("tt", CultureInfo.InvariantCulture).ToUpper();
		}
		//Leave Track
		private void DisplayLeaves(string searchTerm = null)
		{
			try
			{
				string query = @"SELECT lr.LeaveRequestID, lr.EmpID, lr.Type, lr.StartDate, lr.EndDate, lr.Reason, lr.Status, 
                                e.FirstName, e.LastName, e.Email 
                         FROM LeaveRequestsTbl lr 
                         INNER JOIN EmployeeTbl e ON lr.EmpID = e.EmpID";

				if (!string.IsNullOrEmpty(searchTerm))
				{
					query += " WHERE (e.EmpID LIKE ? OR e.FirstName LIKE ? OR e.LastName LIKE ?)";
				}

				using (OleDbConnection con = GetConnection())
				{
					using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, con))
					{
						if (!string.IsNullOrEmpty(searchTerm))
						{
							string pattern = $"%{searchTerm}%";
							adapter.SelectCommand.Parameters.AddWithValue("?", pattern);
							adapter.SelectCommand.Parameters.AddWithValue("?", pattern);
							adapter.SelectCommand.Parameters.AddWithValue("?", pattern);
						}

						DataTable dt = new DataTable();
						adapter.Fill(dt);
						LeaveDGV.DataSource = dt;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error loading leaves: " + ex.Message);
			}
		}
		private void ClearLeaveFields()
		{
			LEmpCb.SelectedIndex = -1;
			guna2DateTimePicker2.Value = DateTime.Now;
			guna2DateTimePicker1.Value = DateTime.Now;
			LeaveTypeCb.SelectedIndex = -1;
			ReasonTb.Text = "";
		}
		private void ApproveBtn_Click(object sender, EventArgs e)
		{
			UpdateLeaveStatus("Approved");
		}
		private void DenyBtn_Click(object sender, EventArgs e)
		{
			UpdateLeaveStatus("Denied");
		}
		private void LSaveBtn_Click(object sender, EventArgs e)
		{
			if (LEmpCb.SelectedIndex == -1 || LeaveTypeCb.SelectedIndex == -1 || string.IsNullOrWhiteSpace(ReasonTb.Text))
			{
				MessageBox.Show("Please fill in all leave request fields.");
				return;
			}

			ComboboxItem selectedEmp = (ComboboxItem)LEmpCb.SelectedItem;
			int empId = selectedEmp.Value;

			// ✅ Fix: Use "Type" instead of "LeaveType"
			string insertQuery = "INSERT INTO LeaveRequestsTbl (EmpId, Type, StartDate, EndDate, Reason, Status) VALUES (@EmpId, @Type, @StartDate, @EndDate, @Reason, @Status)";

			using (OleDbConnection con = GetConnection())
			{
				using (OleDbCommand cmd = new OleDbCommand(insertQuery, con))
				{
					cmd.Parameters.AddWithValue("@EmpId", empId);
					cmd.Parameters.AddWithValue("@Type", LeaveTypeCb.SelectedItem.ToString()); // ← Changed here
					cmd.Parameters.AddWithValue("@StartDate", guna2DateTimePicker2.Value.Date);
					cmd.Parameters.AddWithValue("@EndDate", guna2DateTimePicker1.Value.Date);
					cmd.Parameters.AddWithValue("@Reason", ReasonTb.Text);
					cmd.Parameters.AddWithValue("@Status", "Pending");

					try
					{
						con.Open();
						cmd.ExecuteNonQuery();
						MessageBox.Show("Leave request submitted successfully.");
						DisplayLeaves();
						ClearLeaveFields();
					}
					catch (Exception ex)
					{
						MessageBox.Show("Error submitting leave request: " + ex.Message);
					}
				}
			}
		}
		private void UpdateLeaveStatus(string status)
		{
			if (LeaveDGV.SelectedRows.Count == 0) return;

			int leaveId = Convert.ToInt32(LeaveDGV.SelectedRows[0].Cells["LeaveRequestID"].Value);
			string empEmail = LeaveDGV.SelectedRows[0].Cells["Email"].Value.ToString();

			using (OleDbConnection con = GetConnection())
			{
				con.Open();
				string query = "UPDATE LeaveRequestsTbl SET Status = @Status WHERE LeaveRequestID = @ID";
				OleDbCommand cmd = new OleDbCommand(query, con);
				cmd.Parameters.AddWithValue("@Status", status);
				cmd.Parameters.AddWithValue("@ID", leaveId);
				cmd.ExecuteNonQuery();
			}

			SendStatusEmail(status, empEmail);
			DisplayLeaves(); // Refresh grid
		}
		private void LeaveDGV_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0) return;
			DataGridViewRow selectedRow = LeaveDGV.Rows[e.RowIndex];
			int empId = Convert.ToInt32(selectedRow.Cells["EmpId"].Value);
			foreach (ComboboxItem item in LEmpCb.Items)
			{
				if (item.Value == empId)
				{
					LEmpCb.SelectedItem = item;
					break;
				}
			}
			guna2DateTimePicker2.Value = Convert.ToDateTime(selectedRow.Cells["StartDate"].Value);
			guna2DateTimePicker1.Value = Convert.ToDateTime(selectedRow.Cells["EndDate"].Value);
			LeaveTypeCb.SelectedItem = selectedRow.Cells["Type"].Value.ToString();
			ReasonTb.Text = selectedRow.Cells["Reason"].Value.ToString();
		}
		private void BackPB_Click(object sender, EventArgs e)
		{
			this.home.Show();
			this.Hide();
		}
		private void AttSearchBtn_Click(object sender, EventArgs e)
		{
			string searchTerm = AttSearchTb.Text.Trim();
			DisplayAttendance(searchTerm);
		}
		private void AttClearSearchBtn_Click(object sender, EventArgs e)
		{
			AttSearchTb.Clear();
			DisplayAttendance();
		}
		private void LTSearchBtn_Click(object sender, EventArgs e)
		{
			string searchTerm = LTSearchTb.Text.Trim();
			DisplayLeaves(searchTerm);
		}
		private void LTClearSearchBtn_Click(object sender, EventArgs e)
		{
			LTSearchTb.Clear();
			DisplayLeaves();
		}
		private void SendStatusEmail(string status, string employeeEmail)
		{
			try
			{
				var smtpClient = new SmtpClient("smtp.gmail.com")
				{
					Port = 587,
					Credentials = new NetworkCredential("izzydnq@gmail.com", "rjvp zwxa zdfn gkct"),
					EnableSsl = true,
				};

				var mailMessage = new MailMessage
				{
					From = new MailAddress("izzydnq@gmail.com"),
					Subject = $"Leave Request {status}",
					Body = $"Your leave request has been {status.ToLower()}.",
					IsBodyHtml = true,
				};
				mailMessage.To.Add(employeeEmail);

				smtpClient.Send(mailMessage);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error sending email: {ex.Message}");
			}
		}
		private void ApplyColorPalette()
		{
			// Form background
			this.BackColor = palette.Background;

			// Guna2TextBox FillColor
			AttSearchTb.FillColor = Color.Salmon;
			TInTb.FillColor = Color.Salmon;
			TOutTb.FillColor = Color.Salmon;
			ReasonTb.BackColor = Color.Salmon; // Standard TextBox

			// Guna2ComboBox FillColor
			EmpCb.FillColor = Color.Salmon;
			StsCb.FillColor = Color.Salmon;
			//TInAMPMCb.FillColor = Color.Salmon;
			//TOutAMPMCb.FillColor = Color.Salmon;
			LEmpCb.FillColor = Color.Salmon;
			LeaveTypeCb.FillColor = Color.Salmon;
			LStsCb.FillColor = Color.Salmon;

			// Guna2DateTimePicker FillColor
			DateDtp.FillColor = Color.Salmon;
			guna2DateTimePicker1.FillColor = Color.Salmon;
			guna2DateTimePicker2.FillColor = Color.Salmon;

			// BackColor for other controls
			BackPB.BackColor = Color.BurlyWood;

			// TabControl Styling
			AttLeaTc.TabMenuBackColor = Color.IndianRed;
			AttLeaTc.TabButtonIdleState.FillColor = Color.IndianRed;
			AttLeaTc.TabButtonIdleState.ForeColor = Color.Gold;
			AttLeaTc.TabButtonSelectedState.FillColor = Color.Firebrick;
			AttLeaTc.TabButtonSelectedState.ForeColor = Color.Gold;
			AttLeaTc.TabButtonHoverState.FillColor = Color.Salmon;
			AttLeaTc.TabButtonHoverState.ForeColor = Color.Gold;

			// Buttons
			AttClearSearchBtn.FillColor = Color.Salmon;
			AttClearSearchBtn.ForeColor = Color.White;

			AttSearchBtn.FillColor = Color.Salmon;
			AttSearchBtn.ForeColor = Color.White;

			DeleteBtn.FillColor = Color.IndianRed;
			DeleteBtn.ForeColor = Color.White;

			UpdateBtn.FillColor = Color.IndianRed;
			UpdateBtn.ForeColor = Color.White;

			SaveBtn.FillColor = Color.IndianRed;
			SaveBtn.ForeColor = Color.White;

			LTClearSearchBtn.FillColor = Color.Salmon;
			LTClearSearchBtn.ForeColor = Color.White;

			LTSearchBtn.FillColor = Color.Salmon;
			LTSearchBtn.ForeColor = Color.White;

			DenyBtn.FillColor = Color.Firebrick;
			DenyBtn.ForeColor = Color.White;

			ApproveBtn.FillColor = Color.Firebrick;
			ApproveBtn.ForeColor = Color.White;

			LSaveBtn.FillColor = Color.Firebrick;
			LSaveBtn.ForeColor = Color.White;

			// DataGridViews
			AttendanceDGV.BackgroundColor = Color.BurlyWood;
			AttendanceDGV.GridColor = Color.Salmon;

			LeaveDGV.BackgroundColor = Color.BurlyWood;
			LeaveDGV.GridColor = Color.Salmon;

			// GroupBoxes
			this.BackColor = Color.BurlyWood;
			groupBox1.BackColor = Color.BurlyWood;
			groupBox2.BackColor = Color.BurlyWood;

			// Labels
			label1.ForeColor = Color.Gold;
			label2.ForeColor = Color.Gold;
			label3.ForeColor = Color.Gold;
			label4.ForeColor = Color.Gold;
			label5.ForeColor = Color.Gold;
			label6.ForeColor = Color.Gold;
			label7.ForeColor = Color.Gold;
			label8.ForeColor = Color.Gold;
			label9.ForeColor = Color.Gold;
			label10.ForeColor = Color.Gold;
			label11.ForeColor = Color.Gold;
		}
	}
}