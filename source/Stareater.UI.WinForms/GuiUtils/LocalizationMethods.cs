using System;
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
			var textVars = new TextVar();
			
			if (construction.CompletedCount >= 1) 
			{
				var overflow = construction.Overflow / construction.Cost;
				
				if (construction.CompletedCount < 10)
					textVars.And("count", new DecimalsFormatter(0, 1).Format(construction.CompletedCount + overflow, RoundingMethod.Floor, 1));
				else
					textVars.And("count", new ThousandsFormatter().Format(construction.CompletedCount));

				return perTurnText.Text(null, textVars.Get);
			}
			
			if (construction.Investment <= 0 || (construction.Investment / construction.Cost) < MinimumPerTurnDone)
				return neverText.Text();
			
			var eta = (construction.Cost - construction.FromStockpile) / construction.Investment;
			var numVars = new Var("eta", eta).Get;
			
			if (eta < 10)
				textVars.And("eta", new DecimalsFormatter(0, 1).Format(eta, RoundingMethod.Ceil, 1));
			else
				textVars.And("eta", new ThousandsFormatter().Format(Math.Ceiling(eta)));
			
			return etaText.Text(numVars, textVars.Get);
		}
		
		public static string PlanetName(Planet planet)
		{
			string starName = planet.Star.Name.ToText(LocalizationManifest.Get.CurrentLanguage);
			
			var context = LocalizationManifest.Get.CurrentLanguage["FormMain"];
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
