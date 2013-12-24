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

		public override SiteType Type
		{
			get { return SiteType.StarSystem; }
		}

		internal StellarisAdmin Copy(Player player, StarData star)
		{
			var copy = new StellarisAdmin(player, star);

			foreach (var leftovers in this.Leftovers)
				copy.Leftovers.Add(leftovers.Key, leftovers.Value);

			return copy;
		}
	}
}
