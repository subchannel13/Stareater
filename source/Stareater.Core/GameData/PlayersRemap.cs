using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.Players;
using Stareater.Ships;
using Stareater.Ships.Missions;

namespace Stareater.GameData 
{
	class PlayersRemap 
	{
		public IDictionary<Player, Player> Players;
		public IDictionary<AConstructionSite, Colony> Colonies;
		public IDictionary<AConstructionSite, StellarisAdmin> Stellarises;
		public IDictionary<Design, Design> Designs;
		public IDictionary<Fleet, Fleet> Fleets;
		public IDictionary<ColonizationProject, ColonizationProject> Colonizations;
		public IDictionary<AMission, AMission> Missions;

		public PlayersRemap(IDictionary<Player, Player> players, IDictionary<AConstructionSite, Colony> colonies, IDictionary<AConstructionSite, StellarisAdmin> stellarises, 
		                    IDictionary<Design, Design> designs, IDictionary<Fleet, Fleet> fleets, IDictionary<ColonizationProject, ColonizationProject> colonizations, 
		                    IDictionary<AMission, AMission> missions)
		{
			this.Players = players;
			this.Colonies = colonies;
			this.Stellarises = stellarises;
			this.Designs = designs;
			this.Fleets = fleets;
			this.Colonizations = colonizations;
			this.Missions = missions;
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
