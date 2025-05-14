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
	public partial class Profile : BaseForm
	{
		public Profile(int empId, Login loginForm)
		{
			InitializeComponent();
			this.Opacity = 0;
			this.empId = empId;
			_loginForm = loginForm;
			EmployeeData();
			ApplyColorPalette();
			this.Opacity = 1;
			Logoutbtn.Click += Logoutbtn_Click;
		}
		private readonly IColorPalette palette = new SpiritedAwayBathhousePalette();
		private int empId;
		private readonly Login _loginForm;
		private void EmployeeData()
		{
			using (var connection = GetConnection())
			{
				connection.Open();
				string query = "SELECT * FROM EmployeeTbl WHERE EmpID = @EmpID";

				using (OleDbCommand cmd = new OleDbCommand(query, connection))
				{
					cmd.Parameters.AddWithValue("@EmpID", empId);

					using (OleDbDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							string firstName = reader["FirstName"].ToString();
							string lastName = reader["LastName"].ToString();
							NameLbl.Text = $"{firstName} {lastName}";
							IDLbl.Text = empId.ToString();
							PosLbl.Text = reader["Position"].ToString();

							FNTb.Text = firstName;
							LNTb.Text = lastName;
							GenderTb.Text = reader["Gender"].ToString();
							DepartmentTb.Text = reader["Department"].ToString();
							PositionTb.Text = reader["Position"].ToString();
							PhoneTb.Text = reader["Phone"].ToString();
							EmailTb.Text = reader["Email"].ToString();
							SDTb.Text = Convert.ToDateTime(reader["StartDate"]).ToShortDateString();
							StatusTb.Text = reader["Status"].ToString();

							if (!(reader["ProfilePicture"] is DBNull))
							{
								byte[] imgData = (byte[])reader["ProfilePicture"];
								using (MemoryStream ms = new MemoryStream(imgData))
								{
									Image profileImage = Image.FromStream(ms);
									PfpPb.Image = profileImage;    // Sidebar picture
									ProfilePb.Image = profileImage; // Main content picture
								}
							}
							else
							{
								var defaultImage = EmpMS.Properties.Resources.DefaultProfileImage;
								PfpPb.Image = defaultImage;
								ProfilePb.Image = defaultImage;
							}
						}
					}
				}
			}
		}
		private void Homebtn_Click(object sender, EventArgs e)
		{
			EmployeeDashboard dashboard = new EmployeeDashboard(_loginForm);
			dashboard.CurrentEmpId = this.empId;
			dashboard.Show();
			this.Hide();
		}
		private void Logoutbtn_Click(object sender, EventArgs e)
		{
			_loginForm.Show();
			this.Close();
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

			ViewAttbtn.FillColor = palette.Highlight;
			ViewAttbtn.ForeColor = Color.White;
			ViewAttbtn.HoverState.FillColor = palette.Accent;
			ViewAttbtn.HoverState.ForeColor = palette.Primary;

			// Labels
			IDLbl.ForeColor = palette.Accent;
			PosLbl.ForeColor = palette.Accent;
			label1.ForeColor = palette.Accent;
			NameLbl.ForeColor = palette.Accent;
			positionlbl.ForeColor = palette.Accent;
			labelname.ForeColor = palette.Accent;

			// Example for textboxes (optional, for a subtle effect)
			FNTb.FillColor = palette.Secondary;
			LNTb.FillColor = palette.Secondary;
			IDTb.FillColor = palette.Secondary;
			GenderTb.FillColor = palette.Secondary;
			PositionTb.FillColor = palette.Secondary;
			DepartmentTb.FillColor = palette.Secondary;
			PhoneTb.FillColor = palette.Secondary;
			SDTb.FillColor = palette.Secondary;
			StatusTb.FillColor = palette.Secondary;
			EmailTb.FillColor = palette.Secondary;
		}
	}
}