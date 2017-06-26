using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Views;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.Players;

namespace Stareater.Controllers
{
	public class StarSystemController
	{
		public const int StarIndex = -1;
		
		private readonly MainGame game;
		private readonly Player player;
		
		public StarData Star { get; private set; }
		public bool IsReadOnly { get; private set; }
		
		internal StarSystemController(MainGame game, StarData star, bool readOnly, Player player)
		{
			this.game = game;
			this.player = player;
			this.IsReadOnly = readOnly;
			this.Star = star;
		}
		
		public IEnumerable<Planet> Planets
		{
			get {
				var planetInfos = this.player.Intelligence.About(Star).Planets;
				var knownPlanets = planetInfos.Where(x => x.Value.Explored >= PlanetIntelligence.FullyExplored).Select(x => x.Key);
				
				return knownPlanets.OrderBy(x => x.Position);
			}
		}
		
		public ColonyInfo PlanetsColony(Planet planet)
		{
			if (this.player.Intelligence.About(Star).Planets[planet].LastVisited != PlanetIntelligence.NeverVisited)
				//TODO(later) show last known colony information
				if (game.States.Colonies.AtPlanet.Contains(planet))
					return new ColonyInfo(game.States.Colonies.AtPlanet[planet]);
			
			return null;
		}
		
		public StellarisInfo StarsAdministration()
		{
			var stellaris = game.States.Stellarises.At[Star].FirstOrDefault(x => x.Owner == this.player);
			if (this.player.Intelligence.About(Star).LastVisited != StarIntelligence.NeverVisited && stellaris != null)
				//TODO(later) show last known star system information
				return new StellarisInfo(stellaris, this.game);
			
			return null;
		}
		
		public BodyType BodyType(int bodyIndex)
		{
			if (bodyIndex == StarIndex) {
				if (game.States.Stellarises.At[Star].Count == 0)
					return Views.BodyType.NoStellarises;
					
				var stellarises = game.States.Stellarises.At[Star];
				
				return stellarises.Any(x => x.Owner == this.player) ?
					Views.BodyType.OwnStellaris : 
					Views.BodyType.ForeignStellaris;
			} 

			var planet = game.States.Planets.At[Star].FirstOrDefault(x => x.Position == bodyIndex);

			if (planet == null)
				return Views.BodyType.Empty;
			if (!game.States.Colonies.AtPlanet.Contains(planet))
				return Views.BodyType.NotColonised;

			var colony = game.States.Colonies.AtPlanet[planet];

			return colony.Owner == this.player ? 
				Views.BodyType.OwnColony : 
				Views.BodyType.ForeignColony;
		}

		public bool IsColonizing(int position)
		{
			var planet = this.game.States.Planets.At[this.Star].First(x => x.Position == position);
			return planet != null && this.player.Orders.ColonizationOrders.ContainsKey(planet);
		}
		
		public ColonyController ColonyController(int bodyPosition)
		{
			var planet = game.States.Planets.At[Star].FirstOrDefault(x => x.Position == bodyPosition);
			
			if (planet == null)
				throw new ArgumentOutOfRangeException("bodyPosition");

			return new ColonyController(game, game.States.Colonies.AtPlanet[planet], IsReadOnly, this.player);
		}

		public ColonizationController EmptyPlanetController(int bodyPosition)
		{
			var planet = game.States.Planets.At[Star].FirstOrDefault(x => x.Position == bodyPosition);
			
			if (planet == null)
				throw new ArgumentOutOfRangeException("bodyPosition");
			
			return new ColonizationController(this.game, planet, IsReadOnly, this.player);
		}
		
		public StellarisAdminController StellarisController()
		{
			var stellaris = game.States.Stellarises.At[Star].FirstOrDefault(x => x.Owner == this.player);
			return new StellarisAdminController(game, stellaris, IsReadOnly, this.player);
		}
	}
}
