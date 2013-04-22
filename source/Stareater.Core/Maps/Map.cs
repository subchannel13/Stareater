using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Maps
{
	class Map
	{
		private StarData[] stars;
		private Tuple<StarData, StarData>[] wormholes;

		public Map(IEnumerable<StarData> stars, IEnumerable<Tuple<StarData, StarData>> wormholes)
		{
			this.stars = stars.ToArray();
			this.wormholes = wormholes.ToArray();
		}
	}
}
