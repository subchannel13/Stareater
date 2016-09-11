using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using Stareater.AppData;
using Stareater.GuiUtils;
using Stareater.Localization;
using Stareater.Utils;

namespace Stareater.GUI
{
	public partial class FormSettings : Form
	{
		private bool initialized = false;
		private Language selectedLanguage = SettingsWinforms.Get.Language;
		private float selectedGuiScale = SettingsWinforms.Get.GuiScale;
		private string rendererInfoText = "";

		public FormSettings()
		{
			InitializeComponent();
		}

		public FormSettings(string rendererInfoText) : this()
		{
			this.rendererInfoText = rendererInfoText;

			foreach (var langInfo in LocalizationManifest.Get.LanguageNames.OrderBy(x => x.Value))
			{
				languageSelector.Items.Add(new Tag<string>(langInfo.Key, langInfo.Value));
				if (langInfo.Key == SettingsWinforms.Get.Language.Code)
					languageSelector.SelectedIndex = languageSelector.Items.Count - 1;
			}

			var scales = new List<int>();
			scales.AddRange(Methods.Range(5, 100, 5));
			scales.AddRange(Methods.Range(100, 200, 10));
			scales.AddRange(Methods.Range(200, 400, 20, true));
			scales.Reverse();

			for (int i = 0; i < scales.Count; i++)
			{
				guiScaleSelector.Items.Add(new Tag<int>(scales[i], scales[i] + " %"));
				if (Math.Abs(scales[i] / 100f - SettingsWinforms.Get.GuiScale) < 1e-4)
					guiScaleSelector.SelectedIndex = guiScaleSelector.Items.Count - 1;
			}

			fpsInput.Value = SettingsWinforms.Get.Framerate;
			unlimitedFpsCheck.Checked = SettingsWinforms.Get.UnlimitedFramerate;
			
			switch(SettingsWinforms.Get.FramerateBusySpinUsage)
			{
				case BusySpinMode.Always:
					busyFrameLimitAlways.Checked = true;
					break;
				case BusySpinMode.Never:
					busyFrameLimitNever.Checked = true;
					break;
				case BusySpinMode.NotOnBattery:
					busyFrameLimitPlugged.Checked = true;
					break;
			}
			
			setLanguage();
		}

		private void setLanguage()
		{
			Context context = selectedLanguage["FormSettings"];

			this.Text = context["FormTitle"].Text();
			this.Font = new Font(SystemFonts.DefaultFont.FontFamily, SystemFonts.DefaultFont.Size * selectedGuiScale);

			languageTitle.Text = context["LanguageLabel"].Text();
			guiScaleTitle.Text = context["GuiScaleLabel"].Text();
			fpsTitle.Text = context["FpsLabel"].Text();
			unlimitedFpsCheck.Text = context["UnlimitedFpsLabel"].Text();
			fpsTimingTitle.Text = context["FpsTimingLabel"].Text();
			busyFrameLimitAlways.Text = context["FpsBusyAlways"].Text();
			busyFrameLimitNever.Text = context["FpsBusyNever"].Text();
			busyFrameLimitPlugged.Text = context["FpsBusyPlugged"].Text();
			confirmButton.Text = selectedLanguage["General"]["DialogAccept"].Text();

			rendererInfo.Text = context["RendererLabel"].Text() + Environment.NewLine + this.rendererInfoText;
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void FormSettings_Load(object sender, EventArgs e)
		{
			initialized = true;
		}

		private void languageSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (languageSelector.SelectedItem == null || !initialized)
				return;

			selectedLanguage = LocalizationManifest.Get.LoadLanguage((languageSelector.SelectedItem as Tag<string>).Value);
			setLanguage();
		}

		private void guiScaleSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (guiScaleSelector.SelectedItem == null || !initialized)
				return;

			selectedGuiScale = (guiScaleSelector.SelectedItem as Tag<int>).Value / 100f;
			setLanguage();
		}
		
		private void UnlimitedFpsCheckCheckedChanged(object sender, EventArgs e)
		{
			fpsInput.Enabled = !unlimitedFpsCheck.Checked;
		}

		private void confirmButton_Click(object sender, EventArgs e)
		{
			SettingsWinforms.Get.Language = selectedLanguage;
			SettingsWinforms.Get.GuiScale = selectedGuiScale;

			SettingsWinforms.Get.Framerate = (int)fpsInput.Value;
			SettingsWinforms.Get.UnlimitedFramerate = unlimitedFpsCheck.Checked;
			
			if (busyFrameLimitAlways.Checked) 
				SettingsWinforms.Get.FramerateBusySpinUsage = BusySpinMode.Always;
			if (busyFrameLimitNever.Checked) 
				SettingsWinforms.Get.FramerateBusySpinUsage = BusySpinMode.Never;
			if (busyFrameLimitPlugged.Checked) 
				SettingsWinforms.Get.FramerateBusySpinUsage = BusySpinMode.NotOnBattery;
			
			DialogResult = System.Windows.Forms.DialogResult.OK;
		}
	}
}
