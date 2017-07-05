using System;
using System.Linq;
using System.Windows.Forms;
using Stareater.Controllers;
using Stareater.Localization;
using Stareater.Properties;

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
			
			updateTreaties();
		}

		private void updateTreaties()
		{
			var context = LocalizationManifest.Get.CurrentLanguage["FormAudience"];
			
			this.warAction.Text = context[this.controller.IsAtWar ? "declarePeace" : "declareWar"].Text();
			
			this.treatyList.SuspendLayout();
			this.treatyList.Controls.Clear();
			foreach(var treaty in this.controller.Treaties)
			{
				var control = new TreatyBriefView();
				control.SetData(Resources.cancel, treaty.Name);
				this.treatyList.Controls.Add(control);
			}
			this.treatyList.ResumeLayout();
			//TODO(v0.6) update treaty list
		}

		private void formAudience_FormClosed(object sender, FormClosedEventArgs e)
		{
			controller.Done();
		}

		private void endAudienceAction_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void warAction_Click(object sender, EventArgs e)
		{
			if (this.controller.IsAtWar)
				this.controller.DeclearePeace();
			else
				this.controller.DecleareWar();

			updateTreaties();
		}
	}
}
