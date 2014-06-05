 
using System;
using System.Collections.Generic;
using Stareater.Galaxy;

namespace Stareater.GameData 
{
	partial class StarIntelligence 
	{
		public int LastVisited { get; private set; }
		public IDictionary<Planet, PlanetIntelligence> Planets { get; private set; }

		public StarIntelligence(IEnumerable<Planet> planets) 
		{
			this.LastVisited = NeverVisited;
			initPlanets(planets);
 
		} 

		private StarIntelligence(StarIntelligence original, GalaxyRemap galaxyRemap) 
		{
			this.LastVisited = original.LastVisited;
			copyPlanets(original, galaxyRemap);
 
		}

		internal StarIntelligence Copy(GalaxyRemap galaxyRemap) 
		{
			return new StarIntelligence(this, galaxyRemap);
 
		} 
 
	}
}
