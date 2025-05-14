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
	public partial class View_Attendance : BaseForm
	{
		public View_Attendance(int currentEmpId)
		{
			InitializeComponent();
			ApplyColorPalette();
			LoadAttendance("All");
			ExitPb.Click += ExitPb_Click;
		}
		private readonly IColorPalette palette = new SpiritedAwayBathhousePalette();
		private readonly int CurrentEmpId;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public static int currentEmpId { get; private set; }

		private void LoadAttendance(string statusFilter)
		{
			string query = "SELECT * FROM AttendanceTbl WHERE EmpID = @EmpID";

			// Add status filter if not "All"
			if (statusFilter != "All")
			{
				query += " AND Status = @Status";
			}

			using (OleDbConnection con = GetConnection())
			{
				OleDbCommand cmd = new OleDbCommand(query, con);
				cmd.Parameters.AddWithValue("@EmpID", CurrentEmpId);

				if (statusFilter != "All")
				{
					cmd.Parameters.AddWithValue("@Status", statusFilter);
				}

				OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
				DataTable dt = new DataTable();
				adapter.Fill(dt);
				AttendanceDGV.DataSource = dt;
			}
		}
		private void AllRtn_CheckedChanged(object sender, EventArgs e)
		{
			if (AllRtn.Checked) LoadAttendance("All");
		}
		private void PresentRtn_CheckedChanged(object sender, EventArgs e)
		{
			if (PresentRtn.Checked) LoadAttendance("Present");
		}
		private void HDRtn_CheckedChanged(object sender, EventArgs e)
		{
			if (HDRtn.Checked) LoadAttendance("Half-Day");
		}
		private void AbsentRtn_CheckedChanged(object sender, EventArgs e)
		{
			if (AbsentRtn.Checked) LoadAttendance("Absent");
		}
		private void ExitPb_Click(object sender, EventArgs e)
		{
			this.Close(); // Close instead of hiding
		}
		private void ApplyColorPalette()
		{
			// Form background
			this.BackColor = palette.Background;

			// DataGridView colors
			AttendanceDGV.BackgroundColor = palette.Background;
			AttendanceDGV.GridColor = palette.Secondary;
			AttendanceDGV.ForeColor = palette.Primary;
			AttOvervwLbl.ForeColor = palette.Accent;
		}
	}
}