using System;
using System.Collections.Generic;
using Stareater.Controllers.Data.Ships;
using Stareater.Galaxy;

namespace Stareater.Controllers.Data
{
	public class GalaxySearchResult
	{
		public IList<StarData> Stars { get; set; }
		public IList<FleetInfo> IdleFleets { get; private set; }
		public IList<FoundGalaxyObject> FoundObjects { get; private set; }
		
		public GalaxySearchResult(IList<StarData> stars, IList<FleetInfo> idleFleets, IList<FoundGalaxyObject> foundObjects)
		{
			this.Stars = stars;
			this.IdleFleets = idleFleets;
			this.FoundObjects = foundObjects;
		}
	}
}
