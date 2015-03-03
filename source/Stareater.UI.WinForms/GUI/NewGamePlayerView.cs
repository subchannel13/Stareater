using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stareater.Controllers.Views;
using Stareater.AppData;

namespace Stareater.GUI
{
	public partial class NewGamePlayerView : UserControl
	{
		public NewGamePlayerView()
		{
			InitializeComponent();
		}

		public void SetData(NewGamePlayerInfo playerInfo) {
			flagImage.BackColor = playerInfo.Color;
			nameLabel.Text = playerInfo.Name;
			organizationLabel.Text = (playerInfo.Organization != null) ?
				playerInfo.Organization.Name :
				SettingsWinforms.Get.Language["General"]["RandomOrganization"].Text();
		}

		private void flagImage_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}

		private void nameLabel_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}

		private void organizationLabel_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
	}
}
