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
		
		public static string ConstructionEstimation(ConstructableInfo construction, IText neverText, IText perTurnText, IText perTurnPlusText, IText etaText, IText burstText)
		{
			if (construction.CompletedCount > 1)
			{
				if (construction.Investment < construction.Cost)
					return burstText.Text(null, new TextVar("count", new ThousandsFormatter().Format(construction.CompletedCount)).Get);

				var rate = (long)Math.Floor(construction.Investment / construction.Cost);
				if (rate < construction.CompletedCount)
					return perTurnPlusText.Text(
						null,
						new TextVar("count", new ThousandsFormatter().Format(construction.CompletedCount)).
						And("extra", new ThousandsFormatter().Format(construction.CompletedCount - rate)).Get
					);

				return perTurnText.Text(null, new TextVar("count", new ThousandsFormatter().Format(construction.CompletedCount)).Get);
			}

			if (construction.Investment <= 0 || (construction.Investment / construction.Cost) < MinimumPerTurnDone)
				return neverText.Text();

			var eta = (construction.Cost - construction.FromStockpile) / construction.Investment;

			return etaText.Text(
				new Var("eta", eta).Get, 
				new TextVar("eta", (eta < 10) ?
					new DecimalsFormatter(0, 1).Format(eta, RoundingMethod.Ceil, 1) :
					new ThousandsFormatter().Format(Math.Ceiling(eta))
				).Get
			);
		}
		
		public static string PlanetName(PlanetInfo planet)
		{
			string starName = planet.HostStar.Name.ToText(LocalizationManifest.Get.CurrentLanguage);
			
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
