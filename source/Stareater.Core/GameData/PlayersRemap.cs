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
		
		public PlayersRemap(IDictionary<Player, Player> players)
		{
			this.Players = players;
			this.Colonies = new Dictionary<AConstructionSite, Colony>();
		}

		internal AConstructionSite Site(AConstructionSite site)
		{
			if (Colonies.ContainsKey(site))
				return Colonies[site];

			throw new NotImplementedException();
		}
	}
}
