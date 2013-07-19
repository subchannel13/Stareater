using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Galaxy.Builders;

namespace Stareater.Galaxy
{
	class Map
	{
		public StarsCollection Stars { get; private set; }
		public WormholeCollection Wormholes { get; private set; }
		public PlanetsCollection Planets { get; private set; }

		public Map(IEnumerable<StarSystem> starSystems, IEnumerable<Tuple<int, int>> wormholes)
		{
			StarData[] starList = starSystems.Select(x => x.Star).ToArray();
			
			this.Stars = new StarsCollection();
			this.Stars.Add(starList);
			
			this.Wormholes = new WormholeCollection();
			this.Wormholes.Add(wormholes.Select(x => new Tuple<StarData, StarData>(starList[x.Item1], starList[x.Item2])));
			
			this.Planets = new PlanetsCollection();
			foreach(var system in starSystems)
				this.Planets.Add(system.Planets);
		}
	}
}
