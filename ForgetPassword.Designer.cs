namespace EmpMS
{
	partial class ForgetPassword
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
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges19 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges20 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			VerifyBtn = new Guna.UI2.WinForms.Guna2Button();
			SendCodeBtn = new Guna.UI2.WinForms.Guna2Button();
			label2 = new Label();
			label1 = new Label();
			CodeTb = new Guna.UI2.WinForms.Guna2TextBox();
			EmailTb = new Guna.UI2.WinForms.Guna2TextBox();
			ForgetPassElps = new Guna.UI2.WinForms.Guna2Elipse(components);
			ClearEntriesBtn = new Guna.UI2.WinForms.Guna2Button();
			SuspendLayout();
			// 
			// VerifyBtn
			// 
			VerifyBtn.CustomizableEdges = customizableEdges11;
			VerifyBtn.DisabledState.BorderColor = Color.DarkGray;
			VerifyBtn.DisabledState.CustomBorderColor = Color.DarkGray;
			VerifyBtn.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
			VerifyBtn.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
			VerifyBtn.FillColor = Color.RosyBrown;
			VerifyBtn.Font = new Font("Segoe UI", 9F);
			VerifyBtn.ForeColor = Color.White;
			VerifyBtn.Location = new Point(517, 231);
			VerifyBtn.Name = "VerifyBtn";
			VerifyBtn.ShadowDecoration.CustomizableEdges = customizableEdges12;
			VerifyBtn.Size = new Size(136, 49);
			VerifyBtn.TabIndex = 8;
			VerifyBtn.Text = "Verify Code";
			VerifyBtn.Click += VerifyBtn_Click;
			// 
			// SendCodeBtn
			// 
			SendCodeBtn.CustomizableEdges = customizableEdges13;
			SendCodeBtn.DisabledState.BorderColor = Color.DarkGray;
			SendCodeBtn.DisabledState.CustomBorderColor = Color.DarkGray;
			SendCodeBtn.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
			SendCodeBtn.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
			SendCodeBtn.FillColor = Color.RosyBrown;
			SendCodeBtn.Font = new Font("Segoe UI", 9F);
			SendCodeBtn.ForeColor = Color.White;
			SendCodeBtn.Location = new Point(517, 135);
			SendCodeBtn.Name = "SendCodeBtn";
			SendCodeBtn.ShadowDecoration.CustomizableEdges = customizableEdges14;
			SendCodeBtn.Size = new Size(136, 49);
			SendCodeBtn.TabIndex = 9;
			SendCodeBtn.Text = "Send";
			SendCodeBtn.Click += SendBtn_Click;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
			label2.Location = new Point(48, 242);
			label2.Name = "label2";
			label2.Size = new Size(62, 28);
			label2.TabIndex = 6;
			label2.Text = "Code:";
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
			label1.Location = new Point(47, 156);
			label1.Name = "label1";
			label1.Size = new Size(63, 28);
			label1.TabIndex = 7;
			label1.Text = "Email:";
			// 
			// CodeTb
			// 
			CodeTb.CustomizableEdges = customizableEdges15;
			CodeTb.DefaultText = "";
			CodeTb.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
			CodeTb.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
			CodeTb.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
			CodeTb.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
			CodeTb.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
			CodeTb.Font = new Font("Segoe UI", 9F);
			CodeTb.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
			CodeTb.Location = new Point(125, 225);
			CodeTb.Margin = new Padding(3, 4, 3, 4);
			CodeTb.Name = "CodeTb";
			CodeTb.PlaceholderText = "";
			CodeTb.SelectedText = "";
			CodeTb.ShadowDecoration.CustomizableEdges = customizableEdges16;
			CodeTb.Size = new Size(359, 60);
			CodeTb.TabIndex = 4;
			// 
			// EmailTb
			// 
			EmailTb.CustomizableEdges = customizableEdges17;
			EmailTb.DefaultText = "";
			EmailTb.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
			EmailTb.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
			EmailTb.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
			EmailTb.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
			EmailTb.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
			EmailTb.Font = new Font("Segoe UI", 9F);
			EmailTb.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
			EmailTb.Location = new Point(125, 129);
			EmailTb.Margin = new Padding(3, 4, 3, 4);
			EmailTb.Name = "EmailTb";
			EmailTb.PlaceholderText = "";
			EmailTb.SelectedText = "";
			EmailTb.ShadowDecoration.CustomizableEdges = customizableEdges18;
			EmailTb.Size = new Size(359, 60);
			EmailTb.TabIndex = 5;
			// 
			// ForgetPassElps
			// 
			ForgetPassElps.BorderRadius = 10;
			ForgetPassElps.TargetControl = this;
			// 
			// ClearEntriesBtn
			// 
			ClearEntriesBtn.CustomizableEdges = customizableEdges19;
			ClearEntriesBtn.DisabledState.BorderColor = Color.DarkGray;
			ClearEntriesBtn.DisabledState.CustomBorderColor = Color.DarkGray;
			ClearEntriesBtn.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
			ClearEntriesBtn.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
			ClearEntriesBtn.FillColor = Color.RosyBrown;
			ClearEntriesBtn.Font = new Font("Segoe UI", 9F);
			ClearEntriesBtn.ForeColor = Color.White;
			ClearEntriesBtn.Location = new Point(250, 315);
			ClearEntriesBtn.Name = "ClearEntriesBtn";
			ClearEntriesBtn.ShadowDecoration.CustomizableEdges = customizableEdges20;
			ClearEntriesBtn.Size = new Size(136, 49);
			ClearEntriesBtn.TabIndex = 10;
			ClearEntriesBtn.Text = "Clear Entries";
			// 
			// ForgetPassword
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(809, 512);
			Controls.Add(ClearEntriesBtn);
			Controls.Add(VerifyBtn);
			Controls.Add(SendCodeBtn);
			Controls.Add(label2);
			Controls.Add(label1);
			Controls.Add(CodeTb);
			Controls.Add(EmailTb);
			FormBorderStyle = FormBorderStyle.None;
			Name = "ForgetPassword";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "ForgetPassword";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Guna.UI2.WinForms.Guna2Button VerifyBtn;
		private Guna.UI2.WinForms.Guna2Button SendCodeBtn;
		private Label label2;
		private Label label1;
		private Guna.UI2.WinForms.Guna2TextBox CodeTb;
		private Guna.UI2.WinForms.Guna2TextBox EmailTb;
		private Guna.UI2.WinForms.Guna2Elipse ForgetPassElps;
		private Guna.UI2.WinForms.Guna2Button ClearEntriesBtn;
	}
}