 
using System;
using System.Linq;
using Stareater.GameData;
using Stareater.Players;

namespace Stareater.Galaxy 
{
	partial class StellarisAdmin : AConstructionSite 
	{
		
		public StellarisAdmin(StarData star, Player owner) : base(new LocationBody(star), owner) 
		{
			 
		} 

		internal StellarisAdmin(StellarisAdmin original, StarData star, Player owner) : base(original, new LocationBody(star), owner) 
		{
			 
		}

		internal StellarisAdmin Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap)
		{
			return new StellarisAdmin(this, galaxyRemap.Stars[this.Location.Star], playersRemap.Players[this.Owner]);
		}
 
	}
}
