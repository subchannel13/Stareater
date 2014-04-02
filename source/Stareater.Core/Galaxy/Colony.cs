using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Players;
using Stareater.GameData;

namespace Stareater.Galaxy
{
	class Colony : AConstructionSite
	{
		public Colony(Player owner, Planet planet) : base(owner, new LocationBody(planet.Star, planet))
		{
			this.Population = 1;
		}

		protected Colony(Colony original, Planet planet, Player owner) : base(original, owner, new LocationBody(planet.Star, planet))
		{
			this.Population = original.Population;
		}

		public StarData Star
		{
			get {
				return Location.Star;
			}
		}
		
		public override SiteType Type
		{
			get { return SiteType.Colony; }
		}

		public double Population { get; set; }

		internal Colony Copy(Player player, Planet planet)
		{
			return new Colony(this, planet, player);
		}
	}
}
