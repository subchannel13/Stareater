using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Maps
{
	class Map
	{
		public StarData[] Stars { get; private set; }
		public Tuple<StarData, StarData>[] Wormholes { get; private set; }

		public Map(IEnumerable<StarData> stars, IEnumerable<Tuple<StarData, StarData>> wormholes)
		{
			this.Stars = stars.ToArray();
			this.Wormholes = wormholes.ToArray();
		}
	}
}
