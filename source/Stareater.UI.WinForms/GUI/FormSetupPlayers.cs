using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stareater.Controllers;
using Stareater.AppData;
using Stareater.Localization;
using Stareater.Players;
using Stareater.Controllers.Data;

namespace Stareater.GUI
{
	public partial class FormSetupPlayers : Form
	{
		private NewGameController controller;
		private bool eventClutch = false;

		public FormSetupPlayers()
		{
			InitializeComponent();
			
			setLanguage();
		}

		public FormSetupPlayers(NewGameController controller) : this()
		{
			this.controller = controller;

			updatePlayerViews();

			foreach(var playerType in NewGameController.PlayerTypes)
				controllerPicker.Items.Add(new Tag<PlayerType>(playerType, playerType.Name));

			organizationPicker.Items.Add(new Tag<Organization>(null, SettingsWinforms.Get.Language["General"]["RandomOrganization"]));
			foreach (var org in Organization.List)
				organizationPicker.Items.Add(new Tag<Organization>(org, org.Name));

			foreach (var color in PlayerAssets.Colors) {
				PictureBox colorBox = new PictureBox();
				colorBox.BackColor = color;
				colorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
				colorBox.Size = new System.Drawing.Size(32, 32);
				colorsLayout.Controls.Add(colorBox);
			}

			playerViewsLayout.SelectedIndex = 0;
		}

		private void setLanguage()
		{
			Context context = SettingsWinforms.Get.Language["FormSetupPlayers"];

			this.Font = SettingsWinforms.Get.FormFont;
			this.Text = context["FormTitle"];

			controllerLabel.Text = context["controllerLabel"] + ":";
			nameLabel.Text = context["nameLabel"] + ":";
			organizationLabel.Text = context["organizationLabel"] + ":";
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

		private NewGamePlayerView selectedPlayerView
		{
			get
			{
				return playerViewsLayout.SelectedItem as NewGamePlayerView;
			}
		}

		private void playerViewsLayout_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!playerViewsLayout.HasSelection) 
				return;

			PlayerInfo playerInfo = controller.PlayerList[playerViewsLayout.SelectedIndex];
			eventClutch = true;

			nameInput.Text = playerInfo.Name;
			controllerPicker.SelectedItem = new Tag<PlayerType>(playerInfo.ControlType, null);

			if (playerInfo.Organization != null)
				organizationPicker.SelectedItem = new Tag<Organization>(playerInfo.Organization, null);
			else
				organizationPicker.SelectedIndex = 0;

			eventClutch = false;
		}

		private void controllerPicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (eventClutch || controllerPicker.SelectedItem == null || !playerViewsLayout.HasSelection)
				return;

			controller.UpdatePlayer(playerViewsLayout.SelectedIndex, (controllerPicker.SelectedItem as Tag<PlayerType>).Value);
		}

		private void nameInput_TextChanged(object sender, EventArgs e)
		{
			if (eventClutch || !playerViewsLayout.HasSelection)
				return;

			controller.UpdatePlayer(playerViewsLayout.SelectedIndex, nameInput.Text);
			selectedPlayerView.SetData(controller.PlayerList[playerViewsLayout.SelectedIndex]);
		}

		private void organizationPicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (organizationPicker.SelectedItem == null || !playerViewsLayout.HasSelection)
				return;

			if (!eventClutch) {
				controller.UpdatePlayer(playerViewsLayout.SelectedIndex, (organizationPicker.SelectedItem as Tag<Organization>).Value);
				selectedPlayerView.SetData(controller.PlayerList[playerViewsLayout.SelectedIndex]);
			}
			
			var org = controller.PlayerList[playerViewsLayout.SelectedIndex].Organization;
			organizationDescription.Text = (org != null) ? org.Description : "";
		}
	}
}
