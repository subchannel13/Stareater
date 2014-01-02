using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
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

			resetView();
		}
		
		private void resetView()
		{
			this.Font = SettingsWinforms.Get.FormFont;

			Context context = SettingsWinforms.Get.Language["FormMain"];
			this.detailsButton.Text = context["SiteDetails"].Text();
			
			if (controller.ConstructionQueue.Count() == 0) {
				this.queueButton.Text = context["NotBuilding"].Text();
				this.queueButton.Image = null;
				
				industrySlider.Enabled = false;
			}
			else {
				this.queueButton.Text = "";
				this.queueButton.Image = ImageCache.Get[controller.ConstructionQueue.First().ImagePath];
				
				industrySlider.Enabled = !controller.IsReadOnly;
			}
			
			resetEstimation();
		}
		
		private void resetEstimation()
		{
			var constructionItem = controller.ConstructionQueue.FirstOrDefault();
			
			//TODO: set localized text
			if (constructionItem != null)
				estimationLabel.Text = constructionItem.PerTurnDone.Value.ToString("#.00");
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
			controller.DesiredSpendingRatio = e.NewValue / (double)industrySlider.Maximum;
			resetEstimation();
		}
	}
}
