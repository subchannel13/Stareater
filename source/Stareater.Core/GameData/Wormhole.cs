 
using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.GameData 
{
	public class Wormhole 
	{
		public StarData FromStar { get; private set; }
		public StarData ToStar { get; private set; }

		public Wormhole(StarData fromStar, StarData toStar) 
		{
			this.FromStar = fromStar;
			this.ToStar = toStar;
 
		} 


		internal Wormhole Copy(GalaxyRemap galaxyRemap) 
		{
			return new Wormhole(galaxyRemap.Stars[this.FromStar], galaxyRemap.Stars[this.ToStar]);
 
		} 
 
	}
}
