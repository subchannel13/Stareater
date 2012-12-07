using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stareater.Controllers.Data;

namespace Stareater.GUI
{
	public partial class NewGamePlayerView : UserControl
	{
		public NewGamePlayerView()
		{
			InitializeComponent();
		}

		public void SetData(NewGamePlayerInfo playerInfo) {
			nameLabel.Text = playerInfo.Name;
			organizationLabel.Text = playerInfo.Organization.Name;
		}
	}
}
