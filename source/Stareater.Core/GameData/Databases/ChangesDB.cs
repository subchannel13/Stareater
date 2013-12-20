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
		public IDictionary<string, int> ResearchQueue { get; private set; }
		
		public IDictionary<AConstructionSite, ConstructionOrders> ConstructionPlans { get; private set; }
		
		public ChangesDB()
		{
			this.DevelopmentQueue = new Dictionary<string, int>();
			this.ResearchQueue = new Dictionary<string, int>();
			this.ConstructionPlans = new Dictionary<AConstructionSite, ConstructionOrders>();
		}
		
		public void Reset(ISet<string> validTechs,
			IEnumerable<AConstructionSite> validColonies, IEnumerable<AConstructionSite> validStellarises)
		{
			DevelopmentQueue = resetTechQueue(DevelopmentQueue, validTechs);
			ResearchQueue = resetTechQueue(ResearchQueue, validTechs);
			
			var validSites = validColonies.Concat(validStellarises);
			var oldSpendings = this.ConstructionPlans;
			this.ConstructionPlans = validSites.ToDictionary(x => x, x => new ConstructionOrders(DefaultSiteSpendingRatio));
			
			foreach (var site in validSites) {
				if (!oldSpendings.ContainsKey(site))
					ConstructionPlans[site] = oldSpendings[site];
			}
		}
		
		private static IDictionary<string, int> resetTechQueue(IDictionary<string, int> queue, ISet<string> validItems)
		{
			var newOrder = queue
				.Where(x => validItems.Contains(x.Key))
				.OrderBy(x => x.Value)
				.Select(x => x.Key).ToArray();
			
			var newQueue = new Dictionary<string, int>();
			for (int i = 0; i < newOrder.Length; i++) 
				newQueue.Add(newOrder[i], i);
			
			return newQueue;
		}

		internal ChangesDB Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap)
		{
			ChangesDB copy = new ChangesDB();

			copy.ConstructionPlans = this.ConstructionPlans.ToDictionary(
				x => playersRemap.Site(x.Key),
				x => x.Value.Copy());

			copy.DevelopmentQueue = new Dictionary<string, int>(this.DevelopmentQueue);
			copy.ResearchQueue = new Dictionary<string, int>(this.ResearchQueue);

			return copy;
		}
	}
}
