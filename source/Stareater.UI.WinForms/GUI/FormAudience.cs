using System;
using System.Linq;
using System.Windows.Forms;
using Stareater.Controllers;
using Stareater.Localization;

namespace Stareater.GUI
{
	public sealed partial class FormAudience : Form
	{
		private readonly AudienceController controller;
		
		public FormAudience()
		{
			InitializeComponent();
		}
		
		public FormAudience(AudienceController controller) : this()
		{
			this.controller = controller;
			
			var context = LocalizationManifest.Get.CurrentLanguage["FormAudience"];
			this.Text = context["FormTitle"].Text();
			this.endAudienceAction.Text = context["endButton"].Text();
			
			player1View.SetData(this.controller.Participant1);
			player2View.SetData(this.controller.Participant2);
		}
		
		private void formAudience_FormClosed(object sender, FormClosedEventArgs e)
		{
			controller.Done();
		}

		private void endAudienceAction_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
