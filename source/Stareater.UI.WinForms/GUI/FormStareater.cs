using Stareater.Controllers;
using Stareater.Galaxy;
using Stareater.GuiUtils;
using Stareater.Localization;
using System.Linq;
using System.Windows.Forms;
using System;
using Stareater.Utils.Collections;

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

			var lang = LocalizationManifest.Get.CurrentLanguage;
            var context = lang["FormStareater"];

			this.Text = context["FormTitle"].Text();
			this.closeAction.Text = context["close"].Text();
			this.ejectLabel.Text = context["eject"].Text() + ":";

			if (controller.HasControl)
				this.whoControlsLabel.Text = context["controlledByYou"].Text();
			else
				this.whoControlsLabel.Text = context["notControlledByYou"].Text();

			this.starSelector.Items.Add(new Tag<StarData>(null, context["noStar"].Text()));
			var ejectables = controller.EjectableStars.
				Select(x => new Tag<StarData>(x, x.Name.ToText(lang))).
				OrderBy(x => x.DisplayText).
				ToList();
            foreach (var item in ejectables)
				this.starSelector.Items.Add(item);
			this.starSelector.SelectedIndex = 
				controller.EjectTarget != null ? 
				(ejectables.FindIndex(x => x.Value == controller.EjectTarget) + 1) :
				0;

			updateEta();
        }

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape)
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void updateEta()
		{
			var context = LocalizationManifest.Get.CurrentLanguage["FormStareater"];
			var progress = this.controller.EjectionProgress;

			this.selectionInfo.Text = 
				progress.CanProgress ? 
				context["ejectionEta"].Text(
					new Var("eta", Math.Ceiling(progress.Eta)).Get,
					new TextVar("eta", Math.Ceiling(progress.Eta).ToString()).Get) : 
				"";
		}

		private void starSelector_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.starSelector.SelectedItem == null)
				return;

			controller.EjectTarget = (this.starSelector.SelectedItem as Tag<StarData>).Value;
			updateEta();
		}

		private void closeAction_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
