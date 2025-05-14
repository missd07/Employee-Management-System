using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace EmpMS
{
	public partial class ResetPassword : BaseForm
	{
		public ResetPassword(string userEmail, string userType)
		{
			InitializeComponent();
			this.userEmail = userEmail;
			this.userType = userType;
			LoginBtn.Text = "Back to Login";
			LoginBtn.Click += LoginBtn_Click;
			ApplyColorPalette();
		}
		private readonly IColorPalette palette = new SpiritedAwayBathhousePalette();
		private string userEmail;
		private string userType;
		private void ResetBtn_Click(object sender, EventArgs e)
		{
			string newPassword = NewPasswordTb.Text;
			string confirmPassword = ConfirmPasswordTb.Text;

			if (newPassword != confirmPassword)
			{
				MessageBox.Show("Passwords do not match!");
				return;
			}

			// Validate password strength (optional)
			if (newPassword.Length < 6)
			{
				MessageBox.Show("Password must be at least 6 characters!");
				return;
			}

			string updateQuery = userType == "Admin"
				? "UPDATE AdminTbl SET [Password] = ? WHERE [Email] = ?"
				: "UPDATE UserTbl SET [Password] = ? WHERE [Email] = ?";

			using (OleDbConnection connection = GetConnection())
			{
				using (OleDbCommand command = new OleDbCommand(updateQuery, connection))
				{
					// CORRECTED PARAMETER ORDER: Password first, Email second
					command.Parameters.AddWithValue("@Password", newPassword);
					command.Parameters.AddWithValue("@Email", userEmail);

					try
					{
						connection.Open();
						int rowsAffected = command.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							MessageBox.Show("Password updated successfully!");
							new Login().Show(); // Auto-redirect to login
							this.Close();
						}
						else
						{
							MessageBox.Show("No records updated. Email not found.");
						}
					}
					catch (OleDbException ex)
					{
						MessageBox.Show($"Database Error: {ex.Message}\nError Code: {ex.ErrorCode}");
					}
					catch (Exception ex)
					{
						MessageBox.Show($"Error: {ex.Message}");
					}
				}
			}
		}
		private void LoginBtn_Click(object sender, EventArgs e)
		{
			new Login().Show();
			this.Close();
		}
		private void ApplyColorPalette()
		{
			// Form background
			this.BackColor = palette.Background;

			// Labels
			label1.ForeColor = palette.Accent;
			label2.ForeColor = palette.Accent;

			// TextBoxes
			NewPasswordTb.FillColor = palette.Secondary; // Only for ResetPassword
			ConfirmPasswordTb.FillColor = palette.Secondary; // Only for ResetPassword

			ResetBtn.FillColor = palette.Primary;
			ResetBtn.ForeColor = Color.White;
			ResetBtn.HoverState.FillColor = palette.Highlight;
			ResetBtn.HoverState.ForeColor = palette.Accent;

			LoginBtn.FillColor = palette.Accent;
			LoginBtn.ForeColor = palette.Primary;
			LoginBtn.HoverState.FillColor = palette.Highlight;
			LoginBtn.HoverState.ForeColor = Color.White;
		}
	}
}