using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.GameData;
using Stareater.GuiUtils;
using Stareater.Localization;

namespace Stareater.GUI
{
	public partial class ConstructionSiteView : UserControl
	{
		private AConstructionSiteController controller;
		
		public ConstructionSiteView()
		{
			InitializeComponent();
		}
		
		public void SetView(AConstructionSiteController siteController)
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new Action<AConstructionSiteController>(this.SetView), siteController);
				return;
			}
			
			this.controller = siteController;

			this.industrySlider.Value = (int)(siteController.DesiredSpendingRatio * this.industrySlider.Maximum);

			this.setName();
			this.resetView();
		}

		private void setName()
		{
			if (this.controller.SiteType == SiteType.Colony)
			{
				var colonyController = this.controller as ColonyController;
				this.nameLabel.Text = LocalizationMethods.PlanetName(colonyController.PlanetBody);
			}
			else
				this.nameLabel.Text = this.controller.HostStar.Name.ToText(LocalizationManifest.Get.CurrentLanguage);
		}
		
		private void resetView()
		{
			this.Font = SettingsWinforms.Get.FormFont;

			var context = LocalizationManifest.Get.CurrentLanguage["FormMain"];
			this.detailsButton.Text = context["SiteDetails"].Text();
			
			if (!controller.ConstructionQueue.Any()) {
				this.queueButton.Text = context["NotBuilding"].Text();
				this.queueButton.Image = null;

				this.industrySlider.Enabled = false;
			} else {
				this.queueButton.Text = "";
				this.queueButton.Image = ImageCache.Get[this.controller.ConstructionQueue.First().ImagePath];

				this.industrySlider.Enabled = !this.controller.IsReadOnly;
			}

			//TODO(later) replace with sprites
			var policyColors = new Dictionary<string, Color>()
			{
				{ "develop", Color.Yellow },
				{ "exploit", Color.Green }
			};
			this.policyName.Text = this.controller.Policy.Name;
			this.policyButton.BackColor = policyColors[this.controller.Policy.Id];

			this.resetEstimation();
		}
		
		private void resetEstimation()
		{
			var constructionItem = this.controller.ConstructionQueue.FirstOrDefault();
			var context = LocalizationManifest.Get.CurrentLanguage["FormMain"];
			
			if (constructionItem != null)
				this.estimationLabel.Text = LocalizationMethods.ConstructionEstimation(
					constructionItem, 
					context["EtaNever"], 
					context["BuildingsPerTurn"], 
					context["Eta"]
				);
			else
				this.estimationLabel.Text = "No construction plans";
		}
		
		private void queueButton_Click(object sender, EventArgs e)
		{
			if (this.controller == null)
				return;
			
			using (var form = new FormBuildingQueue(this.controller))
				form.ShowDialog();

			this.resetView();
		}

		private void industrySlider_Scroll(object sender, ScrollEventArgs e)
		{
			if (e.Type == ScrollEventType.EndScroll)
				return;

			this.controller.DesiredSpendingRatio = e.NewValue / (double)this.industrySlider.Maximum;
			this.resetEstimation();
		}

		private void detailsButton_Click(object sender, EventArgs e)
		{
			Form form = null;

			switch (this.controller.SiteType) {
				case SiteType.Colony:
					form = new FormColonyDetails(this.controller as ColonyController);
					break;
				case SiteType.StarSystem:
					form = new FormStellarisDetails(this.controller as StellarisAdminController);
					break;
			}

			form.ShowDialog();
			form.Dispose();
		}

		private void policyButton_Click(object sender, EventArgs e)
		{
			var policyIndex = Array.FindIndex(this.controller.Policies, this.controller.Policy.Equals);
			policyIndex = (policyIndex + 1) % this.controller.Policies.Length;

			this.controller.Policy = this.controller.Policies[policyIndex];

			this.industrySlider.Value = (int)(this.controller.DesiredSpendingRatio * this.industrySlider.Maximum);
			this.resetView();
		}
	}
}
