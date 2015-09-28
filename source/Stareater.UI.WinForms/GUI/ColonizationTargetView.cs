using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Utils.Collections;
using Stareater.Utils.NumberFormatters;
using Stareater.GuiUtils;

namespace Stareater.GUI
{
	public partial class ColonizationTargetView : UserControl
	{
		private readonly ColonizationController controller;
		
		public ColonizationTargetView()
		{
			InitializeComponent();
		}
		
		public ColonizationTargetView(ColonizationController controller) : this()
		{
			this.controller = controller;
			var context = SettingsWinforms.Get.Language["FormColonization"];
			
			var infoFormatter = new ThousandsFormatter(controller.Population, controller.PopulationMax);
			var infoVars = new TextVar("pop", infoFormatter.Format(controller.Population)).
				And("max", infoFormatter.Format(controller.PopulationMax));
			
			this.targetName.Text = LocalizationMethods.PlanetName(controller.PlanetBody);
			this.targetInfo.Text = context["population"].Text(infoVars.Get);
			
			foreach(var source in controller.Sources())
	        {
	        	var itemView = new ColonizationSourceView();
	        	itemView.Data = source;
				sourceList.Controls.Add(itemView);
	        }
		}
	}
}
