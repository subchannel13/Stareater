using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.GameData
{
	class PlayersRemap
	{
		public IDictionary<Player, Player> Players;
		public IDictionary<AConstructionSite, Colony> Colonies;
		public IDictionary<AConstructionSite, StellarisAdmin> Stellarises;

		public PlayersRemap(IDictionary<Player, Player> players, IDictionary<AConstructionSite, Colony> colonies, IDictionary<AConstructionSite, StellarisAdmin> stellarises) {
			this.Players = players;
			this.Colonies = colonies;
			this.Stellarises = stellarises;
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
