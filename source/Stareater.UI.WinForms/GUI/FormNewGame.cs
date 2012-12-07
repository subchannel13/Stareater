using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Localization;
using Stareater.Controllers;

namespace Stareater.GUI
{
	partial class FormNewGame : Form
	{
		private NewGameController controller;

		public FormNewGame()
		{
			InitializeComponent();
			
			setLanguage(SettingsWinforms.Get.Language);
		}

		public void Initialize()
		{
			if (NewGameController.CanCreateGame) {
				controller = new NewGameController();
			}
			else
				DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}

		private void setLanguage(Language newLanguage)
		{
			Context context = newLanguage["FormNewGame"];

			this.Font = SettingsWinforms.Get.FormFont;
			this.Text = context["FormTitle"];

			setupPlayersButton.Text = context["setupPlayersButton"];
			setupMapButton.Text = context["setupMapButton"];
			setupStartButton.Text = context["setupStartButton"];
			startButton.Text = context["startButton"];
		}

		private void setupPlayersButton_Click(object sender, EventArgs e)
		{
			using (var playersForm = new FormSetupPlayers())
				if (playersForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					;
		}
	}
}
