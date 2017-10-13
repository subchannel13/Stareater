using Stareater.Controllers;
using Stareater.Localization;
using System.Windows.Forms;

namespace Stareater.GUI
{
	public partial class FormStareater : Form
	{
		private StareaterController controller;

        public FormStareater()
		{
			InitializeComponent();
		}

		public FormStareater(StareaterController controller) : this()
		{
			this.controller = controller;

			var context = LocalizationManifest.Get.CurrentLanguage["FormStareater"];
			this.Text = context["FormTitle"].Text();

			if (controller.HasControl)
				this.whoControlsLabel.Text = context["controlledByYou"].Text();
			else
				this.whoControlsLabel.Text = context["notControlledByYou"].Text();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape)
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
