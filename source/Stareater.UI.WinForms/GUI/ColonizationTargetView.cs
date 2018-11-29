using System;
using System.Linq;
using System.Windows.Forms;
using Stareater.Controllers;
using Stareater.Localization;
using Stareater.Utils.Collections;
using Stareater.Utils.NumberFormatters;
using Stareater.GuiUtils;

namespace Stareater.GUI
{
	public partial class ColonizationTargetView : UserControl
	{
		private readonly ColonizationController controller;
		private readonly PlayerController gameController;
		
		public ColonizationTargetView()
		{
			InitializeComponent();
		}
		
		public ColonizationTargetView(ColonizationController controller, PlayerController gameController) : this()
		{
			this.controller = controller;
			this.gameController = gameController;
			var context = LocalizationManifest.Get.CurrentLanguage["FormColonization"];
			
			var infoFormatter = new ThousandsFormatter(controller.PopulationMax);
			var infoVars = new TextVar("pop", infoFormatter.Format(controller.Population)).
				And("max", infoFormatter.Format(controller.PopulationMax));
			
			this.targetName.Text = LocalizationMethods.PlanetName(controller.PlanetBody);
			this.targetInfo.Text = context["population"].Text(infoVars.Get);

			var enrouteShips = gameController.EnrouteColonizers(controller.PlanetBody).SelectMany(x => x.Ships).ToArray();
			var enroutePopulation = enrouteShips.Length > 0 ? 
				enrouteShips.Sum(x => x.Quantity * x.Design.ColonizerPopulation) : 
				0;

			this.enrouteInfo.Text =	context["enroute"].Text(
				new TextVar("count", new ThousandsFormatter().Format(enroutePopulation)).Get
			);
		}
	}
}
