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
		
		public void SetData(PlayerInfo playerInfo)
		{
			this.flagImage.BackColor = playerInfo.Color;
			this.nameLabel.Text = playerInfo.Name;
			this.organizationLabel.Text = playerInfo.Organization.Name;
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
