using System;
using Stareater.AppData;
using Stareater.Controllers.Views;
using Stareater.Galaxy;
using Stareater.Localization;
using Stareater.Utils.Collections;
using Stareater.Utils.NumberFormatters;

namespace Stareater.GuiUtils
{
	public static class LocalizationMethods
	{
		private const double MinimumPerTurnDone = 1e-3;
		
		public static string ConstructionEstimation(ConstructableItem construction, IText neverText, IText perTurnText, IText etaText)
		{
			if (construction.PerTurnDone < MinimumPerTurnDone)
				return neverText.Text();
			
			var textVars = new TextVar();
			
			if (construction.PerTurnDone >= 1) {
				if (construction.PerTurnDone.Value < 10)
					textVars.And("count", new DecimalsFormatter(0, 1).Format(construction.PerTurnDone.Value, RoundingMethod.Floor, 1));
				else
					textVars.And("count", new ThousandsFormatter().Format(Math.Floor(construction.PerTurnDone.Value)));
				
				return perTurnText.Text(null, textVars.Get);
			}
			
			var numVars = new Var("eta", 1 / construction.PerTurnDone.Value).Get;
			
			if (construction.PerTurnDone.Value < 10)
				textVars.And("eta", new DecimalsFormatter(0, 1).Format(1 / construction.PerTurnDone.Value, RoundingMethod.Ceil, 1));
			else
				textVars.And("eta", new ThousandsFormatter().Format(Math.Ceiling(1 / construction.PerTurnDone.Value)));
			
			return etaText.Text(numVars, textVars.Get);
		}
		
		public static string PlanetName(Planet planet)
		{
			string starName = planet.Star.Name.ToText(SettingsWinforms.Get.Language);
			
			var context = SettingsWinforms.Get.Language["FormMain"];
			var textVars = new TextVar(
				"bodyName",
				starName + " " + RomanFromatter.Fromat(planet.Position)
			).Get;
			
			switch(planet.Type)
			{
				case PlanetType.Asteriod:
					return context["AsteriodName"].Text(textVars);
				case PlanetType.GasGiant:
					return context["GasGiantName"].Text(textVars);
				case PlanetType.Rock:
					return context["RockName"].Text(textVars);
				default:
					throw new NotImplementedException("Unimplemented planet type: " + planet.Type);
			} 
		}
	}
}
