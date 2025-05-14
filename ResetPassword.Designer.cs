namespace EmpMS
{
	partial class ResetPassword
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
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			ResetBtn = new Guna.UI2.WinForms.Guna2Button();
			label2 = new Label();
			label1 = new Label();
			ConfirmPasswordTb = new Guna.UI2.WinForms.Guna2TextBox();
			NewPasswordTb = new Guna.UI2.WinForms.Guna2TextBox();
			LoginBtn = new Guna.UI2.WinForms.Guna2Button();
			ResetPassElp = new Guna.UI2.WinForms.Guna2Elipse(components);
			SuspendLayout();
			// 
			// ResetBtn
			// 
			ResetBtn.CustomizableEdges = customizableEdges1;
			ResetBtn.DisabledState.BorderColor = Color.DarkGray;
			ResetBtn.DisabledState.CustomBorderColor = Color.DarkGray;
			ResetBtn.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
			ResetBtn.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
			ResetBtn.FillColor = Color.RosyBrown;
			ResetBtn.Font = new Font("Segoe UI", 9F);
			ResetBtn.ForeColor = Color.White;
			ResetBtn.Location = new Point(332, 323);
			ResetBtn.Name = "ResetBtn";
			ResetBtn.ShadowDecoration.CustomizableEdges = customizableEdges2;
			ResetBtn.Size = new Size(136, 49);
			ResetBtn.TabIndex = 15;
			ResetBtn.Text = "Reset";
			ResetBtn.Click += ResetBtn_Click;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
			label2.Location = new Point(60, 246);
			label2.Name = "label2";
			label2.Size = new Size(172, 28);
			label2.TabIndex = 12;
			label2.Text = "Confirm Password:";
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
			label1.Location = new Point(99, 151);
			label1.Name = "label1";
			label1.Size = new Size(133, 28);
			label1.TabIndex = 13;
			label1.Text = "New Pasword:";
			// 
			// ConfirmPasswordTb
			// 
			ConfirmPasswordTb.CustomizableEdges = customizableEdges3;
			ConfirmPasswordTb.DefaultText = "";
			ConfirmPasswordTb.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
			ConfirmPasswordTb.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
			ConfirmPasswordTb.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
			ConfirmPasswordTb.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
			ConfirmPasswordTb.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
			ConfirmPasswordTb.Font = new Font("Segoe UI", 9F);
			ConfirmPasswordTb.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
			ConfirmPasswordTb.Location = new Point(238, 230);
			ConfirmPasswordTb.Margin = new Padding(3, 4, 3, 4);
			ConfirmPasswordTb.Name = "ConfirmPasswordTb";
			ConfirmPasswordTb.PlaceholderText = "";
			ConfirmPasswordTb.SelectedText = "";
			ConfirmPasswordTb.ShadowDecoration.CustomizableEdges = customizableEdges4;
			ConfirmPasswordTb.Size = new Size(359, 60);
			ConfirmPasswordTb.TabIndex = 10;
			// 
			// NewPasswordTb
			// 
			NewPasswordTb.CustomizableEdges = customizableEdges5;
			NewPasswordTb.DefaultText = "";
			NewPasswordTb.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
			NewPasswordTb.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
			NewPasswordTb.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
			NewPasswordTb.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
			NewPasswordTb.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
			NewPasswordTb.Font = new Font("Segoe UI", 9F);
			NewPasswordTb.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
			NewPasswordTb.Location = new Point(238, 134);
			NewPasswordTb.Margin = new Padding(3, 4, 3, 4);
			NewPasswordTb.Name = "NewPasswordTb";
			NewPasswordTb.PlaceholderText = "";
			NewPasswordTb.SelectedText = "";
			NewPasswordTb.ShadowDecoration.CustomizableEdges = customizableEdges6;
			NewPasswordTb.Size = new Size(359, 60);
			NewPasswordTb.TabIndex = 11;
			// 
			// LoginBtn
			// 
			LoginBtn.CustomizableEdges = customizableEdges7;
			LoginBtn.DisabledState.BorderColor = Color.DarkGray;
			LoginBtn.DisabledState.CustomBorderColor = Color.DarkGray;
			LoginBtn.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
			LoginBtn.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
			LoginBtn.FillColor = Color.RosyBrown;
			LoginBtn.Font = new Font("Segoe UI", 9F);
			LoginBtn.ForeColor = Color.White;
			LoginBtn.Location = new Point(647, 442);
			LoginBtn.Name = "LoginBtn";
			LoginBtn.ShadowDecoration.CustomizableEdges = customizableEdges8;
			LoginBtn.Size = new Size(136, 49);
			LoginBtn.TabIndex = 15;
			LoginBtn.Text = "Login";
			LoginBtn.Click += LoginBtn_Click;
			// 
			// ResetPassElp
			// 
			ResetPassElp.BorderRadius = 10;
			ResetPassElp.TargetControl = this;
			// 
			// ResetPassword
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(809, 512);
			Controls.Add(LoginBtn);
			Controls.Add(ResetBtn);
			Controls.Add(label2);
			Controls.Add(label1);
			Controls.Add(ConfirmPasswordTb);
			Controls.Add(NewPasswordTb);
			FormBorderStyle = FormBorderStyle.None;
			Name = "ResetPassword";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "ResetPassword";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Guna.UI2.WinForms.Guna2Button ResetBtn;
		private Label label2;
		private Label label1;
		private Guna.UI2.WinForms.Guna2TextBox ConfirmPasswordTb;
		private Guna.UI2.WinForms.Guna2TextBox NewPasswordTb;
		private Guna.UI2.WinForms.Guna2Button LoginBtn;
		private Guna.UI2.WinForms.Guna2Elipse ResetPassElp;
	}
}