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
		public const int StarIndex = -1;
		
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

		/*public bool IsColonised(int bodyIndex)
		{
			var planet = game.States.Planets.At(Star).Where(x => x.Position == bodyIndex).FirstOrDefault();
			
			if (planet == null)
				return false;
			
			return game.States.Colonies.AtPlanetContains(planet);
		}*/
		
		public BodyType BodyType(int bodyIndex)
		{
			if (bodyIndex == StarIndex)
				return Data.BodyType.NoStarManagement; //TODO: check if there is management

			var planet = game.States.Planets.At(Star).Where(x => x.Position == bodyIndex).FirstOrDefault();

			if (planet == null)
				return Data.BodyType.Empty;
			if (!game.States.Colonies.AtPlanetContains(planet))
				return Data.BodyType.NotColonised;

			var colony = game.States.Colonies.AtPlanet(planet);

			if (colony.Owner == game.Players[game.CurrentPlayer])
				return Data.BodyType.OwnColony;
			else
				return Data.BodyType.ForeignColony;
		}

		public ColonyController ColonyController(int bodyPosition)
		{
			var planet = game.States.Planets.At(Star).Where(x => x.Position == bodyPosition).FirstOrDefault();
			
			if (planet == null)
				throw new ArgumentOutOfRangeException("bodyPosition");

			return new ColonyController(game, game.States.Colonies.AtPlanet(planet), IsReadOnly);
		}
		
		public StarManagementController StarController(int bodyPosition)
		{
			return new StarManagementController(game, null, IsReadOnly);
		}
	}
}
