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
using Stareater.Controllers.Data;
using Stareater.Maps;
using Stareater.StringTools;

namespace Stareater.GUI
{
	partial class FormNewGame : Form
	{
		private NewGameController controller;

		public FormNewGame()
		{
			InitializeComponent();
			
			setLanguage();
		}

		public void Initialize()
		{
			if (NewGameController.CanCreateGame) {
				controller = new NewGameController();
				updatePlayerViews();

				foreach (var start in MapAssets.Starts)
					setupStartSelector.Items.Add(new Tag<StartingConditions>(start, start.Name));
				setupStartSelector.SelectedIndex = 0;
			}
			else
				DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}

		private void setLanguage()
		{
			Context context = SettingsWinforms.Get.Language["FormNewGame"];

			this.Font = SettingsWinforms.Get.FormFont;
			this.Text = context["FormTitle"];

			setupPlayersButton.Text = context["setupPlayersButton"];
			setupMapButton.Text = context["setupMapButton"];
			startButton.Text = context["startButton"];
		}

		private void updatePlayerViews()
		{
			var players = controller.PlayerList;
			
			playerViewsLayout.Controls.Clear();
			while (playerViewsLayout.Controls.Count < players.Count)
				playerViewsLayout.Controls.Add(new NewGamePlayerView());

			for (int i = 0; i < players.Count; i++)
				(playerViewsLayout.Controls[i] as NewGamePlayerView).SetData(players[i]);
		}

		private void setupPlayersButton_Click(object sender, EventArgs e)
		{
			using (var playersForm = new FormSetupPlayers(controller)) {
				playersForm.ShowDialog();
				updatePlayerViews();
			}
		}

		private void setupStartSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (setupStartSelector.SelectedItem == null)
				return;

			StartingConditions start = (setupStartSelector.SelectedItem as Tag<StartingConditions>).Value;
			Context context = SettingsWinforms.Get.Language["FormNewGame"];
			StringBuilder sb = new StringBuilder();
			var formatter = new ThousandsFormatter(start.Population, start.Infrastructure);

			sb.AppendLine("Colonies: " + start.Colonies);
			sb.AppendLine("Population: " + formatter.Format(start.Population));
			sb.Append("Infrastructure: " + formatter.Format(start.Infrastructure));

			startingDescription.Text = sb.ToString();
		}
	}
}
