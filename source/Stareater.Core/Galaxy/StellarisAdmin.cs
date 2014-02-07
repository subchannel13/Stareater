using System;
using Stareater.Players;
using Stareater.GameData;

namespace Stareater.Galaxy
{
	class StellarisAdmin : AConstructionSite
	{
		public StarData Location { get; private set; }
		
		public StellarisAdmin(Player owner, StarData star) : base(owner)
		{
			this.Location = star;
		}

		protected StellarisAdmin(StellarisAdmin original, StarData star, Player owner) : base(original, owner)
		{
			this.Location = star;
		}
		
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
