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
		public IDictionary<AConstructionSite, AConstructionSite> Sites;
		
		public PlayersRemap(IDictionary<Player, Player> players)
		{
			this.Players = players;
			this.Sites = new Dictionary<AConstructionSite, AConstructionSite>();
		}
	}
}
