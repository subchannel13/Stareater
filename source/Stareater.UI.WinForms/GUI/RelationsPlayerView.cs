using System;
using System.Windows.Forms;
using Stareater.Controllers.Views;
using Stareater.Localization;
using Stareater.Properties;

namespace Stareater.GUI
{
	public partial class RelationsPlayerView : UserControl
	{
		public ContactInfo Data { get; private set; }

		public RelationsPlayerView()
		{
			InitializeComponent();
		}

		public void SetData(ContactInfo contact)
		{
			this.Data = contact;

			this.playerName.Text = contact.Player.Name;
			this.playerColor.BackColor = contact.Player.Color;
			
			this.audienceRequest.SetData(Resources.message, LocalizationManifest.Get.CurrentLanguage["FormRelations"]["audienceRequested"].Text());
			this.treatyList.SuspendLayout();
			foreach(var treaty in contact.Treaties)
			{
				var control = new TreatyBriefView();
				control.SetData(Resources.cancel, treaty.Name);
				this.treatyList.Controls.Add(control);
			}
			this.treatyList.ResumeLayout();
		}
		
		public void RequestedAudience(bool isRequested)
		{
			this.audienceRequest.Visible = isRequested;
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
