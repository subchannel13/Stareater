using Stareater.Utils.StateEngine;
using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.Ships;
using Stareater.Utils;

namespace Stareater.GameData.Databases.Tables
{
	class PlayerOrders 
	{
		//TODO(v0.8): move or remove
		public const double DefaultSiteSpendingRatio = 1;

		[StateProperty]
		public int DevelopmentFocusIndex { get; set; }

		[StateProperty]
		public Dictionary<string, int> DevelopmentQueue { get; set; }

		[StateProperty]
		public string ResearchFocus { get; set; }

		[StateProperty]
		public Dictionary<AConstructionSite, ConstructionOrders> ConstructionPlans { get; private set; }

		[StateProperty]
		public Dictionary<StellarisAdmin, SystemPolicy> Policies { get; private set; }

		[StateProperty]
		public Dictionary<Vector2D, HashSet<Fleet>> ShipOrders { get; private set; }

		[StateProperty]
		public Dictionary<Planet, ColonizationPlan> ColonizationOrders { get; private set; }

		[StateProperty]
		public Dictionary<Design, Design> RefitOrders { get; private set; }

		[StateProperty]
		public HashSet<int> AudienceRequests { get; private set; }

		[StateProperty]
		public StarData EjectingStar { get; set; }

		public PlayerOrders()
		{
			this.DevelopmentFocusIndex = 0;
			this.DevelopmentQueue = new Dictionary<string, int>();
			this.ResearchFocus = "";
			this.ConstructionPlans = new Dictionary<AConstructionSite, ConstructionOrders>();
			this.Policies = new Dictionary<StellarisAdmin, SystemPolicy>();
			this.ShipOrders = new Dictionary<Vector2D, HashSet<Fleet>>();
			this.ColonizationOrders = new Dictionary<Planet, ColonizationPlan>();
			this.RefitOrders = new Dictionary<Design, Design>();
			this.AudienceRequests = new HashSet<int>();
			this.EjectingStar = null;
 		}
	}
}
