namespace EmpMS
{
	partial class Email
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Email));
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			label1 = new Label();
			SubjectTb = new Guna.UI2.WinForms.Guna2TextBox();
			label2 = new Label();
			SendBtn = new Guna.UI2.WinForms.Guna2Button();
			CancelBtn = new Guna.UI2.WinForms.Guna2Button();
			TopPnl = new Panel();
			label3 = new Label();
			EmailTb = new Guna.UI2.WinForms.Guna2TextBox();
			STABtn = new Guna.UI2.WinForms.Guna2Button();
			EmaliElps = new Guna.UI2.WinForms.Guna2Elipse(components);
			BodyTb = new TextBox();
			SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(29, 86);
			label1.Name = "label1";
			label1.Size = new Size(0, 20);
			label1.TabIndex = 1;
			// 
			// SubjectTb
			// 
			SubjectTb.CustomizableEdges = customizableEdges1;
			SubjectTb.DefaultText = "";
			SubjectTb.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
			SubjectTb.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
			SubjectTb.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
			SubjectTb.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
			SubjectTb.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
			SubjectTb.Font = new Font("Segoe UI", 9F);
			SubjectTb.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
			SubjectTb.Location = new Point(107, 165);
			SubjectTb.Margin = new Padding(3, 4, 3, 4);
			SubjectTb.Name = "SubjectTb";
			SubjectTb.PlaceholderText = "";
			SubjectTb.SelectedText = "";
			SubjectTb.ShadowDecoration.CustomizableEdges = customizableEdges2;
			SubjectTb.Size = new Size(368, 38);
			SubjectTb.TabIndex = 2;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
			label2.Location = new Point(31, 174);
			label2.Name = "label2";
			label2.Size = new Size(70, 23);
			label2.TabIndex = 5;
			label2.Text = "Subject:";
			// 
			// SendBtn
			// 
			SendBtn.BorderRadius = 10;
			SendBtn.Cursor = Cursors.Hand;
			SendBtn.CustomizableEdges = customizableEdges3;
			SendBtn.DisabledState.BorderColor = Color.DarkGray;
			SendBtn.DisabledState.CustomBorderColor = Color.DarkGray;
			SendBtn.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
			SendBtn.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
			SendBtn.FillColor = Color.Maroon;
			SendBtn.Font = new Font("Segoe UI", 9F);
			SendBtn.ForeColor = Color.White;
			SendBtn.Image = (Image)resources.GetObject("SendBtn.Image");
			SendBtn.ImageAlign = HorizontalAlignment.Left;
			SendBtn.ImageSize = new Size(25, 25);
			SendBtn.Location = new Point(505, 178);
			SendBtn.Name = "SendBtn";
			SendBtn.ShadowDecoration.CustomizableEdges = customizableEdges4;
			SendBtn.Size = new Size(124, 44);
			SendBtn.TabIndex = 7;
			SendBtn.Text = "Send";
			SendBtn.Click += SendBtn_Click;
			// 
			// CancelBtn
			// 
			CancelBtn.BorderRadius = 10;
			CancelBtn.Cursor = Cursors.Hand;
			CancelBtn.CustomizableEdges = customizableEdges5;
			CancelBtn.DisabledState.BorderColor = Color.DarkGray;
			CancelBtn.DisabledState.CustomBorderColor = Color.DarkGray;
			CancelBtn.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
			CancelBtn.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
			CancelBtn.FillColor = Color.Maroon;
			CancelBtn.Font = new Font("Segoe UI", 9F);
			CancelBtn.ForeColor = Color.White;
			CancelBtn.Image = (Image)resources.GetObject("CancelBtn.Image");
			CancelBtn.ImageSize = new Size(25, 25);
			CancelBtn.Location = new Point(505, 611);
			CancelBtn.Name = "CancelBtn";
			CancelBtn.ShadowDecoration.CustomizableEdges = customizableEdges6;
			CancelBtn.Size = new Size(124, 44);
			CancelBtn.TabIndex = 8;
			CancelBtn.Text = "Cancel";
			CancelBtn.Click += CancelBtn_Click;
			// 
			// TopPnl
			// 
			TopPnl.BorderStyle = BorderStyle.FixedSingle;
			TopPnl.Dock = DockStyle.Top;
			TopPnl.Location = new Point(0, 0);
			TopPnl.Name = "TopPnl";
			TopPnl.Size = new Size(655, 71);
			TopPnl.TabIndex = 9;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
			label3.Location = new Point(31, 122);
			label3.Name = "label3";
			label3.Size = new Size(55, 23);
			label3.TabIndex = 5;
			label3.Text = "Email:";
			// 
			// EmailTb
			// 
			EmailTb.CustomizableEdges = customizableEdges7;
			EmailTb.DefaultText = "";
			EmailTb.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
			EmailTb.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
			EmailTb.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
			EmailTb.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
			EmailTb.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
			EmailTb.Font = new Font("Segoe UI", 9F);
			EmailTb.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
			EmailTb.Location = new Point(107, 113);
			EmailTb.Margin = new Padding(3, 4, 3, 4);
			EmailTb.Name = "EmailTb";
			EmailTb.PlaceholderText = "";
			EmailTb.SelectedText = "";
			EmailTb.ShadowDecoration.CustomizableEdges = customizableEdges8;
			EmailTb.Size = new Size(368, 38);
			EmailTb.TabIndex = 2;
			// 
			// STABtn
			// 
			STABtn.BorderRadius = 10;
			STABtn.Cursor = Cursors.Hand;
			STABtn.CustomizableEdges = customizableEdges9;
			STABtn.DisabledState.BorderColor = Color.DarkGray;
			STABtn.DisabledState.CustomBorderColor = Color.DarkGray;
			STABtn.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
			STABtn.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
			STABtn.FillColor = Color.Maroon;
			STABtn.Font = new Font("Segoe UI", 9F);
			STABtn.ForeColor = Color.White;
			STABtn.Image = (Image)resources.GetObject("STABtn.Image");
			STABtn.ImageAlign = HorizontalAlignment.Left;
			STABtn.ImageSize = new Size(25, 25);
			STABtn.Location = new Point(25, 611);
			STABtn.Name = "STABtn";
			STABtn.ShadowDecoration.CustomizableEdges = customizableEdges10;
			STABtn.Size = new Size(163, 44);
			STABtn.TabIndex = 10;
			STABtn.Text = "Send to All";
			STABtn.Click += STABtn_Click;
			// 
			// EmaliElps
			// 
			EmaliElps.BorderRadius = 10;
			EmaliElps.TargetControl = this;
			// 
			// BodyTb
			// 
			BodyTb.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
			BodyTb.Location = new Point(25, 251);
			BodyTb.Multiline = true;
			BodyTb.Name = "BodyTb";
			BodyTb.Size = new Size(604, 336);
			BodyTb.TabIndex = 11;
			// 
			// Email
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(655, 695);
			Controls.Add(BodyTb);
			Controls.Add(STABtn);
			Controls.Add(TopPnl);
			Controls.Add(CancelBtn);
			Controls.Add(SendBtn);
			Controls.Add(label3);
			Controls.Add(label2);
			Controls.Add(EmailTb);
			Controls.Add(SubjectTb);
			Controls.Add(label1);
			FormBorderStyle = FormBorderStyle.None;
			Name = "Email";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Email";
			Load += Email_Load;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Label label1;
		private Guna.UI2.WinForms.Guna2TextBox SubjectTb;
		private Label label2;
		private Guna.UI2.WinForms.Guna2Button SendBtn;
		private Guna.UI2.WinForms.Guna2Button CancelBtn;
		private Panel TopPnl;
		private Label label3;
		private Guna.UI2.WinForms.Guna2TextBox EmailTb;
		private Guna.UI2.WinForms.Guna2Button STABtn;
		private Guna.UI2.WinForms.Guna2Elipse EmaliElps;
		private TextBox BodyTb;
	}
}