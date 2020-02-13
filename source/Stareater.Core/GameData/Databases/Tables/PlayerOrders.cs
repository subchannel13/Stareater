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

		[StatePropertyAttribute]
		public int DevelopmentFocusIndex { get; set; }

		[StatePropertyAttribute]
		public Dictionary<string, int> DevelopmentQueue { get; set; }

		[StatePropertyAttribute]
		public string ResearchFocus { get; set; }

		[StatePropertyAttribute]
		public Dictionary<string, string[]> ResearchPriorities { get; set; }

		[StatePropertyAttribute]
		public Dictionary<AConstructionSite, ConstructionOrders> ConstructionPlans { get; private set; }

		[StatePropertyAttribute]
		public Dictionary<AConstructionSite, ConstructionOrders> AutomatedConstruction { get; private set; }

		[StatePropertyAttribute]
		public Dictionary<StellarisAdmin, SystemPolicy> Policies { get; private set; }

		[StatePropertyAttribute]
		public Dictionary<Vector2D, HashSet<Fleet>> ShipOrders { get; private set; }

		[StatePropertyAttribute]
		public HashSet<Planet> ColonizationTargets { get; private set; }

		[StatePropertyAttribute]
		public HashSet<StellarisAdmin> ColonizationSources { get; private set; }

		[StatePropertyAttribute]
		public long TargetTransportCapacity { get; set; }

		[StatePropertyAttribute]
		public Design ColonizerDesign { get; set; }

		[StatePropertyAttribute]
		public Dictionary<Design, Design> RefitOrders { get; private set; }

		[StatePropertyAttribute]
		public HashSet<Design> DiscardOrders { get; private set; }

		[StatePropertyAttribute]
		public HashSet<int> AudienceRequests { get; private set; }

		[StatePropertyAttribute]
		public StarData EjectingStar { get; set; }

		public PlayerOrders()
		{
			this.DevelopmentFocusIndex = 0;
			this.DevelopmentQueue = new Dictionary<string, int>();
			this.ResearchFocus = "";
			this.ResearchPriorities = new Dictionary<string, string[]>();
			this.ConstructionPlans = new Dictionary<AConstructionSite, ConstructionOrders>();
			this.Policies = new Dictionary<StellarisAdmin, SystemPolicy>();
			this.ShipOrders = new Dictionary<Vector2D, HashSet<Fleet>>();
			this.AutomatedConstruction = new Dictionary<AConstructionSite, ConstructionOrders>();
			this.ColonizationTargets = new HashSet<Planet>();
			this.ColonizationSources = new HashSet<StellarisAdmin>();
			this.TargetTransportCapacity = 0;
			this.RefitOrders = new Dictionary<Design, Design>();
			this.DiscardOrders = new HashSet<Design>();
			this.AudienceRequests = new HashSet<int>();
			this.EjectingStar = null;
 		}
	}
}
