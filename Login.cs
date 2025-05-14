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

namespace EmpMS
{
	public partial class Login : BaseForm
	{
		public Login()
		{
			InitializeComponent();
			PasswordTb.UseSystemPasswordChar = true; // Hide password by default
			ShowPassCb.CheckedChanged += ShowPassCb_CheckedChanged; // Add event handler
			ApplyColorPalette();
		}
		private readonly IColorPalette palette = new SpiritedAwayBathhousePalette();
		private void LoginBtn_Click(object sender, EventArgs e)
		{
			string username = UsernameTb.Text.Trim();
			string password = PasswordTb.Text.Trim();

			using (OleDbConnection connection = GetConnection())
			{
				try
				{
					connection.Open();
					// 1. Admin Check
					string adminQuery = "SELECT COUNT(*) FROM AdminTbl WHERE Username = ? AND [Password] = ?";
					using (OleDbCommand adminCmd = new OleDbCommand(adminQuery, connection))
					{
						adminCmd.Parameters.Add("@Username", OleDbType.VarChar).Value = username;
						adminCmd.Parameters.Add("@Password", OleDbType.VarChar).Value = password;

						if ((int)adminCmd.ExecuteScalar() > 0)
						{
							Home home = new Home(this);
							home.Show();
							this.Hide();
							return;
						}
					}
					// 2. Employee Check
					string employeeQuery = @"SELECT u.EmpID FROM UserTbl u WHERE u.Username = ? AND u.[Password] = ?";
					using (OleDbCommand empCmd = new OleDbCommand(employeeQuery, connection))
					{
						empCmd.Parameters.Add("@Username", OleDbType.VarChar).Value = username;
						empCmd.Parameters.Add("@Password", OleDbType.VarChar).Value = password;

						object result = empCmd.ExecuteScalar();
						if (result != null)
						{
							int empId = Convert.ToInt32(result);
							string validateQuery = "SELECT COUNT(*) FROM EmployeeTbl WHERE EmpID = ?";
							using (OleDbCommand validateCmd = new OleDbCommand(validateQuery, connection))
							{
								validateCmd.Parameters.Add("@EmpID", OleDbType.Integer).Value = empId;
								int exists = (int)validateCmd.ExecuteScalar();

								if (exists > 0)
								{
									EmployeeDashboard empDashboard = new EmployeeDashboard(this); // Pass 'this'
									empDashboard.CurrentEmpId = empId;
									empDashboard.Show();
									this.Hide(); // Hide the login form instead of closing
									return;
								}
								else
								{
									MessageBox.Show("Employee record not found. Contact administrator.");
									return;
								}
							}
						}
					}

					MessageBox.Show("Invalid credentials");
				}
				catch (Exception ex)
				{
					MessageBox.Show("Login error: " + ex.Message);
				}
			}
		}
		private void ClearEntriesBtn_Click(object sender, EventArgs e)
		{
			UsernameTb.Text = "";
			PasswordTb.Text = "";
		}
		private void ForgetPassLbl_Click(object sender, EventArgs e)
		{
			ForgetPassword forgetPasswordForm = new ForgetPassword();
			forgetPasswordForm.Show();
		}
		private void ShowPassCb_CheckedChanged(object sender, EventArgs e)
		{
			PasswordTb.UseSystemPasswordChar = !ShowPassCb.Checked; // Toggle visibility
		}
		private void ApplyColorPalette()
		{
			// Main form and side panel
			this.BackColor = palette.Background;
			SidePnl.BackColor = palette.Primary;

			// Labels
			label1.ForeColor = palette.Accent;
			label2.ForeColor = palette.Accent;
			label3.ForeColor = palette.Accent;
			ForgetPassLbl.ForeColor = palette.Highlight;

			// TextBoxes
			UsernameTb.FillColor = palette.Secondary;
			PasswordTb.FillColor = palette.Secondary;

			// Buttons
			foreach (var btn in new[] { LoginBtn, ClearEntriesBtn })
			{
				btn.FillColor = palette.Primary;
				btn.ForeColor = Color.White;
				btn.HoverState.FillColor = palette.Highlight;
				btn.HoverState.ForeColor = palette.Accent;
			}

			// Checkbox
			ShowPassCb.ForeColor = palette.Highlight;
		}
	}
}