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
using Guna.UI2.WinForms;

namespace EmpMS
{
	public partial class RequestforLeave : BaseForm
	{
		public RequestforLeave()
		{
			InitializeComponent();
			ApplyColorPalette();
			LeaveTypeCb.Items.Add("Sick");
			LeaveTypeCb.Items.Add("Vacation");
			LeaveTypeCb.Items.Add("Personal");
			ExitPb.Click += ExitPb_Click;
		}
		private readonly IColorPalette palette = new SpiritedAwayBathhousePalette();
		private BaseForm baseForm = new BaseForm();
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int CurrentEmpId { get; set; }
		private void SubmitBtn_Click(object sender, EventArgs e)
		{
			if (LeaveTypeCb.SelectedIndex == -1 || string.IsNullOrWhiteSpace(ReasonTb.Text))
			{
				MessageBox.Show("Please fill all fields");
				return;
			}

			using (OleDbConnection con = GetConnection())
			{
				try
				{
					con.Open();

					// Corrected table name and column name to match your schema
					string query = @"INSERT INTO LeaveRequestsTbl (EmpID, Type, StartDate, EndDate, Reason, Status)
                             VALUES (?, ?, ?, ?, ?, 'Pending')";

					using (OleDbCommand cmd = new OleDbCommand(query, con))
					{
						cmd.Parameters.AddWithValue("?", CurrentEmpId); // EmpID
						cmd.Parameters.AddWithValue("?", LeaveTypeCb.SelectedItem.ToString()); // Type (was LeaveType)
						cmd.Parameters.AddWithValue("?", StartDateDtp.Value.Date); // StartDate
						cmd.Parameters.AddWithValue("?", EndDateDtp.Value.Date); // EndDate
						cmd.Parameters.AddWithValue("?", ReasonTb.Text); // Reason

						cmd.ExecuteNonQuery();
						MessageBox.Show("Request submitted!");
						this.Close();
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Database error: {ex.Message}");
				}
			}
		}
		private void ApplyColorPalette()
		{
			// Form and GroupBox background
			this.BackColor = palette.Background;
			groupBox1.BackColor = palette.Background;

			// Labels
			label1.ForeColor = palette.Accent;
			label2.ForeColor = palette.Accent;
			label3.ForeColor = palette.Accent;
			label4.ForeColor = palette.Accent;

			// Buttons
			SubmitBtn.FillColor = palette.Primary;
			SubmitBtn.ForeColor = Color.White;
			SubmitBtn.HoverState.FillColor = palette.Highlight;
			SubmitBtn.HoverState.ForeColor = palette.Accent;

			// ComboBox
			LeaveTypeCb.FillColor = palette.Secondary;
		}

		private void ExitPb_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}