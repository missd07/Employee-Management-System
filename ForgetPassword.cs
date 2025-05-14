using System;
using System.Data.OleDb;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace EmpMS
{
	public partial class ForgetPassword : BaseForm
	{
		public ForgetPassword()
		{
			InitializeComponent();
			ApplyColorPalette();
		}
		private readonly IColorPalette palette = new SpiritedAwayBathhousePalette();
		private string verificationCode;
		private string userEmail;
		private string userType;
		// Updated email configuration
		private const string SMTP_EMAIL = "izzydnq@gmail.com";
		private const string SMTP_PASSWORD = "rjvp zwxa zdfn gkct";
		private const string SMTP_HOST = "smtp.gmail.com";
		private const int SMTP_PORT = 587;
		private void SendBtn_Click(object sender, EventArgs e)
		{
			userEmail = EmailTb.Text.Trim();

			if (!IsValidEmail(userEmail))
			{
				MessageBox.Show("Please enter a valid email address");
				return;
			}

			if (!CheckEmailExists(userEmail))
			{
				MessageBox.Show("Email not registered in our system");
				return;
			}

			verificationCode = new Random().Next(100000, 999999).ToString();

			try
			{
				using (SmtpClient client = new SmtpClient(SMTP_HOST, SMTP_PORT))
				{
					client.EnableSsl = true;
					client.DeliveryMethod = SmtpDeliveryMethod.Network; // Explicit network delivery
					client.UseDefaultCredentials = false;
					client.Credentials = new NetworkCredential(SMTP_EMAIL, SMTP_PASSWORD);
					client.Timeout = 10000;

					MailMessage msg = new MailMessage
					{
						From = new MailAddress(SMTP_EMAIL, "Password Reset Service"),
						Subject = "Your Verification Code",
						Body = $"Your verification code is: {verificationCode}",
						IsBodyHtml = false,
						Priority = MailPriority.Normal // Ensure normal priority
					};
					msg.To.Add(userEmail);

					// Add headers to improve deliverability
					msg.Headers.Add("X-Mailer", "EmpMS");
					msg.Headers.Add("X-Priority", "3");

					client.Send(msg);

					MessageBox.Show($"Verification code sent to {userEmail}. Check spam folder if not received.");
				}
			}
			catch (SmtpException smtpEx)
			{
				MessageBox.Show($"SMTP Error: {smtpEx.StatusCode}. Ensure:\n1. 'Less secure apps' is enabled\n2. Using correct app password\n3. Not hitting Gmail's daily limit");
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Failed to send email: {ex.Message}");
			}
		}
		private bool CheckEmailExists(string email)
		{
			try
			{
				using (OleDbConnection conn = GetConnection())
				{
					// Fixed SQL syntax with proper UNION ALL spacing
					string query = @"SELECT 'Admin' FROM AdminTbl WHERE Email = ? UNION ALL SELECT 'Employee' FROM UserTbl WHERE Email = ?";

					using (OleDbCommand cmd = new OleDbCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("Email", email); // First placeholder (?)
						cmd.Parameters.AddWithValue("Email", email); // Second placeholder (?)

						conn.Open();
						object result = cmd.ExecuteScalar();
						userType = result?.ToString();
						return result != null;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Database error: {ex.Message}");
				return false;
			}
		}
		private void VerifyBtn_Click(object sender, EventArgs e)
		{
			if (CodeTb.Text == verificationCode)
			{
				ResetPassword resetForm = new ResetPassword(userEmail, userType);
				resetForm.Show();
				this.Close();
			}
			else
			{
				MessageBox.Show("Invalid verification code. Please check your code.");
			}
		}

		private bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}
		private void ApplyColorPalette()
		{
			// Form background
			this.BackColor = palette.Background;

			// Labels
			label1.ForeColor = palette.Accent;
			label2.ForeColor = palette.Accent;

			// TextBoxes
			EmailTb.FillColor = palette.Secondary;
			CodeTb.FillColor = palette.Secondary; // Only for ForgetPassword

			// Buttons
			SendCodeBtn.FillColor = palette.Primary;
			SendCodeBtn.ForeColor = Color.White;
			SendCodeBtn.HoverState.FillColor = palette.Highlight;
			SendCodeBtn.HoverState.ForeColor = palette.Accent;

			VerifyBtn.FillColor = palette.Primary;
			VerifyBtn.ForeColor = Color.White;
			VerifyBtn.HoverState.FillColor = palette.Highlight;
			VerifyBtn.HoverState.ForeColor = palette.Accent;

			ClearEntriesBtn.FillColor = palette.Accent;
			ClearEntriesBtn.ForeColor = palette.Primary;
			ClearEntriesBtn.HoverState.FillColor = palette.Highlight;
			ClearEntriesBtn.HoverState.ForeColor = Color.White;
		}
	}
}