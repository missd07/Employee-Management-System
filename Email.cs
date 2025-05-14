using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EmpMS;
using Guna.UI2.WinForms;

namespace EmpMS
{
	public partial class Email : BaseForm
	{
		public Email()
		{
			InitializeComponent();
			ApplyColorPalette();
		}
		private readonly IColorPalette palette = new SpiritedAwayBathhousePalette();
		private List<string> GetAllEmployeeEmails()
		{
			var emails = new List<string>();
			try
			{
				using (OleDbConnection conn = GetConnection())
				{
					conn.Open();
					OleDbCommand cmd = new OleDbCommand("SELECT Email FROM EmployeeTbl", conn);
					using (OleDbDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							emails.Add(reader["Email"].ToString());
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error fetching emails: {ex.Message}");
			}
			return emails;
		}
		private void SendEmail(string recipient, string subject, string body)
		{
			try
			{
				using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587)) // Replace with your SMTP settings
				{
					client.EnableSsl = true;
					client.Credentials = new NetworkCredential("izzydnq@gmail.com", "rjvp zwxa zdfn gkct");

					MailMessage message = new MailMessage();
					message.From = new MailAddress("izzydnq@gmail.com");
					message.To.Add(recipient);
					message.Subject = subject;
					message.Body = body;

					client.Send(message);
				}
				MessageBox.Show("Email sent successfully!");
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error sending email: {ex.Message}");
			}
		}
		private void Email_Load(object sender, EventArgs e)
		{
			//
		}
		private void SendBtn_Click(object sender, EventArgs e)
		{
			string email = EmailTb.Text.Trim();
			string subject = SubjectTb.Text.Trim();
			string body = BodyTb.Text.Trim();

			if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body))
			{
				MessageBox.Show("Please fill all fields!");
				return;
			}

			SendEmail(email, subject, body);
		}
		private void STABtn_Click(object sender, EventArgs e)
		{
			var emails = GetAllEmployeeEmails();
			string subject = SubjectTb.Text.Trim();
			string body = BodyTb.Text.Trim();

			if (emails.Count == 0 || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body))
			{
				MessageBox.Show("No emails found or fields are empty!");
				return;
			}

			foreach (var email in emails)
			{
				SendEmail(email, subject, body);
			}
		}
		private void CancelBtn_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		private void ApplyColorPalette()
		{
			// Form background
			this.BackColor = palette.Background;

			// Top panel
			TopPnl.BackColor = palette.Primary;

			// Labels
			label1.ForeColor = palette.Accent;
			label2.ForeColor = palette.Accent;
			label3.ForeColor = palette.Accent;

			// TextBoxes
			SubjectTb.FillColor = palette.Secondary;
			EmailTb.FillColor = palette.Secondary;
			BodyTb.BackColor = palette.Secondary; // BodyTb is a standard TextBox, so BackColor

			// Buttons
			SendBtn.FillColor = palette.Primary;
			SendBtn.ForeColor = Color.White;
			SendBtn.HoverState.FillColor = palette.Highlight;
			SendBtn.HoverState.ForeColor = palette.Accent;

			CancelBtn.FillColor = palette.Primary;
			CancelBtn.ForeColor = Color.White;
			CancelBtn.HoverState.FillColor = palette.Highlight;
			CancelBtn.HoverState.ForeColor = palette.Accent;

			STABtn.FillColor = palette.Primary;
			STABtn.ForeColor = Color.White;
			STABtn.HoverState.FillColor = palette.Highlight;
			STABtn.HoverState.ForeColor = palette.Accent;
		}
	}
}