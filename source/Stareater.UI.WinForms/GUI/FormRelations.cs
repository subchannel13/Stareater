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
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
