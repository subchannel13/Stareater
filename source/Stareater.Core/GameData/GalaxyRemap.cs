using System;
using System.Collections.Generic;

using Stareater.Galaxy;

namespace Stareater.GameData
{
	class GalaxyRemap
	{
		public IDictionary<StarData, StarData> Stars;
		public IDictionary<Planet, Planet> Planets;

		public GalaxyRemap()
		{
			this.Planets = new Dictionary<Planet, Planet>();
			this.Stars = new Dictionary<StarData, StarData>();
		}
	}
}
