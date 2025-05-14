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
	public partial class Employee : BaseForm
	{
		public Employee(Home home)
		{
			this.Opacity = 0;
			InitializeComponent();
			ApplyColorPalette();
			LoadDepartment();
			LoadStatus();
			this.home = home;
			GenderCb.Items.AddRange(new object[] { "Male", "Female", "Other" });
			GenderCb.SelectedIndex = 0;
			this.LoadBtn.Click += new EventHandler(LoadBtn_Click);
			this.EditBtn.Click += new EventHandler(EditBtn_Click);
			this.DeleteBtn.Click += new EventHandler(DeleteBtn_Click);
			this.EmployeeDGV.CellClick += new DataGridViewCellEventHandler(EmployeeDGV_CellClick);
			this.ProfileBtn.Click += new EventHandler(ProfileBtn_Click);
			this.ReturnPb.Click += new EventHandler(ReturnPb_Click);
			this.Opacity = 1;
		}
		private readonly IColorPalette palette = new SpiritedAwayBathhousePalette();
		private Home home;
		private byte[] profileImageBytes;
		private Dictionary<string, List<string>> Position = new Dictionary<string, List<string>>()
		{
			{ "Finance", new List<string> { "Accountant", "Finance Analyst", "Payroll Officer", "Budget Officer" } },
			{ "Information Technology", new List<string> { "IT Support Specialist", "Software Developer", "Network Administrator", "Systems Analyst" } },
			{ "Customer Service", new List<string> { "Customer Support Agent", "Client Relations Officer", "Call Center Representative", "Helpdesk Support" } },
			{ "Administration", new List<string> { "Administrative Assistant", "Office Clerk", "Executive Secretary", "Document Controller" } }
		};
		private void LoadDepartment()
		{
			DptCb.Items.Add("Information Technology");
			DptCb.Items.Add("Finance");
			DptCb.Items.Add("Administration");
			DptCb.Items.Add("Customer Service");
			DptCb.SelectedIndex = 0;
			LoadPosition(DptCb.SelectedItem.ToString());
		}
		private void LoadPosition(string department)
		{
			PosCb.Items.Clear();

			if (Position.ContainsKey(department))
			{
				foreach (string jobTitle in Position[department])
				{
					PosCb.Items.Add(jobTitle);
				}
				PosCb.SelectedIndex = 0;
			}
		}
		private void LoadStatus()
		{
			StsCb.Items.Add("Active");
			StsCb.Items.Add("Inactive");
			StsCb.SelectedIndex = 0;
		}
		private void Employee_Load(object sender, EventArgs e)
		{
			this.Show();
			LoadEmployees();
		}
		private void DptCb_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadPosition(DptCb.SelectedItem.ToString());
		}
		private void SaveBtn_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(FNTb.Text) || string.IsNullOrWhiteSpace(UsernameTb.Text) || string.IsNullOrWhiteSpace(EmailTb.Text))
			{
				MessageBox.Show("Please fill in all required fields.");
				return;
			}
			using (OleDbConnection con = GetConnection())
			{
				con.Open();
				using (OleDbTransaction transaction = con.BeginTransaction())
				{
					try
					{
						// 1. Insert into EmployeeTbl
						string empQuery = @"INSERT INTO EmployeeTbl 
                    ([FirstName], [LastName], [Gender], [Address], [Phone], [Email], 
                    [Department], [Position], [StartDate], [Status], [ProfilePicture]) 
                    VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

						using (OleDbCommand empCmd = new OleDbCommand(empQuery, con, transaction))
						{
							empCmd.Parameters.Add("@FirstName", OleDbType.VarChar).Value = FNTb.Text.Trim();
							empCmd.Parameters.Add("@LastName", OleDbType.VarChar).Value = LNTb.Text.Trim();
							empCmd.Parameters.Add("@Gender", OleDbType.VarChar).Value = GenderCb.SelectedItem.ToString();
							empCmd.Parameters.Add("@Address", OleDbType.VarChar).Value = AddTb.Text.Trim();
							empCmd.Parameters.Add("@Phone", OleDbType.VarChar).Value = PhoneTb.Text.Trim();
							empCmd.Parameters.Add("@Email", OleDbType.VarChar).Value = EmailTb.Text.Trim();
							empCmd.Parameters.Add("@Department", OleDbType.VarChar).Value = DptCb.SelectedItem.ToString();
							empCmd.Parameters.Add("@Position", OleDbType.VarChar).Value = PosCb.SelectedItem.ToString();
							empCmd.Parameters.Add("@StartDate", OleDbType.Date).Value = SDDtp.Value.Date;
							empCmd.Parameters.Add("@Status", OleDbType.VarChar).Value = StsCb.SelectedItem.ToString();
							empCmd.Parameters.Add("@ProfilePicture", OleDbType.Binary).Value =
								(profileImageBytes != null && profileImageBytes.Length > 0)
									? profileImageBytes
									: (object)DBNull.Value;

							empCmd.ExecuteNonQuery();
						}
						// 2. Get the auto-generated EmpID
						int empId;
						using (OleDbCommand identityCmd = new OleDbCommand("SELECT @@IDENTITY", con, transaction))
						{
							empId = Convert.ToInt32(identityCmd.ExecuteScalar());
						}
						// 3. Validate Unique Username/Email
						string usernameCheckQuery = "SELECT COUNT(*) FROM UserTbl WHERE Username = ?";
						using (OleDbCommand usernameCmd = new OleDbCommand(usernameCheckQuery, con, transaction))
						{
							usernameCmd.Parameters.Add("@Username", OleDbType.VarChar).Value = UsernameTb.Text.Trim();
							if ((int)usernameCmd.ExecuteScalar() > 0)
							{
								MessageBox.Show("Username already exists.");
								transaction.Rollback();
								return;
							}
						}
						string emailCheckQuery = "SELECT COUNT(*) FROM UserTbl WHERE Email = ?";
						using (OleDbCommand emailCmd = new OleDbCommand(emailCheckQuery, con, transaction))
						{
							emailCmd.Parameters.Add("@Email", OleDbType.VarChar).Value = EmailTb.Text.Trim();
							if ((int)emailCmd.ExecuteScalar() > 0)
							{
								MessageBox.Show("Email already exists.");
								transaction.Rollback();
								return;
							}
						}
						// 4. Insert into UserTbl
						string userQuery = @"INSERT INTO UserTbl ([Username], [Email], [Password], [EmpID]) 
                                   VALUES (?, ?, '1234', ?)";
						using (OleDbCommand userCmd = new OleDbCommand(userQuery, con, transaction))
						{
							userCmd.Parameters.Add("@Username", OleDbType.VarChar).Value = UsernameTb.Text.Trim();
							userCmd.Parameters.Add("@Email", OleDbType.VarChar).Value = EmailTb.Text.Trim();
							userCmd.Parameters.Add("@EmpID", OleDbType.Integer).Value = empId;
							userCmd.ExecuteNonQuery();
						}
						transaction.Commit();
						MessageBox.Show("Employee Created!");
						LoadEmployees();
						Clear();
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						MessageBox.Show("Error saving employee: " + ex.Message);
					}
				}
			}
		}
		private void LoadEmployees()
		{
			using (var connection = GetConnection())
			{
				try
				{
					connection.Open();
					string query = "SELECT * FROM EmployeeTbl";
					using (var adapter = new OleDbDataAdapter(query, connection))
					{
						var dt = new DataTable();
						adapter.Fill(dt);
						EmployeeDGV.DataSource = dt;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error loading employees: " + ex.Message);
				}
			}
		}
		private void EditBtn_Click(object sender, EventArgs e)
		{
			if (EmployeeDGV.SelectedRows.Count > 0)
			{
				using (OleDbConnection con = GetConnection())
				{
					con.Open();
					using (OleDbTransaction transaction = con.BeginTransaction())
					{
						try
						{
							int empId = Convert.ToInt32(EmployeeDGV.SelectedRows[0].Cells["EmpID"].Value);

							// 1. Update EmployeeTbl
							string empQuery = @"UPDATE EmployeeTbl SET [FirstName] = ?, [LastName] = ?, [Gender] = ?, [Address] = ?, [Phone] = ?, [Email] = ?, [Department] = ?, [Position] = ?, [StartDate] = ?, [Status] = ?, [ProfilePicture] = ?  WHERE [EmpID] = ?";

							using (OleDbCommand empCmd = new OleDbCommand(empQuery, con, transaction))
							{
								// Add parameters IN THE ORDER of the SQL query
								empCmd.Parameters.Add("@FirstName", OleDbType.VarChar).Value = FNTb.Text.Trim();
								empCmd.Parameters.Add("@LastName", OleDbType.VarChar).Value = LNTb.Text.Trim();
								empCmd.Parameters.Add("@Gender", OleDbType.VarChar).Value = GenderCb.SelectedItem.ToString();
								empCmd.Parameters.Add("@Address", OleDbType.VarChar).Value = AddTb.Text.Trim();
								empCmd.Parameters.Add("@Phone", OleDbType.VarChar).Value = PhoneTb.Text.Trim();
								empCmd.Parameters.Add("@Email", OleDbType.VarChar).Value = EmailTb.Text.Trim(); // Employee's contact email
								empCmd.Parameters.Add("@Department", OleDbType.VarChar).Value = DptCb.SelectedItem.ToString();
								empCmd.Parameters.Add("@Position", OleDbType.VarChar).Value = PosCb.SelectedItem.ToString();
								empCmd.Parameters.Add("@StartDate", OleDbType.Date).Value = SDDtp.Value.Date;
								empCmd.Parameters.Add("@Status", OleDbType.VarChar).Value = StsCb.SelectedItem.ToString();
								empCmd.Parameters.Add("@ProfilePicture", OleDbType.Binary).Value =
	(profileImageBytes != null && profileImageBytes.Length > 0)
		? profileImageBytes
		: (object)DBNull.Value;
								empCmd.Parameters.Add("@EmpID", OleDbType.Integer).Value = empId; // For WHERE clause

								empCmd.ExecuteNonQuery();
							}
							// 2. Validate Unique Username/Email in UserTbl (if changed)
							string newUsername = UsernameTb.Text.Trim();
							string newEmail = EmailTb.Text.Trim();
							// Check if Username is updated and unique
							string usernameCheckQuery = @"SELECT COUNT(*) FROM UserTbl WHERE Username = ? AND UserID <> (SELECT UserID FROM UserTbl WHERE EmpID = ?)";
							using (OleDbCommand usernameCmd = new OleDbCommand(usernameCheckQuery, con, transaction))
							{
								usernameCmd.Parameters.Add("@Username", OleDbType.VarChar).Value = newUsername;
								usernameCmd.Parameters.Add("@EmpID", OleDbType.Integer).Value = empId;
								if ((int)usernameCmd.ExecuteScalar() > 0)
								{
									MessageBox.Show("Username already exists.");
									transaction.Rollback();
									return;
								}
							}
							// Check if Email is updated and unique
							string emailCheckQuery = @"SELECT COUNT(*) FROM UserTbl WHERE Email = ? AND UserID <> (SELECT UserID FROM UserTbl WHERE EmpID = ?)";
							using (OleDbCommand emailCmd = new OleDbCommand(emailCheckQuery, con, transaction))
							{
								emailCmd.Parameters.Add("@Email", OleDbType.VarChar).Value = newEmail;
								emailCmd.Parameters.Add("@EmpID", OleDbType.Integer).Value = empId;
								if ((int)emailCmd.ExecuteScalar() > 0)
								{
									MessageBox.Show("Email already exists.");
									transaction.Rollback();
									return;
								}
							}
							// 3. Update UserTbl
							string userQuery = @"UPDATE UserTbl SET [Username] = ?, [Email] = ? WHERE [EmpID] = ?";
							using (OleDbCommand userCmd = new OleDbCommand(userQuery, con, transaction))
							{
								userCmd.Parameters.Add("@Username", OleDbType.VarChar).Value = newUsername;
								userCmd.Parameters.Add("@Email", OleDbType.VarChar).Value = newEmail;
								userCmd.Parameters.Add("@EmpID", OleDbType.Integer).Value = empId;
								userCmd.ExecuteNonQuery();
							}
							transaction.Commit();
							MessageBox.Show("Employee updated successfully.");
							LoadEmployees();
							Clear();
						}
						catch (Exception ex)
						{
							transaction.Rollback();
							MessageBox.Show("Error updating employee: " + ex.Message);
						}
					}
				}
			}
			else
			{
				MessageBox.Show("Please select an employee to edit.");
			}
		}
		private void DeleteBtn_Click(object sender, EventArgs e)
		{
			if (EmployeeDGV.SelectedRows.Count > 0)
			{
				using (OleDbConnection con = GetConnection())
				{
					con.Open();
					try
					{
						int empId = Convert.ToInt32(EmployeeDGV.SelectedRows[0].Cells["EmpID"].Value);

						// 1. Delete from UserTbl first
						string deleteUserQuery = "DELETE FROM UserTbl WHERE EmpID = ?";
						using (OleDbCommand userCmd = new OleDbCommand(deleteUserQuery, con))
						{
							userCmd.Parameters.Add("@EmpID", OleDbType.Integer).Value = empId;
							userCmd.ExecuteNonQuery();
						}

						// 2. Delete from EmployeeTbl
						string deleteEmpQuery = "DELETE FROM EmployeeTbl WHERE EmpID = ?";
						using (OleDbCommand empCmd = new OleDbCommand(deleteEmpQuery, con))
						{
							empCmd.Parameters.Add("@EmpID", OleDbType.Integer).Value = empId;
							empCmd.ExecuteNonQuery();
						}

						MessageBox.Show("Employee deleted successfully.");
						LoadEmployees();
						Clear();
					}
					catch (Exception ex)
					{
						MessageBox.Show("Error deleting employee: " + ex.Message);
					}
				}
			}
			else
			{
				MessageBox.Show("Please select an employee to delete.");
			}
		}
		private void Clear()
		{
			FNTb.Text = string.Empty;
			LNTb.Text = string.Empty;
			GenderCb.SelectedIndex = 0;
			AddTb.Text = string.Empty;
			EmailTb.Text = string.Empty;
			PhoneTb.Text = string.Empty;
			DptCb.SelectedIndex = 0;
			PosCb.SelectedIndex = 0;
			StsCb.SelectedIndex = 0;
			SDDtp.Value = DateTime.Now;
		}
		private void EmployeeDGV_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0)
			{
				DataGridViewRow row = EmployeeDGV.Rows[e.RowIndex];

				FNTb.Text = row.Cells["FirstName"].Value.ToString();
				LNTb.Text = row.Cells["LastName"].Value.ToString();
				GenderCb.SelectedItem = row.Cells["Gender"].Value.ToString();
				AddTb.Text = row.Cells["Address"].Value.ToString();
				EmailTb.Text = row.Cells["Email"].Value.ToString();
				PhoneTb.Text = row.Cells["Phone"].Value.ToString();

				string department = row.Cells["Department"].Value.ToString();
				DptCb.SelectedItem = department;

				string position = row.Cells["Position"].Value.ToString();
				if (PosCb.Items.Contains(position))
				{
					PosCb.SelectedItem = position;
				}
				StsCb.SelectedItem = row.Cells["Status"].Value.ToString();

				if (row.Cells["StartDate"].Value != DBNull.Value)
				{
					SDDtp.Value = Convert.ToDateTime(row.Cells["StartDate"].Value);
				}
				// Load profile picture
				if (row.Cells["ProfilePicture"].Value != DBNull.Value)
				{
					byte[] imgData = (byte[])row.Cells["ProfilePicture"].Value;
					using (MemoryStream ms = new MemoryStream(imgData))
					{
						ProfilePb.Image = Image.FromStream(ms);
					}
				}
				else
				{
					ProfilePb.Image = null;
				}
			}
		}
		private void LoadBtn_Click(object sender, EventArgs e)
		{
			LoadEmployees();
		}
		private void ProfileBtn_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog openFile = new OpenFileDialog())
			{
				openFile.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
				if (openFile.ShowDialog() == DialogResult.OK)
				{
					ProfilePb.Image = Image.FromFile(openFile.FileName);
					profileImageBytes = File.ReadAllBytes(openFile.FileName);
				}
			}
		}
		private void ReturnPb_Click(object sender, EventArgs e)
		{
			this.home.Show(); // Show Home
			this.Close();     // Close Employee form (not just hide)
		}
		private void ApplyColorPalette()
		{
			// Main form and panels
			this.BackColor = palette.Background;
			guna2Panel1.BackColor = palette.Primary;
			EmpInfoGb.BackColor = palette.Background;

			// Labels
			foreach (var label in new[] { label1, label2, label3, label4, label5, label6, label7, label8, label9, label11, UsernameLbl })
				label.ForeColor = palette.Accent;
			EmpInfoGb.ForeColor = palette.Accent;

			// MaterialSkin TextBoxes
			foreach (var tb in new[] { FNTb, LNTb, AddTb, PhoneTb, EmailTb, UsernameTb })
				tb.BackColor = palette.Secondary;

			// ComboBoxes and DateTimePicker
			foreach (var cb in new[] { StsCb, PosCb, GenderCb, DptCb })
				cb.FillColor = palette.Secondary;
			SDDtp.FillColor = palette.Secondary;

			// Buttons
			foreach (var btn in new[] { ProfileBtn, SaveBtn, EditBtn, DeleteBtn })
			{
				btn.FillColor = palette.Primary;
				btn.ForeColor = Color.White;
				btn.HoverState.FillColor = palette.Highlight;
				btn.HoverState.ForeColor = palette.Accent;
			}
			LoadBtn.FillColor = palette.Accent;
			LoadBtn.ForeColor = palette.Primary;
			LoadBtn.HoverState.FillColor = palette.Highlight;
			LoadBtn.HoverState.ForeColor = Color.White;

			// DataGridView
			EmployeeDGV.BackgroundColor = palette.Background;
			EmployeeDGV.GridColor = palette.Secondary;
		}
	}
}