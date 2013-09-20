using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Data;
using Stareater.Galaxy;
using Stareater.GameData;

namespace Stareater.Controllers
{
	public class StarSystemController
	{
		private Game game;
		
		public StarData Star { get; private set; }
			
		internal StarSystemController(Game game, StarData star)
		{
			this.game = game;
			this.Star = star;
		}
		
		public IEnumerable<Planet> Planets
		{
			get {
				var planetInfos = game.Players[game.CurrentPlayer].Intelligence.About(Star).Planets;
				var knownPlanets = planetInfos.Where(x => x.Value.Explored == PlanetIntelligence.FullyExplored).Select(x => x.Key);
				
				return knownPlanets.OrderBy(x => x.Position);
			}
		}
		
		public ColonyInfo PlanetsColony(Planet planet)
		{
			if (game.Players[game.CurrentPlayer].Intelligence.About(Star).Planets[planet].LastVisited != PlanetIntelligence.NeverVisited)
				//TODO: show last known colony information
				if (game.States.Colonies.AtPlanetContains(planet))
					return new ColonyInfo(game.States.Colonies.AtPlanet(planet));
			
			return null;
		}
	}
}
