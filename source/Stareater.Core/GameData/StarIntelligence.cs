using System;
using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.Utils;

namespace Stareater.GameData
{
	public class StarIntelligence
	{
		public const int NeverVisited = -1;
		
		public int LastVisited { get; private set; }
		public IDictionary<Planet, PlanetIntelligence> Planets { get; private set; }
			
		public StarIntelligence(IEnumerable<Planet> planets)
		{
			this.LastVisited = NeverVisited;
			
			this.Planets = new Dictionary<Planet, PlanetIntelligence>();
			foreach(var planet in planets)
				this.Planets.Add(planet, new PlanetIntelligence());
		}

		private StarIntelligence()
		{ }

		public bool IsVisited
		{
			get { return LastVisited != NeverVisited; }
		}
		
		public void Visit(int turn)
		{
			this.LastVisited = turn;
		}

		internal StarIntelligence Copy(IDictionary<Planet, Planet> planetRemap)
		{
			StarIntelligence copy = new StarIntelligence();

			copy.LastVisited = this.LastVisited;
			copy.Planets = new Dictionary<Planet, PlanetIntelligence>();

			foreach (var planetIntell in Planets)
				copy.Planets.Add(planetRemap[planetIntell.Key], planetIntell.Value.Copy());

			return copy;
		}
	}
}
