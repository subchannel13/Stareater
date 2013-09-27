using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Galaxy;
using Stareater.GameData.Databases.Tables;

namespace Stareater.GameData.Databases
{
	class ChangesDB
	{
		//TODO: move or remove
		public const double DefaultSiteSpendingRatio = 1;
		
		public IDictionary<string, int> DevelopmentQueue { get; private set; }
		
		public IDictionary<AConstructionSite, ConstructionOrders> Constructions { get; private set; }
		
		public ChangesDB()
		{
			this.DevelopmentQueue = new Dictionary<string, int>();
			this.Constructions = new Dictionary<AConstructionSite, ConstructionOrders>();
		}
		
		public void Reset(IEnumerable<AConstructionSite> validSites)
		{
			DevelopmentQueue.Clear();
			
			var oldSpendings = Constructions;
			Constructions = validSites.ToDictionary(x => x, x => new ConstructionOrders(DefaultSiteSpendingRatio));
			
			foreach (var site in validSites) {
				if (!oldSpendings.ContainsKey(site))
					Constructions[site] = oldSpendings[site];
			}
		}
	}
}
