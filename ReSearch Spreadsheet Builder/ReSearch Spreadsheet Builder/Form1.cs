using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using System.IO;
using System.Reflection;

using System.Data.SqlClient;

namespace ReSearch_Spreadsheet_Builder
{
	public partial class Form1 : Form
	{
		private BackgroundWorker worker = new BackgroundWorker();
		private string _siteID = string.Empty, _saveLocation = string.Empty;
		private bool? _noData = false;

		public Form1()
		{
			InitializeComponent();

			worker.DoWork += worker_DoWork;
			worker.ProgressChanged += worker_ProgressChanged;
			worker.RunWorkerCompleted += worker_RunWorkerCompleted;
			worker.WorkerReportsProgress = true;
			worker.WorkerSupportsCancellation = true;

			this.txtServerName.Text = Environment.MachineName;
		}

		private void btnTest_Click(object sender, EventArgs e)
		{
			this.btnTest.Text = "Testing...";
			this.btnTest.Enabled = false;

			using (SqlConnection connection = new SqlConnection())
			{
				if (rdoWindowsAuth.Checked)
					connection.ConnectionString = ("Data Source='" + txtServerName.Text + "';Initial Catalog=Operations;Trusted_Connection=Yes;");
				else
					connection.ConnectionString = ("Data Source='" + txtServerName.Text + "';Initial Catalog=Operations;User ID='" + txtUser.Text + "';Password='" + txtPassword.Text + "'");

				try
				{
					connection.Open();
					if (connection.State.Equals(ConnectionState.Open)) // if connection.Open was successful
					{
						try
						{
							using (SqlCommand command = new SqlCommand("SELECT GUID FROM Operations.dbo.Site ORDER BY GUID"))
							{
								command.Connection = connection;
								command.CommandType = CommandType.Text;
								command.CommandTimeout = 30;

								comboSiteIDs.Items.Clear();

								using (SqlDataReader reader = command.ExecuteReader())
								{
									while (reader.Read())
										comboSiteIDs.Items.Add(reader[0].ToString());
								}

								comboSiteIDs.SelectedIndex = 0;
							}

//							MessageBox.Show("You have been successfully connected to the Odyssey DBs!", "Successful");
						}
						catch
						{
							MessageBox.Show("You have been successfully connected to the Operations DB, but there was a problem retrieving Site data.", "Partially successful");
						}
					}
					else
					{
						MessageBox.Show("Connection failed.", "No connection");
					}
				}
				catch (SqlException)
				{
					MessageBox.Show("Connection failed.", "No connection");
				}
			}

			this.btnTest.Text = "Test/Update SiteID List";
			this.btnTest.Enabled = true;
		}

		private void rdoSQLAuth_CheckedChanged(object sender, EventArgs e)
		{
			if (rdoSQLAuth.Checked)
			{
				txtUser.Enabled = true;
				txtPassword.Enabled = true;
			}
			else
			{
				txtUser.Enabled = false;
				txtPassword.Enabled = false;
			}

			this.comboSiteIDs.Items.Clear();
		}

		private void btnRun_Click(object sender, EventArgs e)
		{
			//if (btnRun.Text.Equals("Cancel"))
			//{
			//	if (worker.IsBusy)
			//		worker.CancelAsync();
			//}
			//else
			//{
			//	_siteID = comboSiteIDs.Items[comboSiteIDs.SelectedIndex].ToString();
			//	btnRun.Text = "Cancel";
			//	worker.RunWorkerAsync();
			//}

			if (comboSiteIDs.SelectedIndex < 0)
			{
				MessageBox.Show("SiteID is required.  Please select one before running.", "Site required");
				return;
			}

			_siteID = comboSiteIDs.Items[comboSiteIDs.SelectedIndex].ToString();
			_saveLocation = txtSaveLocation.Text;

			if (!Directory.Exists(Path.GetDirectoryName(_saveLocation)))
			{
				MessageBox.Show("Save location is required.  Please select a valid path/file before running.", "Location required");
				return;
			}

			this.btnRun.Text = "Working.  Please wait...";
			this.btnRun.Enabled = false;

			worker.RunWorkerAsync();
		}

