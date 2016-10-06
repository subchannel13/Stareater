using System;
using System.Drawing;
using System.Windows.Forms;

using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Localization;

namespace Stareater.GUI
{
	internal partial class FormMainMenu : Form
	{
		private Font titleFont;
		private GameController controller = null;
		internal MainMenuResult Result { get; private set; }

		public FormMainMenu()
		{
			InitializeComponent();
			this.titleFont = titleLabel.Font;

			setLanguage(LocalizationManifest.Get.CurrentLanguage);
		}
		
		public FormMainMenu(GameController controller) : this()
		{
			this.controller = controller;
		}

		private void setLanguage(Language newLanguage)
		{
			Context context = newLanguage["FormMainMenu"];

			this.Font = SettingsWinforms.Get.FormFont;
			titleLabel.Font = new Font(titleFont.FontFamily, titleFont.Size * SettingsWinforms.Get.GuiScale, titleFont.Style);
			this.Text = context["FormTitle"].Text();
			titleLabel.Text = context["Title"].Text();

			newGameButton.Text = context["NewGame"].Text();
			saveGameButton.Text = context["Save"].Text();
			settingsButton.Text = context["Settings"].Text();
			exitButton.Text = context["Quit"].Text();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
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
