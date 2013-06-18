using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Galaxy
{
	class Map
	{
		public StarData[] Stars { get; private set; }
		public Tuple<StarData, StarData>[] Wormholes { get; private set; }

		public Map(IEnumerable<StarData> stars, IEnumerable<Tuple<int, int>> wormholes)
		{
			this.Stars = stars.ToArray();
			this.Wormholes = wormholes.Select(x => new Tuple<StarData, StarData>(Stars[x.Item1], Stars[x.Item2])).ToArray();
		}
	}
}
