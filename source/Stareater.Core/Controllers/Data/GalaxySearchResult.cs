using System;
using System.Collections.Generic;
using Stareater.Controllers.Views.Ships;
using Stareater.Galaxy;
using Stareater.Controllers.Views;

namespace Stareater.Controllers.Data
{
	public class GalaxySearchResult
	{
		public IList<StarData> Stars { get; set; }
		public IList<FleetInfo> Fleets { get; private set; }
		public IList<FoundGalaxyObject> FoundObjects { get; private set; }
		
		public GalaxySearchResult(IList<StarData> stars, IList<FleetInfo> idleFleets, IList<FoundGalaxyObject> foundObjects)
		{
			this.Stars = stars;
			this.Fleets = idleFleets;
			this.FoundObjects = foundObjects;
		}
	}
}
