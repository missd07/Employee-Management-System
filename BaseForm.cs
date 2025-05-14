using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmpMS;
using Krypton.Toolkit;

namespace EmpMS
{
	public class BaseForm : Form
	{
		protected OleDbConnection GetConnection()
		{
			return new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\OneDrive\Documents\EmployeeManagementSystem.accdb;Mode=Share Deny None;Jet OLEDB:Database Locking Mode=0;");
		}
	}
}