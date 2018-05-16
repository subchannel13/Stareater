using System;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Localization;
using Stareater.GuiUtils;

namespace Stareater.GUI
{
	public partial class EmpyPlanetView : UserControl
	{
		private ColonizationController controller;
		private PlayerController gameController;
		
		public EmpyPlanetView()
		{
			InitializeComponent();
		}
		
		public void SetView(ColonizationController planetController, PlayerController gameController)
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new Action<ColonizationController, PlayerController>(SetView), planetController, gameController);
				return;
			}
			
			this.controller = planetController;
			this.gameController = gameController;
			
			setName();
			resetView();
		}
		
		private void setName()
		{
			this.nameLabel.Text = LocalizationMethods.PlanetName(controller.PlanetBody);
		}
		
		private void resetView()
		{
			this.Font = SettingsWinforms.Get.FormFont;

			var context = LocalizationManifest.Get.CurrentLanguage["FormMain"];
			this.detailsButton.Text = context["SiteDetails"].Text();
			this.estimationLabel.Text = "";
			
			this.colonizeButton.Text = this.controller.IsColonizing ? context["ColonizeStop"].Text() : context["ColonizeStart"].Text();
		}
		
		private void ColonizeButtonClick(object sender, EventArgs e)
		{
			if (this.controller.IsColonizing)
				this.controller.StopColonization();
			else
			{
				//TODO(v0.8) smarter default colonization source selection
				this.controller.StartColonization(this.gameController.Stellarises().ToArray());
			}
				
			resetView();
		}
	}
}
