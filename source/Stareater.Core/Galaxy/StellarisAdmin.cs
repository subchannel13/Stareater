using System;
using Stareater.Players;
using Stareater.GameData;

namespace Stareater.Galaxy
{
	class StellarisAdmin : AConstructionSite
	{
		public StellarisAdmin(Player owner, StarData star) : base(owner, new LocationBody(star))
		{ }

		protected StellarisAdmin(StellarisAdmin original, StarData star, Player owner) : base(original, owner, new LocationBody(star))
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
