using Stareater.AppData;
using Stareater.Localization;
using System;
using System.Windows.Forms;

namespace Stareater.GUI
{
	public sealed partial class FormReportFilter : Form
	{
		public FormReportFilter()
		{
			InitializeComponent();
			
			this.Font = SettingsWinforms.Get.FormFont;
			this.checkContacts.Checked = SettingsWinforms.Get.ReportContact;
			this.checkTechs.Checked = SettingsWinforms.Get.ReportTechnology;
			
			var context = LocalizationManifest.Get.CurrentLanguage["FormReports"];
			this.Text = context["FilterTitle"].Text();
			this.checkContacts.Text = context["showContacts"].Text();
			this.checkTechs.Text = context["showTechs"].Text();
			this.applyAction.Text = context["applyFilter"].Text();
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void applyAction_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		
		private void checkTechs_CheckedChanged(object sender, EventArgs e)
		{
			SettingsWinforms.Get.ReportTechnology = checkTechs.Checked;
		}

		private void checkContacts_CheckedChanged(object sender, EventArgs e)
		{
			SettingsWinforms.Get.ReportContact = checkContacts.Checked;
		}
	}
}
