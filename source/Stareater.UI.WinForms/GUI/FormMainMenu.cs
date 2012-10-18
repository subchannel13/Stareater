using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using Stareater.AppData;
using Stareater.Localization;

namespace Stareater.GUI
{
	internal partial class FormMainMenu : Form
	{
		private Font titleFont;
		internal MainMenuResult Result { get; private set; }

		public FormMainMenu()
		{
			InitializeComponent();
			this.titleFont = titleLabel.Font;

			setLanguage(SettingsWinforms.Get.Language);
		}

		private void setLanguage(Language newLanguage)
		{
			Context context = newLanguage["FormMainMenu"];

			this.Font = SettingsWinforms.Get.FormFont;
			titleLabel.Font = new Font(titleFont.FontFamily, titleFont.Size * SettingsWinforms.Get.GuiScale, titleFont.Style);
			this.Text = context["FormTitle"];
			titleLabel.Text = context["Title"];

			newGameButton.Text = context["NewGame"];
			loadGameButton.Text = context["Load"];
			saveGameButton.Text = context["Save"];
			settingsButton.Text = context["Settings"];
			exitButton.Text = context["Quit"];
		}

		private void newGameButton_Click(object sender, EventArgs e)
		{
			this.Result = MainMenuResult.NewGame;
			this.DialogResult = DialogResult.OK;
		}

		private void saveGameButton_Click(object sender, EventArgs e)
		{
			this.Result = MainMenuResult.SaveGame;
			this.DialogResult = DialogResult.OK;
		}

		private void loadGameButton_Click(object sender, EventArgs e)
		{
			this.Result = MainMenuResult.LoadGame;
			this.DialogResult = DialogResult.OK;
		}

		private void settingsButton_Click(object sender, EventArgs e)
		{
			this.Result = MainMenuResult.Settings;
			this.DialogResult = DialogResult.OK;
		}

		private void exitButton_Click(object sender, EventArgs e)
		{
			this.Result = MainMenuResult.Quit;
			this.DialogResult = DialogResult.OK;
		}
	}
}
