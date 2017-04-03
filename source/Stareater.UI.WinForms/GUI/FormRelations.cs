using System;
using System.Windows.Forms;
using Stareater.Controllers;
using Stareater.Localization;

namespace Stareater.GUI
{
	public sealed partial class FormRelations : Form
	{
		private readonly PlayerController controller;
		
		public FormRelations()
		{
			InitializeComponent();
		}
		
		public FormRelations(PlayerController controller) : this()
		{
			this.controller = controller;
			
			var context = LocalizationManifest.Get.CurrentLanguage["FormRelations"];
			this.Text = context["FormTitle"].Text();
			this.audienceAction.Text = context["audience"].Text();
			this.audienceAction.Visible = false;

			playerList.SuspendLayout();
			foreach (var contact in this.controller.DiplomaticContacts())
			{
				var contactControl = new RelationsPlayerInfo();
				contactControl.SetData(contact);
				contactControl.RequestedAudience(this.controller.IsAudienceRequested(contact));
				playerList.Controls.Add(contactControl);
			}
			playerList.ResumeLayout();
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void updateAudienceButton()
		{
			this.audienceAction.Visible = playerList.HasSelection;
			
			if (!playerList.HasSelection)
				return;
			
			var context = LocalizationManifest.Get.CurrentLanguage["FormRelations"];
			var contact = (playerList.SelectedItem as RelationsPlayerInfo).Data;
			
			this.audienceAction.Text = this.controller.IsAudienceRequested(contact) ?
				context["cancelAudience"].Text() :
				context["audience"].Text();
		}
		
		private void playerList_SelectedIndexChanged(object sender, EventArgs e)
		{
			updateAudienceButton();
		}

		private void audienceAction_Click(object sender, EventArgs e)
		{
			if (!playerList.HasSelection)
				return;

			var contactView = playerList.SelectedItem as RelationsPlayerInfo;
			var contact = contactView.Data;
			if (this.controller.IsAudienceRequested(contact))
				this.controller.CancelAudience(contact);
			else
				this.controller.RequestAudience(contact);
			
			contactView.RequestedAudience(this.controller.IsAudienceRequested(contact));
			updateAudienceButton();
		}
	}
}
