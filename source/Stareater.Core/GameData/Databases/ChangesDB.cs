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
		public string ResearchFocus { get; set; }
		
		public IDictionary<AConstructionSite, ConstructionOrders> ConstructionPlans { get; set; }
		
		public ChangesDB()
		{
			this.DevelopmentFocusIndex = 0;
			this.ResearchFocus = null;
			this.DevelopmentQueue = new Dictionary<string, int>();
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

			return copy;
		}

		internal Ikadn.IkadnBaseObject Save()
		{
			//TODO(v0.5)
			throw new NotImplementedException();
		}
	}
}
