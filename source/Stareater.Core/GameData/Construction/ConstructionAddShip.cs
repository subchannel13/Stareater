using Stareater.Galaxy;
using Stareater.Ships;
using Stareater.Ships.Missions;
using System;
using System.Linq;

namespace Stareater.GameData.Construction
{
	class ConstructionAddShip : IConstructionEffect
	{
		public Design Design { get; private set; }
		
		public ConstructionAddShip(Design design)
		{
			this.Design = design;
		}

		public void Apply(MainGame game, AConstructionSite site, long quantity)
		{
			//TODO(v0.8) report new ship construction
			var playerProc = game.Derivates[site.Owner];

			var colonies = game.States.Colonies.AtStar[site.Location.Star].Where(x => x.Owner == site.Owner).ToList();
			var systemPopulation = colonies.Sum(x => x.Population);
			var migrants = Math.Min(playerProc.DesignStats[this.Design].ColonizerPopulation * quantity, systemPopulation);

			playerProc.SpawnShip(site.Location.Star, this.Design, quantity, migrants, new AMission[0], game.States);

			var toMove = migrants;
			while(toMove >= 0.1)
			{
				foreach(var colony in colonies)
				{
					var delta = Math.Min(migrants * colony.Population / systemPopulation, colony.Population);
					colony.Population -= delta;
					toMove -= delta;
				}
				migrants = toMove;
				systemPopulation = colonies.Sum(x => x.Population);
			}
		}
	}
}
