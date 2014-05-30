using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.Players;
using Stareater.Ships;

namespace Stareater.GameData 
{
	class PlayersRemap 
	{
		public IDictionary<Player, Player> Players;
		public IDictionary<AConstructionSite, Colony> Colonies;
		public IDictionary<AConstructionSite, StellarisAdmin> Stellarises;
		public IDictionary<Design, Design> Designs;
		public IDictionary<IdleFleet, IdleFleet> IdleFleets;

		public PlayersRemap(IDictionary<Player, Player> players, IDictionary<AConstructionSite, Colony> colonies, IDictionary<AConstructionSite, StellarisAdmin> stellarises, IDictionary<Design, Design> designs, IDictionary<IdleFleet, IdleFleet> idleFleets) 
		{
			this.Players = players;
			this.Colonies = colonies;
			this.Stellarises = stellarises;
			this.Designs = designs;
			this.IdleFleets = idleFleets;
		}

		internal AConstructionSite Site(AConstructionSite site)
		{
			if (Colonies.ContainsKey(site))
				return Colonies[site];
			else if (Stellarises.ContainsKey(site))
				return Stellarises[site];

			throw new KeyNotFoundException();
		}
	}
}
