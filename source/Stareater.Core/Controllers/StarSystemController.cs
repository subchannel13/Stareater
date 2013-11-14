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
		public bool IsReadOnly { get; private set; }
			
		internal StarSystemController(Game game, StarData star, bool readOnly)
		{
			this.game = game;
			this.IsReadOnly = readOnly;
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
		
		public bool IsColonised(int selectedBody)
		{
			var planet = game.States.Planets.At(Star).Where(x => x.Position == selectedBody).FirstOrDefault();
			
			if (planet == null)
				return false;
			
			return game.States.Colonies.AtPlanetContains(planet);
		}
		
		public ColonyController ColonyController(int bodyPosition)
		{
			var planet = game.States.Planets.At(Star).Where(x => x.Position == bodyPosition).FirstOrDefault();
			
			if (planet == null)
				throw new ArgumentOutOfRangeException("bodyPosition");

			return new ColonyController(game, game.States.Colonies.AtPlanet(planet), IsReadOnly);
		}
	}
}
