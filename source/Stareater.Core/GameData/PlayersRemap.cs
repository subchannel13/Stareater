using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.GameData
{
	class PlayersRemap
	{
		public IDictionary<Player, Player> Players;
		public IDictionary<AConstructionSite, Colony> Colonies;
		public IDictionary<AConstructionSite, StellarisAdmin> Stellarises;
		
		public PlayersRemap(IDictionary<Player, Player> players)
		{
			this.Players = players;
			this.Colonies = new Dictionary<AConstructionSite, Colony>();
			this.Stellarises = new Dictionary<AConstructionSite, StellarisAdmin>();
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
