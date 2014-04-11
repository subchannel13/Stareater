using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Galaxy;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;
using Stareater.GameLogic;

namespace Stareater.GameData.Databases
{
	class ChangesDB
	{
		//TODO(later): move or remove
		public const double DefaultSiteSpendingRatio = 1;
		
		public int DevelopmentFocusIndex { get; set; }
		public IDictionary<string, int> DevelopmentQueue { get; set; }
		public IDictionary<string, int> ResearchQueue { get; set; }
		
		public IDictionary<AConstructionSite, ConstructionOrders> ConstructionPlans { get; set; }
		
		public ChangesDB()
		{
			this.DevelopmentFocusIndex = 0;
			this.DevelopmentQueue = new Dictionary<string, int>();
			this.ResearchQueue = new Dictionary<string, int>();
			this.ConstructionPlans = new Dictionary<AConstructionSite, ConstructionOrders>();
		}
		
		internal ChangesDB Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap)
		{
			ChangesDB copy = new ChangesDB();

			copy.ConstructionPlans = this.ConstructionPlans.ToDictionary(
				x => playersRemap.Site(x.Key),
				x => x.Value.Copy());

			copy.DevelopmentFocusIndex = this.DevelopmentFocusIndex;
			copy.DevelopmentQueue = new Dictionary<string, int>(this.DevelopmentQueue);
			copy.ResearchQueue = new Dictionary<string, int>(this.ResearchQueue);

			return copy;
		}
	}
}
