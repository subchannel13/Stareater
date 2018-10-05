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
		private readonly PlayerController playerController;
		private readonly Player player;
		private readonly StarData star;
		
		public bool IsReadOnly { get; private set; }
		
		internal StarSystemController(MainGame game, StarData star, bool readOnly, PlayerController playerController)
		{
			this.game = game;
			this.playerController = playerController;
			this.player = playerController.PlayerInstance(game);
			this.IsReadOnly = readOnly;
			this.star = star;
		}

		public StarInfo HostStar
		{
			get { return new StarInfo(this.star); }
		}

		public IEnumerable<PlanetInfo> Planets
		{
			get {
				var planetInfos = this.player.Intelligence.About(this.star).Planets;
				var knownPlanets = planetInfos.Where(x => x.Value.Explored).Select(x => x.Key);
				
				return knownPlanets.OrderBy(x => x.Position).Select(x => new PlanetInfo(x));
			}
		}
		
		public ColonyInfo PlanetsColony(PlanetInfo planet)
		{
			if (this.player.Intelligence.About(this.star).Planets[planet.Data].LastVisited != PlanetIntelligence.NeverVisited)
				//TODO(later) show last known colony information
				if (game.States.Colonies.AtPlanet.Contains(planet.Data))
					return new ColonyInfo(game.States.Colonies.AtPlanet[planet.Data]);
			
			return null;
		}
		
		public StellarisInfo StarsAdministration()
		{
			var stellaris = game.States.Stellarises.At[this.star, this.player].FirstOrDefault();
			if (this.player.Intelligence.About(this.star).LastVisited != StarIntelligence.NeverVisited && stellaris != null)
				//TODO(later) show last known star system information
				return new StellarisInfo(stellaris, this.game);
			
			return null;
		}
		
		public BodyType BodyType(int bodyIndex)
		{
			if (bodyIndex == StarIndex) {
				if (game.States.Stellarises.At[this.star].Count == 0)
					return Views.BodyType.NoStellarises;
					
				var stellarises = game.States.Stellarises.At[this.star, this.player];
				
				return stellarises.Any() ?
					Views.BodyType.OwnStellaris : 
					Views.BodyType.ForeignStellaris;
			} 

			var planet = game.States.Planets.At[this.star].FirstOrDefault(x => x.Position == bodyIndex);

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
			var planet = this.game.States.Planets.At[this.star].First(x => x.Position == position);
			return planet != null && this.game.Orders[this.player].ColonizationOrders.ContainsKey(planet);
		}
		
		public ColonyController ColonyController(int bodyPosition)
		{
			var planet = game.States.Planets.At[this.star].FirstOrDefault(x => x.Position == bodyPosition);
			
			if (planet == null)
				throw new ArgumentOutOfRangeException("bodyPosition");

			return new ColonyController(game, game.States.Colonies.AtPlanet[planet], IsReadOnly, this.player);
		}

		public ColonizationController EmptyPlanetController(int bodyPosition)
		{
			var planet = game.States.Planets.At[this.star].FirstOrDefault(x => x.Position == bodyPosition);
			
			if (planet == null)
				throw new ArgumentOutOfRangeException("bodyPosition");
			
			return new ColonizationController(this.game, planet, IsReadOnly, this.playerController);
		}
		
		public StellarisAdminController StellarisController()
		{
			var stellaris = game.States.Stellarises.At[this.star, this.player].FirstOrDefault();
			return new StellarisAdminController(game, stellaris, IsReadOnly, this.player);
		}
	}
}