		private void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			int cur = 0;

			_noData = false;
			
			using (SqlConnection connectionMain = new SqlConnection())
			{
				if (rdoWindowsAuth.Checked)
					connectionMain.ConnectionString = ("Data Source='" + txtServerName.Text + "';Initial Catalog=Justice;Trusted_Connection=Yes;");
				else
					connectionMain.ConnectionString = ("Data Source='" + txtServerName.Text + "';Initial Catalog=Justice;User ID='" + txtUser.Text + "';Password='" + txtPassword.Text + "'");

				try
				{
					connectionMain.Open();

					if (connectionMain.State.Equals(ConnectionState.Open)) // if connection.Open was successful
					{
//						worker.ReportProgress(cur, "starting");

						SqlCommand commandMain = new SqlCommand();
						commandMain.Connection = connectionMain;
						commandMain.CommandType = CommandType.Text;
						commandMain.CommandText = Resources.Get_ReSearch_Codes;

						commandMain.Parameters.Add(new SqlParameter("@SiteID", this._siteID));

						commandMain.CommandTimeout = 0; // may run a while

						DataSet dsCodes = new DataSet();
						SqlDataAdapter daCodes = new SqlDataAdapter();

						daCodes.SelectCommand = commandMain;
						daCodes.Fill(dsCodes);

						connectionMain.Close();

						//CreateSpreadsheet(ref dsCodes);

						try
						{
							FileInfo newFile = new FileInfo(_saveLocation);

							if (newFile.Exists)
							{
								if (MessageBox.Show("This file currently exists.  If you continue, it will be overwritten.", "File exists", MessageBoxButtons.OKCancel) != DialogResult.OK)
								{
									_noData = null;
									return;
								}

								File.Delete(_saveLocation);
							}

							File.WriteAllBytes(_saveLocation, Properties.Resources.ReSearch_Template);

							using (ExcelPackage excel = new ExcelPackage(new FileInfo(_saveLocation)))
							{
								ExcelWorkbook workbook = excel.Workbook;

								foreach (ExcelWorksheet oneSheet in workbook.Worksheets)
								{
									var range = oneSheet.Cells["B1"].LoadFromDataTable(dsCodes.Tables[cur++], false);

									if (range == null)
									{
										_noData = true;
										break;
									}

									var tbl = oneSheet.Tables.Add(range, "Table-" + cur.ToString());     //"data-" + cur.ToString()
									tbl.ShowFilter = false;

									if (oneSheet.Name.Equals("Locations"))
									{
										tbl.Columns[0].Name = " ";
										tbl.Columns[1].Name = "Parent Node ID";
										tbl.Columns[2].Name = "Node ID";
										tbl.Columns[3].Name = "Node Name";
									}
									else
									{
										//if (oneSheet.Name.Equals("Extended Connections"))
										//	tbl.Columns[0].Name = "Please put an X by the codes you want EXCLUDED";
										//else
										//tbl.Columns[0].Name = "Please put an X by the codes you want INCLUDED";

										tbl.Columns[0].Name = "Please put an X by the codes you want included";

										tbl.Columns[1].Name = "Node ID";
										tbl.Columns[2].Name = "Code Word";
										tbl.Columns[3].Name = "Description";

										tbl.ShowFilter = true;
									}

									oneSheet.Column(2).AutoFit();
									oneSheet.Column(3).AutoFit();
									oneSheet.Column(4).AutoFit();
									oneSheet.Column(5).AutoFit();
								}

								excel.Save();
							}
						}
						catch (Exception exc)
						{
							Console.WriteLine(exc.Message);
						}
					}
					else
					{
						MessageBox.Show("Connection failed.", "No Connection");
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Error");
				}
			}
		}

		private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			//if (e.ProgressPercentage.Equals(0))
			//	labelCurrentlyRunning.Text = "Retrieving dataset.  This may take a while.  Please wait.";
			//else
			//	labelCurrentlyRunning.Text = e.UserState.ToString();
		}

		private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.btnRun.Text = "Generate Re:Search Spreadsheet";
			this.btnRun.Enabled = true;

