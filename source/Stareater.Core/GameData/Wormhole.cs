using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.GameData 
{
	public class Wormhole 
	{
		public StarData FromStar;
		public StarData ToStar;

		public Wormhole(StarData fromStar, StarData toStar) 
		{
			this.FromStar = fromStar;
			this.ToStar = toStar;
		}
	}
}
