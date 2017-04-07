using System;
using System.Windows.Forms;
using Stareater.Controllers.Views;

namespace Stareater.GUI
{
	public partial class PlayerView : UserControl
	{
		public PlayerView()
		{
			InitializeComponent();
		}
		
		public void SetData(PlayerInfo playerInfo) {
			flagImage.BackColor = playerInfo.Color;
			nameLabel.Text = playerInfo.Name;
			organizationLabel.Text = ""; //TODO(later) add organization info
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
