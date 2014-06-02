using System;
using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.Utils;

namespace Stareater.GameData
{
	partial class StarIntelligence
	{
		public const int NeverVisited = -1;
		
		private void initPlanets(IEnumerable<Planet> planets)
		{
			this.Planets = new Dictionary<Planet, PlanetIntelligence>();
			foreach(var planet in planets)
				this.Planets.Add(planet, new PlanetIntelligence());
		}
		
		private void copyPlanets(StarIntelligence original, GalaxyRemap galaxyRemap)
		{
			this.Planets = new Dictionary<Planet, PlanetIntelligence>();
			foreach (var planetIntell in original.Planets)
				this.Planets.Add(galaxyRemap.Planets[planetIntell.Key], planetIntell.Value.Copy());
		}
		
		public bool IsVisited
		{
			get { return LastVisited != NeverVisited; }
		}
		
		public void Visit(int turn)
		{
			this.LastVisited = turn;
		}

		/*internal StarIntelligence Copy(IDictionary<Planet, Planet> planetRemap)
		{
			StarIntelligence copy = new StarIntelligence();

			copy.LastVisited = this.LastVisited;
			copy.Planets = new Dictionary<Planet, PlanetIntelligence>();

			foreach (var planetIntell in Planets)
				copy.Planets.Add(planetRemap[planetIntell.Key], planetIntell.Value.Copy());

			return copy;
		}*/
	}
}
