using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Localization;
using Stareater.Utils;

namespace Stareater.GUI
{
	public partial class FormSettings : Form
	{
		private bool initialized = false;
		private Language selectedLanguage = SettingsWinforms.Get.Language;
		private float selectedGuiScale = SettingsWinforms.Get.GuiScale;

		public FormSettings()
		{
			InitializeComponent();

			foreach (var langInfo in LocalizationManifest.Get.LanguageNames.OrderBy(x => x.Value))
			{
				languageSelector.Items.Add(new Tag<string>(langInfo.Key, langInfo.Value));
				if (langInfo.Key == SettingsWinforms.Get.Language.Code)
					languageSelector.SelectedIndex = languageSelector.Items.Count - 1;
			}

			List<int> scales = new List<int>();
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

			setLanguage();
		}

		private void setLanguage()
		{
			Context context = selectedLanguage["FormSettings"];

			this.Text = context["FormTitle"].Text();
			this.Font = new Font(SystemFonts.DefaultFont.FontFamily, SystemFonts.DefaultFont.Size * selectedGuiScale);

			languageTitle.Text = context["LanguageLabel"].Text();
			guiScaleTitle.Text = context["GuiScaleLabel"].Text();
			confirmButton.Text = selectedLanguage["General"]["DialogOK"].Text();
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

		private void confirmButton_Click(object sender, EventArgs e)
		{
			SettingsWinforms.Get.Language = selectedLanguage;
			SettingsWinforms.Get.GuiScale = selectedGuiScale; 
			DialogResult = System.Windows.Forms.DialogResult.OK;
		}
	}
}
