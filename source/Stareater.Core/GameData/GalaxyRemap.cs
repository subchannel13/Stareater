using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.GameData 
{
	class GalaxyRemap 
	{
		public IDictionary<StarData, StarData> Stars;
		public IDictionary<Planet, Planet> Planets;

		public GalaxyRemap(IDictionary<StarData, StarData> stars, IDictionary<Planet, Planet> planets) 
		{
			this.Stars = stars;
			this.Planets = planets;
		}
	}
}
