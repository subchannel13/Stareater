using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Stareater.Controllers.Data;

namespace Stareater.GUI
{
	public partial class FormReports : Form
	{
		public FormReports()
		{
			InitializeComponent();
		}
		
		public FormReports(IEnumerable<IReportInfo> reports) : this()
		{
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
			//TODO(v0.5)
			throw new NotImplementedException();
		}
		
		private void filterButton_Click(object sender, EventArgs e)
		{
			//TODO(v0.5)
			throw new NotImplementedException();
		}
	}
}
