namespace EmpMS
{
	partial class RequestforLeave
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
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RequestforLeave));
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			groupBox1 = new GroupBox();
			ReasonTb = new TextBox();
			SubmitBtn = new Guna.UI2.WinForms.Guna2Button();
			ExitPb = new Guna.UI2.WinForms.Guna2PictureBox();
			EndDateDtp = new Guna.UI2.WinForms.Guna2DateTimePicker();
			StartDateDtp = new Guna.UI2.WinForms.Guna2DateTimePicker();
			LeaveTypeCb = new Guna.UI2.WinForms.Guna2ComboBox();
			label4 = new Label();
			label3 = new Label();
			label2 = new Label();
			label1 = new Label();
			RfL = new Guna.UI2.WinForms.Guna2Elipse(components);
			groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)ExitPb).BeginInit();
			SuspendLayout();
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(ReasonTb);
			groupBox1.Controls.Add(SubmitBtn);
			groupBox1.Controls.Add(ExitPb);
			groupBox1.Controls.Add(EndDateDtp);
			groupBox1.Controls.Add(StartDateDtp);
			groupBox1.Controls.Add(LeaveTypeCb);
			groupBox1.Controls.Add(label4);
			groupBox1.Controls.Add(label3);
			groupBox1.Controls.Add(label2);
			groupBox1.Controls.Add(label1);
			groupBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
			groupBox1.Location = new Point(30, 12);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new Size(594, 659);
			groupBox1.TabIndex = 0;
			groupBox1.TabStop = false;
			groupBox1.Text = "Request for Leave";
			// 
			// ReasonTb
			// 
			ReasonTb.Location = new Point(30, 391);
			ReasonTb.Multiline = true;
			ReasonTb.Name = "ReasonTb";
			ReasonTb.Size = new Size(542, 192);
			ReasonTb.TabIndex = 13;
			// 
			// SubmitBtn
			// 
			SubmitBtn.BorderRadius = 10;
			SubmitBtn.CustomizableEdges = customizableEdges1;
			SubmitBtn.DisabledState.BorderColor = Color.DarkGray;
			SubmitBtn.DisabledState.CustomBorderColor = Color.DarkGray;
			SubmitBtn.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
			SubmitBtn.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
			SubmitBtn.FillColor = Color.Maroon;
			SubmitBtn.Font = new Font("Segoe UI", 9F);
			SubmitBtn.ForeColor = Color.White;
			SubmitBtn.Location = new Point(453, 600);
			SubmitBtn.Name = "SubmitBtn";
			SubmitBtn.ShadowDecoration.CustomizableEdges = customizableEdges2;
			SubmitBtn.Size = new Size(135, 51);
			SubmitBtn.TabIndex = 12;
			SubmitBtn.Text = "Submit";
			SubmitBtn.Click += SubmitBtn_Click;
			// 
			// ExitPb
			// 
			ExitPb.CustomizableEdges = customizableEdges3;
			ExitPb.Image = (Image)resources.GetObject("ExitPb.Image");
			ExitPb.ImageRotate = 0F;
			ExitPb.Location = new Point(6, 613);
			ExitPb.Name = "ExitPb";
			ExitPb.ShadowDecoration.CustomizableEdges = customizableEdges4;
			ExitPb.Size = new Size(40, 38);
			ExitPb.SizeMode = PictureBoxSizeMode.Zoom;
			ExitPb.TabIndex = 11;
			ExitPb.TabStop = false;
			ExitPb.Click += ExitPb_Click;
			// 
			// EndDateDtp
			// 
			EndDateDtp.Checked = true;
			EndDateDtp.CustomizableEdges = customizableEdges5;
			EndDateDtp.Font = new Font("Segoe UI", 9F);
			EndDateDtp.Format = DateTimePickerFormat.Long;
			EndDateDtp.Location = new Point(245, 264);
			EndDateDtp.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
			EndDateDtp.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
			EndDateDtp.Name = "EndDateDtp";
			EndDateDtp.ShadowDecoration.CustomizableEdges = customizableEdges6;
			EndDateDtp.Size = new Size(298, 50);
			EndDateDtp.TabIndex = 2;
			EndDateDtp.Value = new DateTime(2025, 4, 27, 15, 33, 36, 607);
			// 
			// StartDateDtp
			// 
			StartDateDtp.Checked = true;
			StartDateDtp.CustomizableEdges = customizableEdges7;
			StartDateDtp.Font = new Font("Segoe UI", 9F);
			StartDateDtp.Format = DateTimePickerFormat.Long;
			StartDateDtp.Location = new Point(245, 162);
			StartDateDtp.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
			StartDateDtp.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
			StartDateDtp.Name = "StartDateDtp";
			StartDateDtp.ShadowDecoration.CustomizableEdges = customizableEdges8;
			StartDateDtp.Size = new Size(298, 50);
			StartDateDtp.TabIndex = 2;
			StartDateDtp.Value = new DateTime(2025, 4, 27, 15, 33, 36, 607);
			// 
			// LeaveTypeCb
			// 
			LeaveTypeCb.BackColor = Color.Transparent;
			LeaveTypeCb.CustomizableEdges = customizableEdges9;
			LeaveTypeCb.DrawMode = DrawMode.OwnerDrawFixed;
			LeaveTypeCb.DropDownStyle = ComboBoxStyle.DropDownList;
			LeaveTypeCb.FocusedColor = Color.FromArgb(94, 148, 255);
			LeaveTypeCb.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
			LeaveTypeCb.Font = new Font("Segoe UI", 10F);
			LeaveTypeCb.ForeColor = Color.FromArgb(68, 88, 112);
			LeaveTypeCb.ItemHeight = 30;
			LeaveTypeCb.Location = new Point(245, 71);
			LeaveTypeCb.Name = "LeaveTypeCb";
			LeaveTypeCb.ShadowDecoration.CustomizableEdges = customizableEdges10;
			LeaveTypeCb.Size = new Size(298, 36);
			LeaveTypeCb.TabIndex = 1;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new Point(44, 350);
			label4.Name = "label4";
			label4.Size = new Size(74, 28);
			label4.TabIndex = 0;
			label4.Text = "Reason";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(44, 275);
			label3.Name = "label3";
			label3.Size = new Size(91, 28);
			label3.TabIndex = 0;
			label3.Text = "End Date";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(44, 174);
			label2.Name = "label2";
			label2.Size = new Size(99, 28);
			label2.TabIndex = 0;
			label2.Text = "Start Date";
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(44, 71);
			label1.Name = "label1";
			label1.Size = new Size(53, 28);
			label1.TabIndex = 0;
			label1.Text = "Type";
			// 
			// RfL
			// 
			RfL.BorderRadius = 10;
			RfL.TargetControl = this;
			// 
			// RequestforLeave
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(655, 695);
			Controls.Add(groupBox1);
			FormBorderStyle = FormBorderStyle.None;
			Name = "RequestforLeave";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "RequestforLeave";
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)ExitPb).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private GroupBox groupBox1;
		private Guna.UI2.WinForms.Guna2DateTimePicker EndDateDtp;
		private Guna.UI2.WinForms.Guna2DateTimePicker StartDateDtp;
		private Guna.UI2.WinForms.Guna2ComboBox LeaveTypeCb;
		private Label label1;
		private Label label4;
		private Label label3;
		private Label label2;
		private Guna.UI2.WinForms.Guna2Elipse RfL;
		private Guna.UI2.WinForms.Guna2PictureBox ExitPb;
		private Guna.UI2.WinForms.Guna2Button SubmitBtn;
		private TextBox ReasonTb;
	}
}