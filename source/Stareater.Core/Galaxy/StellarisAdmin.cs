using System;
using Stareater.Players;
using Stareater.GameData;

namespace Stareater.Galaxy
{
	class StellarisAdmin : AConstructionSite
	{
		public StellarisAdmin(Player owner, StarData star) : base(new LocationBody(star), owner)
		{ }

		protected StellarisAdmin(StellarisAdmin original, StarData star, Player owner) : base(original, new LocationBody(star), owner)
		{ }
		
		public override SiteType Type
		{
			get { return SiteType.StarSystem; }
		}

		internal StellarisAdmin Copy(Player player, StarData star)
		{
			return new StellarisAdmin(this, star, player);
		}
	}
}
