using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.GameData
{
	partial class PlayerOrders
	{
		//TODO(later): move or remove
		public const double DefaultSiteSpendingRatio = 1;
		
		private void copyConstruction(PlayerOrders original, PlayersRemap playersRemap)
		{
			this.ConstructionPlans = original.ConstructionPlans.ToDictionary(
				x => playersRemap.Site(x.Key),
				x => x.Value.Copy());
		}

		private void copyDevelopment(PlayerOrders original)
		{
			this.DevelopmentQueue = new Dictionary<string, int>(original.DevelopmentQueue);
		}
	}
}
