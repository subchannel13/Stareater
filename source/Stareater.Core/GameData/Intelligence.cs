using Stareater.Utils.StateEngine;
using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.GameData.Databases;
using System.Linq;

namespace Stareater.GameData
{
	class Intelligence 
	{
		[StatePropertyAttribute]
		public Dictionary<Wormhole, bool> StarlaneKnowledge { get; set; }

		[StatePropertyAttribute]
		private Dictionary<StarData, StarIntelligence> starKnowledge { get; set; }

		public void Initialize(StatesDB states)
		{
			this.starKnowledge = states.Stars.ToDictionary(x => x, x => new StarIntelligence(states.Planets.At[x]));
			this.StarlaneKnowledge = states.Wormholes.ToDictionary(x => x, x => false);
		}

		public void StarFullyVisited(StarData star, StatesDB states)
		{
			var starInfo = starKnowledge[star];

			starInfo.Visit(0);
			foreach (var planetInfo in starInfo.Planets.Values)
				planetInfo.Discovered = true;
			foreach (var lane in states.Wormholes.At[star])
				this.StarlaneKnowledge[lane] = true;
		}

		public void StarVisited(StarData star, int turn)
		{
			var starInfo = starKnowledge[star];

			starInfo.Visit(turn);
		}

		public StarIntelligence About(StarData star)
		{
			return starKnowledge[star];
		}
	}
}
