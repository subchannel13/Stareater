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
	}
}
