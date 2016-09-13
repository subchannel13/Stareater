using System;
using System.Windows.Forms;
using Stareater.AppData;

namespace Stareater.GUI
{
	public sealed partial class FormReportFilter : Form
	{
		public FormReportFilter()
		{
			InitializeComponent();
			
			this.Font = SettingsWinforms.Get.FormFont;
			this.checkTechs.Checked = SettingsWinforms.Get.ReportTechnology;
			
			var context = SettingsWinforms.Get.Language["FormReports"];
			this.Text = context["FilterTitle"].Text();
			this.checkTechs.Text = context["showTechs"].Text();
			this.applyAction.Text = context["applyFilter"].Text();
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void ApplyActionClick(object sender, EventArgs e)
		{
			this.Close();
		}
		
		private void checkTechs_CheckedChanged(object sender, EventArgs e)
		{
			SettingsWinforms.Get.ReportTechnology = checkTechs.Checked;
		}
	}
}
