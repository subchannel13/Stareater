using Stareater.Utils.StateEngine;
using System.Collections.Generic;
using Stareater.Galaxy;
using System.Linq;

namespace Stareater.GameData
{
	class StarIntelligence 
	{
		public const int NeverVisited = -1;

		[StatePropertyAttribute]
		public int LastVisited { get; private set; }

		[StatePropertyAttribute]
		public Dictionary<Planet, PlanetIntelligence> Planets { get; private set; }

		public StarIntelligence(IEnumerable<Planet> planets) 
		{
			this.LastVisited = NeverVisited;
			this.Planets = planets.ToDictionary(x => x, x => new PlanetIntelligence());
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
	}
}
