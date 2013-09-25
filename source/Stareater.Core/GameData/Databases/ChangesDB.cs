using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Galaxy;

namespace Stareater.GameData.Databases
{
	class ChangesDB
	{
		//TODO: move or remove
		public const double DefaultSiteSpendingRatio = 1;
		
		public IDictionary<string, int> DevelopmentQueue { get; private set; }
		
		public IDictionary<AConstructionSite, double> SiteSpendingRatios { get; private set; }
		
		public ChangesDB()
		{
			this.DevelopmentQueue = new Dictionary<string, int>();
			this.SiteSpendingRatios = new Dictionary<AConstructionSite, double>();
		}
		
		public void Reset(IEnumerable<AConstructionSite> validSites)
		{
			DevelopmentQueue.Clear();
			
			var oldSpendings = SiteSpendingRatios;
			SiteSpendingRatios = validSites.ToDictionary(x => x, x => DefaultSiteSpendingRatio);
			
			foreach (var site in validSites) {
				if (!oldSpendings.ContainsKey(site))
					SiteSpendingRatios[site] = oldSpendings[site];
			}
		}
	}
}
