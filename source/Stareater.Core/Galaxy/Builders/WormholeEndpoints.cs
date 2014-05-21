using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.Galaxy.Builders 
{
	public class WormholeEndpoints 
	{
		public int FromIndex;
		public int ToIndex;

		public WormholeEndpoints(int fromIndex, int toIndex) 
		{
			this.FromIndex = fromIndex;
			this.ToIndex = toIndex;
		}
	}
}
