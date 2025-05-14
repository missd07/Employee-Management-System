using System;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;
using EmpMS;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System.Collections.Generic;
using System.Collections;
using OxyPlot.Legends;
using OxyPlot.Annotations;
using System.Globalization;

namespace EmpMS
{
	public partial class Home : BaseForm
	{
		public Home(Login loginForm)
		{
			InitializeComponent();
			this.Opacity = 0;
			_loginForm = loginForm;
			Employeebtn.Click += Employeebtn_Click;
			Attendancebtn.Click += Attendancebtn_Click;
			Payrollbtn.Click += Payrollbtn_Click;
			EmpCntLbl.Text = FetchEmployeeCount().ToString();
			MngCntLbl.Text = FetchManagerCount().ToString();
			LogoutBtn.Click += LogoutBtn_Click;
			RefreshBtn.Click += RefreshBtn_Click;
			EmailBtn.Click += EmailBtn_Click;
			ApplyColorPalette();
			toolTip1 = new ToolTip(this.components);
			InitializeTooltips();
		}
		private OleDbConnection GetConnection() => base.GetConnection();
		private const string PositionCategoriesKey = "PositionCategories";
		private Login _loginForm;
		private readonly IColorPalette palette = new SpiritedAwayBathhousePalette();
		private void Home_Load(object sender, EventArgs e)
		{
			EmpCntLbl.Text = FetchEmployeeCount().ToString();
			MngCntLbl.Text = FetchManagerCount().ToString();
			InitializePlots();
			this.Opacity = 1;
		}
		private void Employeebtn_Click(object sender, EventArgs e)
		{
			try
			{
				this.Hide();
				Employee empForm = new Employee(this);
				empForm.Show();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error opening Employee form: {ex.Message}");
			}
		}
		private void Attendancebtn_Click(object sender, EventArgs e)
		{
			Attendance att = new Attendance(this);
			att.Show();
			this.Hide();
		}
		private void Payrollbtn_Click(object sender, EventArgs e)
		{
			try
			{
				this.Hide();
				PayRoll payForm = new PayRoll(this);
				payForm.Show();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error opening Payroll form: {ex.Message}");
			}
		}
		private int FetchEmployeeCount()
		{
			int count = 0;
			try
			{
				using (OleDbConnection conn = GetConnection())
				{
					conn.Open();
					OleDbCommand cmd = new OleDbCommand("SELECT COUNT(*) FROM EmployeeTbl", conn);
					count = Convert.ToInt32(cmd.ExecuteScalar());
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error fetching employee count: {ex.Message}");
			}
			return count;
		}
		private int FetchManagerCount()
		{
			int count = 0;
			try
			{
				using (OleDbConnection conn = GetConnection())
				{
					conn.Open();
					OleDbCommand cmd = new OleDbCommand("SELECT COUNT(*) FROM EmployeeTbl WHERE Position = 'Manager'", conn);
					count = Convert.ToInt32(cmd.ExecuteScalar());
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error fetching manager count: {ex.Message}");
			}
			return count;
		}
		private void InitializePlots()
		{
			UpdateAttendancePlot("Weekly");
			WeekRbtn.Checked = true;
			GnederPv.Model = GenderPlotModel();
			PositionPv.Model = PositionPlotModel();
		}
		private static class ChartColors
		{
			public static OxyColor Primary = OxyColor.FromRgb(70, 130, 180);   // Steel Blue
			public static OxyColor Secondary = OxyColor.FromRgb(34, 139, 34);   // Forest Green
			public static OxyColor Tertiary = OxyColor.FromRgb(128, 0, 128);    // Purple
			public static OxyColor Neutral = OxyColor.FromRgb(128, 128, 128);  // Gray
			public static OxyColor Background = OxyColor.FromRgb(245, 245, 220); // Beige
		}
		public class AttendanceStats
		{
			public int Present { get; set; }
			public int HalfDay { get; set; }
			public int Absent { get; set; }
			public int Total { get; set; }
		}
		private Dictionary<DateTime, AttendanceStats> FetchAttendanceBreakdown(string timeframe)
		{
			var attendanceData = new Dictionary<DateTime, AttendanceStats>();
			try
			{
				using (OleDbConnection conn = GetConnection())
				{
					conn.Open();
					string query = timeframe switch
					{
						"Weekly" => @"
                    SELECT 
                        Year([Date]) AS YearPart,
                        DatePart('ww', [Date]) AS WeekPart,
                        SUM(IIF(UCASE(TRIM([Status])) = 'PRESENT', 1, 0)) AS Present,
                        SUM(IIF(UCASE(TRIM([Status])) = 'HALF DAY', 1, 0)) AS HalfDay,
                        SUM(IIF(UCASE(TRIM([Status])) = 'ABSENT', 1, 0)) AS Absent
                    FROM AttendanceTbl
                    WHERE Year([Date]) = Year(Date())
                    GROUP BY Year([Date]), DatePart('ww', [Date])
                    ORDER BY Year([Date]), DatePart('ww', [Date])",
						"Monthly" => @"
                    SELECT 
                        [Date],
                        SUM(IIF(UCASE(TRIM([Status])) = 'PRESENT', 1, 0)) AS Present,
                        SUM(IIF(UCASE(TRIM([Status])) = 'HALF DAY', 1, 0)) AS HalfDay,
                        SUM(IIF(UCASE(TRIM([Status])) = 'ABSENT', 1, 0)) AS Absent
                    FROM AttendanceTbl
                    WHERE Year([Date]) = Year(Date()) 
                        AND Month([Date]) = Month(Date())
                    GROUP BY [Date]
                    ORDER BY [Date]",
						"Yearly" => @"
                    SELECT 
                        Year([Date]) AS YearPart,
                        Month([Date]) AS MonthPart,
                        SUM(IIF(UCASE(TRIM([Status])) = 'PRESENT', 1, 0)) AS Present,
                        SUM(IIF(UCASE(TRIM([Status])) = 'HALF DAY', 1, 0)) AS HalfDay,
                        SUM(IIF(UCASE(TRIM([Status])) = 'ABSENT', 1, 0)) AS Absent
                    FROM AttendanceTbl
                    WHERE Year([Date]) = Year(Date())
                    GROUP BY Year([Date]), Month([Date])
                    ORDER BY Year([Date]), Month([Date])",
						_ => @"
                    SELECT 
                        Year([Date]) AS YearPart,
                        SUM(IIF(UCASE(TRIM([Status])) = 'PRESENT', 1, 0)) AS Present,
                        SUM(IIF(UCASE(TRIM([Status])) = 'HALF DAY', 1, 0)) AS HalfDay,
                        SUM(IIF(UCASE(TRIM([Status])) = 'ABSENT', 1, 0)) AS Absent
                    FROM AttendanceTbl
                    WHERE Year([Date]) = Year(Date())
                    GROUP BY Year([Date])
                    ORDER BY Year([Date])"
					};

					using (OleDbCommand cmd = new OleDbCommand(query, conn))
					using (OleDbDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							DateTime dateKey = timeframe switch
							{
								"Weekly" => GetFirstDayOfWeek(
									Convert.ToInt32(reader["YearPart"]),
									Convert.ToInt32(reader["WeekPart"])),
								"Monthly" => reader.GetDateTime(0),
								"Yearly" => new DateTime(
									Convert.ToInt32(reader["YearPart"]),
									Convert.ToInt32(reader["MonthPart"]), 1),
								_ => new DateTime(
									Convert.ToInt32(reader["YearPart"]), 1, 1)
							};

							attendanceData[dateKey] = new AttendanceStats
							{
								Present = Convert.ToInt32(reader["Present"]),
								HalfDay = Convert.ToInt32(reader["HalfDay"]),
								Absent = Convert.ToInt32(reader["Absent"])
							};
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error fetching attendance data: {ex.Message}");
			}

			return attendanceData;
		}
		private DateTime GetFirstDayOfWeek(int year, int week)
		{
			DateTime jan1 = new DateTime(year, 1, 1);
			int daysOffset = (int)((int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - jan1.DayOfWeek);
			DateTime firstMonday = jan1.AddDays(daysOffset);

			return firstMonday.AddDays((week - 1) * 7);
		}
		private void UpdateAttendancePlot(string timeframe)
		{
			AttendancePv.Model = AttendancePlotModel(timeframe);
			AttendancePv.InvalidatePlot(true);
		}
		private void WeekRbtn_CheckedChanged(object sender, EventArgs e)
		{
			if (WeekRbtn.Checked)
				UpdateAttendancePlot("Weekly");
		}
		private void MonthRbth_CheckedChanged(object sender, EventArgs e)
		{
			if (MonthRbth.Checked)
				UpdateAttendancePlot("Monthly");
		}

		private void YearRbtn_CheckedChanged(object sender, EventArgs e)
		{
			if (YearRbtn.Checked)
				UpdateAttendancePlot("Yearly");
		}
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			WeekRbtn.CheckedChanged += WeekRbtn_CheckedChanged;  // ▲▲▲
			MonthRbth.CheckedChanged += MonthRbth_CheckedChanged;
			YearRbtn.CheckedChanged += YearRbtn_CheckedChanged;
		}
		private PlotModel AttendancePlotModel(string timeframe)
		{
			var plotModel = new PlotModel
			{
				Title = $"Attendance Breakdown ({timeframe})",
				Background = ChartColors.Background
			};

			var dateAxis = new DateTimeAxis
			{
				Position = AxisPosition.Bottom,
				Title = timeframe,
				StringFormat = timeframe switch
				{
					"Weekly" => "dd MMM yyyy",
					"Monthly" => "dd MMM",
					"Yearly" => "MMM yyyy",
					_ => "yyyy"
				},
				IntervalType = timeframe switch
				{
					"Weekly" => DateTimeIntervalType.Weeks,
					"Monthly" => DateTimeIntervalType.Days,
					"Yearly" => DateTimeIntervalType.Months,
					_ => DateTimeIntervalType.Years
				},
				TextColor = ChartColors.Secondary,
				TitleColor = ChartColors.Secondary
			};

			var valueAxis = new LinearAxis
			{
				Position = AxisPosition.Left,
				Title = "Number of Days",
				Minimum = 0,
				TextColor = ChartColors.Secondary,
				TitleColor = ChartColors.Secondary
			};

			var attendanceData = FetchAttendanceBreakdown(timeframe);

			var presentSeries = new LineSeries { Title = "Present", Color = ChartColors.Primary };
			var halfDaySeries = new LineSeries { Title = "Half-Day", Color = ChartColors.Secondary };
			var absentSeries = new LineSeries { Title = "Absent", Color = ChartColors.Tertiary };

			foreach (var period in attendanceData.OrderBy(kvp => kvp.Key))
			{
				double x = DateTimeAxis.ToDouble(period.Key);
				presentSeries.Points.Add(new DataPoint(x, period.Value.Present));
				halfDaySeries.Points.Add(new DataPoint(x, period.Value.HalfDay));
				absentSeries.Points.Add(new DataPoint(x, period.Value.Absent));
			}

			plotModel.Series.Add(presentSeries);
			plotModel.Series.Add(halfDaySeries);
			plotModel.Series.Add(absentSeries);

			plotModel.Axes.Add(dateAxis);
			plotModel.Axes.Add(valueAxis);

			return plotModel;
		}
		private PlotModel GenderPlotModel()
		{
			var plotModel = new PlotModel
			{
				Title = "Gender Distribution",
				Background = ChartColors.Background
			};
			var pieSeries = new PieSeries
			{
				Stroke = OxyColors.White,
				StrokeThickness = 2,
				InsideLabelPosition = 0.5,
				InsideLabelFormat = "{1}",
				OutsideLabelFormat = "{0}",
				StartAngle = 0,
				AngleSpan = 360
			};
			var genderData = FetchGenderCounts();
			foreach (var gender in genderData)
			{
				pieSeries.Slices.Add(new PieSlice(gender.Key, gender.Value)
				{
					Fill = GenderColor(gender.Key)
				});
			}
			plotModel.Series.Add(pieSeries);
			return plotModel;
		}
		private OxyColor GenderColor(string gender)
		{
			string normalizedGender = gender?.Trim().ToLower() ?? string.Empty;
			return normalizedGender switch
			{
				"male" => ChartColors.Primary,
				"female" => ChartColors.Secondary,
				_ => ChartColors.Neutral
			};
		}
		private PlotModel PositionPlotModel()
		{
			var plotModel = new PlotModel
			{
				Title = "Position Distribution",
				Background = ChartColors.Background
			};

			// Add category axis
			plotModel.Axes.Add(new CategoryAxis
			{
				Position = AxisPosition.Left,
				Title = "Position",
				Key = PositionCategoriesKey,
				TextColor = ChartColors.Tertiary,
				TitleColor = ChartColors.Tertiary
			});

			// Add value axis
			plotModel.Axes.Add(new LinearAxis
			{
				Position = AxisPosition.Bottom,
				Title = "Count",
				MinimumPadding = 0,
				AbsoluteMinimum = 0,
				Key = "PositionCount",
				TextColor = ChartColors.Tertiary,
				TitleColor = ChartColors.Tertiary
			});

			// Get category axis safely
			var categoryAxis = plotModel.Axes.OfType<CategoryAxis>()
				.FirstOrDefault(a => a.Key == PositionCategoriesKey);

			if (categoryAxis == null)
			{
				MessageBox.Show("Position axis configuration error");
				return plotModel;
			}

			var series = new BarSeries
			{
				XAxisKey = "PositionCount",
				YAxisKey = PositionCategoriesKey,
				FillColor = ChartColors.Tertiary,
				StrokeColor = OxyColors.White,
				StrokeThickness = 1
			};

			var positionData = FetchPositionCounts();
			foreach (var position in positionData)
			{
				series.Items.Add(new BarItem { Value = position.Value });
				categoryAxis.Labels.Add(position.Key);
			}

			plotModel.Series.Add(series);
			return plotModel;
		}
		private Dictionary<string, int> FetchGenderCounts()
		{
			var counts = new Dictionary<string, int>();
			try
			{
				using (OleDbConnection conn = GetConnection())
				{
					conn.Open();
					OleDbCommand cmd = new OleDbCommand("SELECT Gender, COUNT(*) FROM EmployeeTbl GROUP BY Gender", conn);

					using (OleDbDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							counts[reader.GetString(0)] = reader.GetInt32(1);
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error fetching gender counts: {ex.Message}");
			}
			return counts;
		}
		private Dictionary<string, int> FetchPositionCounts()
		{
			var counts = new Dictionary<string, int>();
			try
			{
				using (OleDbConnection conn = GetConnection())
				{
					conn.Open();
					var cmd = new OleDbCommand(
						"SELECT Position, COUNT(*) FROM EmployeeTbl WHERE Position <> 'Manager' GROUP BY Position",
						conn);

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							counts[reader.GetString(0)] = reader.GetInt32(1);
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error fetching position counts: {ex.Message}");
			}
			return counts;
		}
		private void LogoutBtn_Click(object sender, EventArgs e)
		{
			try
			{
				_loginForm.Show(); // Show the stored Login form
				this.Close();      // Close the Home form
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Logout error: {ex.Message}");
			}
		}
		private void RefreshBtn_Click(object sender, EventArgs e)
		{
			try
			{
				EmpCntLbl.Text = FetchEmployeeCount().ToString();
				MngCntLbl.Text = FetchManagerCount().ToString();
				InitializePlots();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Refresh error: {ex.Message}");
			}
		}
		private void EmailBtn_Click(object sender, EventArgs e)
		{
			try
			{
				Email emailForm = new Email();
				emailForm.Show();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error opening Email form: {ex.Message}");
			}
		}
		private void InitializeTooltips()
		{
			toolTip1.SetToolTip(Employeebtn, "Manage employee records");
			toolTip1.SetToolTip(Attendancebtn, "Track employee attendance");
			toolTip1.SetToolTip(Payrollbtn, "Process employee payroll");
			toolTip1.SetToolTip(LogoutBtn, "Log out of the system");
			toolTip1.SetToolTip(RefreshBtn, "Refresh dashboard statistics");
			toolTip1.SetToolTip(EmailBtn, "Send Email");
		}
		private void ApplyColorPalette()
		{
			// Panels
			Toppnl.BackColor = palette.Primary;           // IndianRed
			Leftpnl.BackColor = palette.Background;       // BurlyWood
			Navpnl.BackColor = palette.Secondary;         // Salmon

			// Buttons
			Payrollbtn.FillColor = palette.Highlight;     // Firebrick
			Payrollbtn.ForeColor = Color.White;
			Payrollbtn.HoverState.FillColor = palette.Accent;  // Gold
			Payrollbtn.HoverState.ForeColor = palette.Primary;

			Attendancebtn.FillColor = palette.Highlight;
			Attendancebtn.ForeColor = Color.White;
			Attendancebtn.HoverState.FillColor = palette.Accent;
			Attendancebtn.HoverState.ForeColor = palette.Primary;

			Employeebtn.FillColor = palette.Highlight;
			Employeebtn.ForeColor = Color.White;
			Employeebtn.HoverState.FillColor = palette.Accent;
			Employeebtn.HoverState.ForeColor = palette.Primary;

			LogoutBtn.FillColor = palette.Primary;
			LogoutBtn.ForeColor = Color.White;
			LogoutBtn.HoverState.FillColor = palette.Highlight;
			LogoutBtn.HoverState.ForeColor = palette.Accent;

			// Labels
			label1.ForeColor = palette.Accent;
			Employeeslbl.ForeColor = palette.Primary;
			EmpCntLbl.ForeColor = palette.Accent;
			Managerslbl.ForeColor = palette.Primary;
			MngCntLbl.ForeColor = palette.Accent;

			// PlotViews
			PositionPv.BackColor = palette.Background;
			GnederPv.BackColor = palette.Background;
			AttendancePv.BackColor = palette.Background;
		}
	}
}