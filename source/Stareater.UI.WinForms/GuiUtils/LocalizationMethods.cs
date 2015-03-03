using System;
using Stareater.Controllers.Views;
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
	}
}
