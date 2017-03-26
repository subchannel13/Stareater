using System;
using System.Windows.Forms;
using Stareater.Controllers.Views;

namespace Stareater.GUI
{
	public partial class RelationsPlayerInfo : UserControl
	{
		public ContactInfo Data { get; private set; }

		public RelationsPlayerInfo()
		{
			InitializeComponent();
		}

		internal void SetData(ContactInfo contact)
		{
			this.Data = contact;

			this.playerName.Text = contact.Player.Name;
			this.playerColor.BackColor = contact.Player.Color;
		}

		private void playerName_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}

		private void playerColor_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}

		private void treatyList_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
	}
}
