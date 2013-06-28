using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Galaxy
{
	class Map
	{
		public StarsCollection Stars { get; private set; }
		public WormholeCollection Wormholes { get; private set; }

		public Map(IEnumerable<StarData> stars, IEnumerable<Tuple<int, int>> wormholes)
		{
			this.Stars = new StarsCollection();
			this.Stars.Add(stars);
			
			StarData[] starList = stars.ToArray();
			this.Wormholes = new WormholeCollection();
			this.Wormholes.Add(wormholes.Select(x => new Tuple<StarData, StarData>(starList[x.Item1], starList[x.Item2])));
		}
	}
}