			if (e.Cancelled)
				MessageBox.Show("Process was cancelled");
			else if (e.Error != null)
				MessageBox.Show("There was an error running the process. The thread aborted with message: " + e.Error.Message.ToString(), "Fail");
			else if (_noData.HasValue)
			{
				if (_noData.Value)
					MessageBox.Show("No data found for that SiteID.", "Fail");
				else
					MessageBox.Show("Spreadsheet has been completed successfully.", "Success");
			}

			//labelCurrentlyRunning.Text = string.Empty;
//			btnRun.Text = "Run Certified Cost Updates";
		}


		//private void CreateSpreadsheet(ref DataSet dsCodes)
		//{
		////	StringBuilder vbaCode = new StringBuilder();
		//	int cur = 0;
			
		//	try
		//	{
		//		FileInfo newFile = new FileInfo(_saveLocation);

		//		if (newFile.Exists)
		//		{
		//			if (MessageBox.Show("This file currently exists.  If you continue, it will be overwritten.", "File exists", MessageBoxButtons.OKCancel) != DialogResult.OK)
		//			{
		//				_noData = null;
		//				return;
		//			}

		//			File.Delete(_saveLocation);
		//		}

		//		File.WriteAllBytes(_saveLocation, Properties.Resources.ReSearch_Template);

		//		//using (ExcelPackage excel = new ExcelPackage(new FileInfo(Path.GetDirectoryName(_saveLocation) + "\\Template3.xlsm")))
		//		using (ExcelPackage excel = new ExcelPackage(new FileInfo(_saveLocation)))
		//		{
		//			ExcelWorkbook workbook = excel.Workbook; 
		//			//workbook.Worksheets.Add("Locations");
		//			//workbook.Worksheets.Add("Case Types");
		//			//workbook.Worksheets.Add("Case Security Groups");
		//			//workbook.Worksheets.Add("Document Types");
		//			//workbook.Worksheets.Add("Document Security Groups");
		//			//workbook.Worksheets.Add("Extended Connections");

		//			//string styleLeftName = "StyleLeft", styleBoldName = "StyleBold";

		//			//var styleLeft = workbook.Styles.CreateNamedStyle(styleLeftName);
		//			//styleLeft.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

		//			//var styleBold = workbook.Styles.CreateNamedStyle(styleBoldName);
		//			//styleBold.Style.Font.Bold = true;

		//			//	excel.Workbook.Worksheets.Add("Locations");
		//			//excel.Workbook.Worksheets.Add("Case Types");
		//			//excel.Workbook.Worksheets.Add("Case Security Groups");
		//			//excel.Workbook.Worksheets.Add("Document Types");
		//			//excel.Workbook.Worksheets.Add("Document Security Groups");
		//			//excel.Workbook.Worksheets.Add("Extended Connections");

		//			//string styleLocationName = "StyleLocation", styleMainName = "StyleMain";
		//			//var styleLocation = excel.Workbook.Styles.CreateNamedStyle(styleLocationName);
		//			//styleLocation.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;


		//			foreach (ExcelWorksheet oneSheet in workbook.Worksheets)
		//			{
		//				var range = oneSheet.Cells["B1"].LoadFromDataTable(dsCodes.Tables[cur++], false);

		//				if (range == null)
		//				{
		//					_noData = true;
		//					break;
		//				}

		//				var tbl = oneSheet.Tables.Add(range, "Table-" + cur.ToString());     //"data-" + cur.ToString()
		//				tbl.ShowFilter = false;

		//				//var tbl = oneSheet.Tables[0];

		//				if (oneSheet.Name.Equals("Locations"))
		//				{
		//					tbl.Columns[0].Name = " ";
		//					tbl.Columns[1].Name = "Parent Node ID";
		//					tbl.Columns[2].Name = "Node ID";
		//					tbl.Columns[3].Name = "Node Name";

		//					//							tbl.ShowFilter = false;
		//					//tbl.Columns[1].DataCellStyleName = styleLeftName;
		//					//tbl.Columns[2].DataCellStyleName = styleLeftName;
		//					//tbl.HeaderRowCellStyle = styleBoldName;

		//					//oneSheet.Cells["A1"].Value = "Locations";
		//					//oneSheet.Cells["B1"].Value = "Parent Node ID";
		//					//oneSheet.Cells["C1"].Value = "Node ID";
		//					//oneSheet.Cells["D1"].Value = "Node Name";
		//					//oneSheet.Cells["A1:D1"].Style.Font.Bold = true;
		//				}
		//				else 
		//				{
		//					if (oneSheet.Name.Equals("Extended Connections"))
		//						tbl.Columns[0].Name = "Please put an X by the codes you want EXCLUDED";
		//					else
		//						tbl.Columns[0].Name = "Please put an X by the codes you want INCLUDED";

		//					tbl.Columns[1].Name = "Node ID";
		//					tbl.Columns[2].Name = "Code Word";
		//					tbl.Columns[3].Name = "Description";

		//					tbl.ShowFilter = true;
		//					//tbl.Columns[1].DataCellStyleName = styleLeftName;
		//					//tbl.HeaderRowCellStyle = styleBoldName;

		//					//oneSheet.Cells["A1"].Value = "Please put an X by the code types you want included";
		//					//oneSheet.Cells["B1"].Value = "Node ID";
		//					//oneSheet.Cells["C1"].Value = "Code Word";
		//					//oneSheet.Cells["D1"].Value = "Description";
		//					//oneSheet.Cells["A1:D1"].Style.Font.Bold = true;
		//				}

		//				//oneSheet.Column(1).AutoFit();
		//				oneSheet.Column(2).AutoFit();
		//				oneSheet.Column(3).AutoFit();
		//				oneSheet.Column(4).AutoFit();
		//				oneSheet.Column(5).AutoFit();
		//			}

		//			//vbaCode.AppendLine("Sub Copy3()");
		//			//vbaCode.AppendLine("	Dim Output As String");
		//			//vbaCode.AppendLine();
		//			//vbaCode.AppendLine("	For i = 2 To Cells(Rows.Count, 'C').End(xlUp).Row");
		//			//vbaCode.AppendLine("		If Not Rows(i).Hidden And Len(Trim((Cells(i, 2).Value))) Then Output = Output & Cells(i, 4).Value & ';'");
		//			//vbaCode.AppendLine("	Next");
		//			//vbaCode.AppendLine();
		//			//vbaCode.AppendLine("	If Len(Output) > 1 Then Output = Left(Output, Len(Output) - 1)");
		//			//vbaCode.AppendLine();
		//			//vbaCode.AppendLine("	ActiveSheet.Range('Z1').Value = Output");
		//			//vbaCode.AppendLine("	ActiveSheet.Range('Z1').Copy");
		//			//vbaCode.AppendLine("End Sub");


		//			//					workbook.CreateVBAProject();
		//			//workbook.CodeModule.Name = "Module2";
		//			//workbook.CodeModule.Code = vbaCode.ToString();





		//			//FileInfo excelFile = new FileInfo(@"D:\ExcelTest.xlsm");
		//			//FileInfo newFile = new FileInfo(_saveLocation);

		//			//if (newFile.Exists)
		//			//	File.Delete(_saveLocation);

		//			//excel.SaveAs(newFile);
		//			excel.Save();
		//		}
		//	}
		//	catch (Exception e)
		//	{
		//		Console.WriteLine(e.Message);
		//	}

		//	// currentSheet = excel.Workbook.Worksheets["Case Types"];
		//}

		private void txtServerName_TextChanged(object sender, EventArgs e)
		{
			this.comboSiteIDs.Items.Clear();
		}

		private void btnLocation_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlgFile = new OpenFileDialog();
			dlgFile.CheckFileExists = false;
			dlgFile.CheckPathExists = true;
			dlgFile.DefaultExt = ".xlsm";
			dlgFile.Filter = "Excel files (*.xlsm)|*.xlsm";

			if (dlgFile.ShowDialog() == DialogResult.OK)
			{
				txtSaveLocation.Text = dlgFile.FileName;
			}

		}
	}
}
