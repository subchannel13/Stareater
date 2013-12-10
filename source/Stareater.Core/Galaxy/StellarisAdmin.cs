using System;
using Stareater.Players;

namespace Stareater.Galaxy
{
	class StellarisAdmin : AConstructionSite
	{
		public StarData Location { get; private set; }
		
		public StellarisAdmin(Player owner, StarData star) : base(owner)
		{
			this.Location = star;
		}
		
		internal StellarisAdmin Copy(Player player, StarData star)
		{
			return new StellarisAdmin(player, star);
		}
	}
}
