using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Stareater.Controllers.Views;
using Stareater.AppData;
using Stareater.Localization;
using Stareater.GUI.Reports;

namespace Stareater.GUI
{
	public sealed partial class FormReports : Form
	{
		private readonly IEnumerable<IReportInfo> reports;
		
		public IReportInfo Result { get; private set; }
		
		public FormReports()
		{
			this.InitializeComponent();
		}
		
		public FormReports(IEnumerable<IReportInfo> reports) : this()
		{
			this.reports = reports.ToList();
			
			this.Text = LocalizationManifest.Get.CurrentLanguage["FormReports"]["FormTitle"].Text();
			this.Font = SettingsWinforms.Get.FormFont;
			this.fillList();
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void fillList()
		{
			this.reportList.Controls.Clear();

			var filter = new FilterRepotVisitor();
			foreach (var report in this.reports)
			{
				report.Accept(filter);
				if (!filter.ShowItem)
					continue;

				this.reportList.Controls.Add(new ReportItem { Data = report });
			}

			if (!this.reports.Any())
			{
				this.reportList.Controls.Add(new Label {
					AutoSize = true,
					Text = LocalizationManifest.Get.CurrentLanguage["FormReports"]["noReports"].Text()
				});
				this.openButton.Enabled = false;
				this.filterButton.Enabled = false;
			}
		}
		
		private void openButton_Click(object sender, EventArgs e)
		{
			if (!(this.reportList.SelectedItem is ReportItem reportItem))
				return;

			this.Result = reportItem.Data;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		
		private void filterButton_Click(object sender, EventArgs e)
		{
			using(var form = new FormReportFilter())
				form.ShowDialog();
			
			this.fillList();
		}
	}
}
