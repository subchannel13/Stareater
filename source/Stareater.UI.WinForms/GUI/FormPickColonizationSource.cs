using System;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Localization;
using Stareater.GuiUtils;

namespace Stareater.GUI
{
	public sealed partial class FormPickColonizationSource : Form
	{
		private readonly ColonizationController controller;
		
		public StellarisInfo SelectedSource { get; private set; }
		
		public FormPickColonizationSource()
		{
			InitializeComponent();
		}
		
		public FormPickColonizationSource(ColonizationController controller) : this()
		{
			this.controller = controller;
			
			foreach(var candidate in controller.AvailableSources())
				sourceList.Items.Add(new Tag<StellarisInfo>(
					candidate,
					candidate.HostStar.Name.ToText(LocalizationManifest.Get.CurrentLanguage)
				));
			sourceList.SelectedIndex = 0;
			
			this.Text = LocalizationManifest.Get.CurrentLanguage["FormColonization"]["addSourceTitle"].Text();
			this.Font = SettingsWinforms.Get.FormFont;
			this.addButton.Text = LocalizationManifest.Get.CurrentLanguage["FormColonization"]["addSource"].Text();
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape)
			{
				this.DialogResult = DialogResult.Cancel;
				this.Close();
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void sourceList_SelectedIndexChanged(object sender, EventArgs e)
		{
			//TODO(v0.8) change ETA info
			this.sourceInfo.Text = "";
		}
		
		private void addButton_Click(object sender, EventArgs e)
		{
			this.SelectedSource = (sourceList.SelectedItem as Tag<StellarisInfo>).Value;
			
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
