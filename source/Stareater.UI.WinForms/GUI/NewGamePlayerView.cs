using System;
using System.Linq;
using System.Windows.Forms;
using Stareater.Controllers.Views;
using Stareater.Localization;

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
				LocalizationManifest.Get.CurrentLanguage["General"]["RandomOrganization"].Text();
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
