namespace EmpMS
{
	partial class View_Attendance
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(View_Attendance));
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			AttOvervwLbl = new Label();
			ViewAttElps = new Guna.UI2.WinForms.Guna2Elipse(components);
			AllRtn = new Guna.UI2.WinForms.Guna2RadioButton();
			PresentRtn = new Guna.UI2.WinForms.Guna2RadioButton();
			HDRtn = new Guna.UI2.WinForms.Guna2RadioButton();
			AbsentRtn = new Guna.UI2.WinForms.Guna2RadioButton();
			AttendanceDGV = new Guna.UI2.WinForms.Guna2DataGridView();
			ExitPb = new Guna.UI2.WinForms.Guna2PictureBox();
			((System.ComponentModel.ISupportInitialize)AttendanceDGV).BeginInit();
			((System.ComponentModel.ISupportInitialize)ExitPb).BeginInit();
			SuspendLayout();
			// 
			// AttOvervwLbl
			// 
			AttOvervwLbl.AutoSize = true;
			AttOvervwLbl.Font = new Font("Verdana", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
			AttOvervwLbl.Location = new Point(26, 22);
			AttOvervwLbl.Name = "AttOvervwLbl";
			AttOvervwLbl.Size = new Size(192, 20);
			AttOvervwLbl.TabIndex = 1;
			AttOvervwLbl.Text = "Attendance Overview";
			// 
			// ViewAttElps
			// 
			ViewAttElps.BorderRadius = 35;
			ViewAttElps.TargetControl = this;
			// 
			// AllRtn
			// 
			AllRtn.AutoSize = true;
			AllRtn.CheckedState.BorderColor = Color.FromArgb(94, 148, 255);
			AllRtn.CheckedState.BorderThickness = 0;
			AllRtn.CheckedState.FillColor = Color.FromArgb(94, 148, 255);
			AllRtn.CheckedState.InnerColor = Color.White;
			AllRtn.CheckedState.InnerOffset = -4;
			AllRtn.Location = new Point(307, 66);
			AllRtn.Name = "AllRtn";
			AllRtn.Size = new Size(48, 24);
			AllRtn.TabIndex = 4;
			AllRtn.Text = "All";
			AllRtn.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
			AllRtn.UncheckedState.BorderThickness = 2;
			AllRtn.UncheckedState.FillColor = Color.Transparent;
			AllRtn.UncheckedState.InnerColor = Color.Transparent;
			AllRtn.CheckedChanged += AllRtn_CheckedChanged;
			// 
			// PresentRtn
			// 
			PresentRtn.AutoSize = true;
			PresentRtn.CheckedState.BorderColor = Color.FromArgb(94, 148, 255);
			PresentRtn.CheckedState.BorderThickness = 0;
			PresentRtn.CheckedState.FillColor = Color.FromArgb(94, 148, 255);
			PresentRtn.CheckedState.InnerColor = Color.White;
			PresentRtn.CheckedState.InnerOffset = -4;
			PresentRtn.Location = new Point(361, 66);
			PresentRtn.Name = "PresentRtn";
			PresentRtn.Size = new Size(78, 24);
			PresentRtn.TabIndex = 4;
			PresentRtn.Text = "Present";
			PresentRtn.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
			PresentRtn.UncheckedState.BorderThickness = 2;
			PresentRtn.UncheckedState.FillColor = Color.Transparent;
			PresentRtn.UncheckedState.InnerColor = Color.Transparent;
			PresentRtn.CheckedChanged += PresentRtn_CheckedChanged;
			// 
			// HDRtn
			// 
			HDRtn.AutoSize = true;
			HDRtn.CheckedState.BorderColor = Color.FromArgb(94, 148, 255);
			HDRtn.CheckedState.BorderThickness = 0;
			HDRtn.CheckedState.FillColor = Color.FromArgb(94, 148, 255);
			HDRtn.CheckedState.InnerColor = Color.White;
			HDRtn.CheckedState.InnerOffset = -4;
			HDRtn.Location = new Point(445, 66);
			HDRtn.Name = "HDRtn";
			HDRtn.Size = new Size(90, 24);
			HDRtn.TabIndex = 4;
			HDRtn.Text = "Half-Day";
			HDRtn.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
			HDRtn.UncheckedState.BorderThickness = 2;
			HDRtn.UncheckedState.FillColor = Color.Transparent;
			HDRtn.UncheckedState.InnerColor = Color.Transparent;
			HDRtn.CheckedChanged += HDRtn_CheckedChanged;
			// 
			// AbsentRtn
			// 
			AbsentRtn.AutoSize = true;
			AbsentRtn.CheckedState.BorderColor = Color.FromArgb(94, 148, 255);
			AbsentRtn.CheckedState.BorderThickness = 0;
			AbsentRtn.CheckedState.FillColor = Color.FromArgb(94, 148, 255);
			AbsentRtn.CheckedState.InnerColor = Color.White;
			AbsentRtn.CheckedState.InnerOffset = -4;
			AbsentRtn.Location = new Point(541, 66);
			AbsentRtn.Name = "AbsentRtn";
			AbsentRtn.Size = new Size(76, 24);
			AbsentRtn.TabIndex = 4;
			AbsentRtn.Text = "Absent";
			AbsentRtn.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
			AbsentRtn.UncheckedState.BorderThickness = 2;
			AbsentRtn.UncheckedState.FillColor = Color.Transparent;
			AbsentRtn.UncheckedState.InnerColor = Color.Transparent;
			AbsentRtn.CheckedChanged += AbsentRtn_CheckedChanged;
			// 
			// AttendanceDGV
			// 
			dataGridViewCellStyle1.BackColor = Color.White;
			AttendanceDGV.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
			dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
			dataGridViewCellStyle2.ForeColor = Color.White;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
			AttendanceDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			AttendanceDGV.ColumnHeadersHeight = 4;
			AttendanceDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = Color.White;
			dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
			dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
			dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
			dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
			AttendanceDGV.DefaultCellStyle = dataGridViewCellStyle3;
			AttendanceDGV.GridColor = Color.FromArgb(231, 229, 255);
			AttendanceDGV.Location = new Point(42, 96);
			AttendanceDGV.Name = "AttendanceDGV";
			AttendanceDGV.RowHeadersVisible = false;
			AttendanceDGV.RowHeadersWidth = 51;
			AttendanceDGV.Size = new Size(575, 551);
			AttendanceDGV.TabIndex = 5;
			AttendanceDGV.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
			AttendanceDGV.ThemeStyle.AlternatingRowsStyle.Font = null;
			AttendanceDGV.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
			AttendanceDGV.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
			AttendanceDGV.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
			AttendanceDGV.ThemeStyle.BackColor = Color.White;
			AttendanceDGV.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
			AttendanceDGV.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
			AttendanceDGV.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
			AttendanceDGV.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
			AttendanceDGV.ThemeStyle.HeaderStyle.ForeColor = Color.White;
			AttendanceDGV.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
			AttendanceDGV.ThemeStyle.HeaderStyle.Height = 4;
			AttendanceDGV.ThemeStyle.ReadOnly = false;
			AttendanceDGV.ThemeStyle.RowsStyle.BackColor = Color.White;
			AttendanceDGV.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
			AttendanceDGV.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
			AttendanceDGV.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
			AttendanceDGV.ThemeStyle.RowsStyle.Height = 29;
			AttendanceDGV.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
			AttendanceDGV.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
			// 
			// ExitPb
			// 
			ExitPb.CustomizableEdges = customizableEdges1;
			ExitPb.Image = (Image)resources.GetObject("ExitPb.Image");
			ExitPb.ImageRotate = 0F;
			ExitPb.Location = new Point(598, 12);
			ExitPb.Name = "ExitPb";
			ExitPb.ShadowDecoration.CustomizableEdges = customizableEdges2;
			ExitPb.Size = new Size(45, 36);
			ExitPb.SizeMode = PictureBoxSizeMode.Zoom;
			ExitPb.TabIndex = 12;
			ExitPb.TabStop = false;
			ExitPb.Click += ExitPb_Click;
			// 
			// View_Attendance
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(655, 695);
			Controls.Add(ExitPb);
			Controls.Add(AttendanceDGV);
			Controls.Add(AbsentRtn);
			Controls.Add(HDRtn);
			Controls.Add(PresentRtn);
			Controls.Add(AllRtn);
			Controls.Add(AttOvervwLbl);
			FormBorderStyle = FormBorderStyle.None;
			Name = "View_Attendance";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "View_Attendance";
			((System.ComponentModel.ISupportInitialize)AttendanceDGV).EndInit();
			((System.ComponentModel.ISupportInitialize)ExitPb).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Label AttOvervwLbl;
		private Guna.UI2.WinForms.Guna2Elipse ViewAttElps;
		private Guna.UI2.WinForms.Guna2RadioButton AbsentRtn;
		private Guna.UI2.WinForms.Guna2RadioButton HDRtn;
		private Guna.UI2.WinForms.Guna2RadioButton PresentRtn;
		private Guna.UI2.WinForms.Guna2RadioButton AllRtn;
		private Guna.UI2.WinForms.Guna2DataGridView AttendanceDGV;
		private Guna.UI2.WinForms.Guna2PictureBox ExitPb;
	}
}