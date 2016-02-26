using System;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.Utils.Collections;
using Stareater.Utils.NumberFormatters;
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
			controller = siteController;
			
			industrySlider.Value = (int)(siteController.DesiredSpendingRatio * industrySlider.Maximum);
			
			setName();
			resetView();
		}

		private void setName()
		{
			if (controller.SiteType == SiteType.Colony)
			{
				var colonyController = controller as ColonyController;
				this.nameLabel.Text = LocalizationMethods.PlanetName(colonyController.PlanetBody);
			}
			else
				this.nameLabel.Text = controller.HostStar.Name.ToText(SettingsWinforms.Get.Language);
		}
		
		private void resetView()
		{
			this.Font = SettingsWinforms.Get.FormFont;

			var context = SettingsWinforms.Get.Language["FormMain"];
			this.detailsButton.Text = context["SiteDetails"].Text();
			
			if (!controller.ConstructionQueue.Any()) {
				this.queueButton.Text = context["NotBuilding"].Text();
				this.queueButton.Image = null;
				
				industrySlider.Enabled = false;
			} else {
				this.queueButton.Text = "";
				this.queueButton.Image = ImageCache.Get[controller.ConstructionQueue.First().ImagePath]; //FIX crushes for ships
				
				industrySlider.Enabled = !controller.IsReadOnly;
			}
			
			resetEstimation();
		}
		
		private void resetEstimation()
		{
			var constructionItem = controller.ConstructionQueue.FirstOrDefault();
			var context = SettingsWinforms.Get.Language["FormMain"];
			
			if (constructionItem != null)
				estimationLabel.Text = LocalizationMethods.ConstructionEstimation(
					constructionItem, 
					context["EtaNever"], 
					context["BuildingsPerTurn"], 
					context["Eta"]
				);
			else
				estimationLabel.Text = "No construction plans";
		}
		
		private void queueButton_Click(object sender, EventArgs e)
		{
			if (controller == null)
				return;
			
			using (var form = new FormBuildingQueue(controller))
				form.ShowDialog();
			
			resetView();
		}

		private void industrySlider_Scroll(object sender, ScrollEventArgs e)
		{
			if (e.Type == ScrollEventType.EndScroll)
				return;
			
			controller.DesiredSpendingRatio = e.NewValue / (double)industrySlider.Maximum;
			resetEstimation();
		}

		private void detailsButton_Click(object sender, EventArgs e)
		{
			Form form = null;

			switch (controller.SiteType) {
				case SiteType.Colony:
					form = new FormColonyDetails(controller as ColonyController);
					break;
				case SiteType.StarSystem:
					form = new FormStellarisDetails(controller as StellarisAdminController);
					break;
			}

			form.ShowDialog();
			form.Dispose();
		}
	}
}
