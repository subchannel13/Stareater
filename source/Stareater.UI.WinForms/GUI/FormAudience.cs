using System;
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
		}
		
		private void formAudience_FormClosed(object sender, FormClosedEventArgs e)
		{
			controller.Done();
		}
	}
}
