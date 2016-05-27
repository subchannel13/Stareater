using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Stareater.Controllers.Views;
using Stareater.AppData;

namespace Stareater.GUI
{
	public partial class FormReports : Form
	{
		public IReportInfo Result { get; private set; }
		
		public FormReports()
		{
			InitializeComponent();
		}
		
		public FormReports(IEnumerable<IReportInfo> reports) : this()
		{
			this.Text = SettingsWinforms.Get.Language["FormReports"]["FormTitle"].Text();

			foreach (var report in reports) {
				var reportItem = new ReportItem();
				reportItem.Data = report;
				
				reportList.Controls.Add(reportItem);
			}
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void openButton_Click(object sender, EventArgs e)
		{
			
			var reportItem = reportList.SelectedItem as ReportItem;
			
			if (reportItem == null)
				return;
			
			this.Result = reportItem.Data;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		
		private void filterButton_Click(object sender, EventArgs e)
		{
			//TODO(v0.5)
			throw new NotImplementedException();
		}
	}
}
