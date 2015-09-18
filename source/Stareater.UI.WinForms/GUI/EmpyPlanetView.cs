using System;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Galaxy;
using Stareater.Localization;
using Stareater.Utils.Collections;
using Stareater.Utils.NumberFormatters;

namespace Stareater.GUI
{
	public partial class EmpyPlanetView : UserControl
	{
		private EmptyPlanetController controller;
		
		public EmpyPlanetView()
		{
			InitializeComponent();
		}
		
		public void SetView(EmptyPlanetController planetController)
		{
			controller = planetController;
			
			setName();
			resetView();
		}
		
		private void setName()
		{
			var context = SettingsWinforms.Get.Language["FormMain"];
			var textVars = new TextVar(
				"bodyName",
				controller.HostStar.Name.ToText(SettingsWinforms.Get.Language) + " " + RomanFromatter.Fromat(controller.BodyPosition)
			).Get;
			
			switch(controller.BodyType)
			{
				case PlanetType.Asteriod:
					this.nameLabel.Text = context["AsteriodName"].Text(textVars);
					break;
				case PlanetType.GasGiant:
					this.nameLabel.Text = context["GasGiantName"].Text(textVars);
					break;
				case PlanetType.Rock:
					this.nameLabel.Text = context["RockName"].Text(textVars);
					break;
			}		
		}
		
		private void resetView()
		{
			this.Font = SettingsWinforms.Get.FormFont;

			var context = SettingsWinforms.Get.Language["FormMain"];
			this.detailsButton.Text = context["SiteDetails"].Text();
			
			this.colonizeButton.Text = this.controller.IsColonizing ? context["ColonizeStop"].Text() : context["ColonizeStart"].Text();
		}
		
		private void ColonizeButtonClick(object sender, EventArgs e)
		{
			if (this.controller.IsColonizing)
				this.controller.StopColonization();
			else
				this.controller.StartColonization();
			//TODO(v0.5) colonize from some stellaris
				
			resetView();
		}
	}
}
