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
				return this.player.Intelligence.About(this.star).Planets.
					Where(x => x.Value.Explored).
					OrderBy(x => x.Key.Position).
					Select(x => new PlanetInfo(x.Key, this.game));
			}
		}

		public IEnumerable<ColonyInfo> Colonies
		{
			get
			{
				var systemIntell = this.player.Intelligence.About(this.star).Planets;

				//TODO(later) show last known colony information
				return game.States.Colonies.
					AtStar[this.star].
					Where(x => systemIntell[x.Location.Planet].Explored).
					Select(x => new ColonyInfo(x, game.Derivates[x]));
			}
		}

		public ColonyInfo PlanetsColony(PlanetInfo planet)
		{
			if (planet == null)
				throw new ArgumentNullException(nameof(planet));

			if (this.player.Intelligence.About(this.star).Planets[planet.Data].Explored)
				if (game.States.Colonies.AtPlanet.Contains(planet.Data))
				{
					var colony = game.States.Colonies.AtPlanet[planet.Data];
					//TODO(later) show last known colony information
					return new ColonyInfo(colony, game.Derivates[colony]);
				}
			
			return null;
		}

		public StellarisInfo StarsAdministration()
		{
			var stellarises = game.States.Stellarises.At;
			if (this.player.Intelligence.About(this.star).LastVisited == StarIntelligence.NeverVisited || !stellarises.Contains(this.star, this.player))
				return null;

			//TODO(later) show last known star system information
			return new StellarisInfo(stellarises[this.star, this.player], this.game);
		}
		
		public BodyType BodyType(int bodyIndex)
		{
			if (bodyIndex == StarIndex) {
				if (game.States.Stellarises.At[this.star].Count == 0)
					return Views.BodyType.NoStellarises;
					
				return game.States.Stellarises.At.Contains(this.star, this.player) ?
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
			return planet != null && this.game.Orders[this.player].ColonizationTargets.Contains(planet);
		}
		
		public ColonyController ColonyController(int bodyPosition)
		{
			var planet = game.States.Planets.At[this.star].FirstOrDefault(x => x.Position == bodyPosition);
			
			if (planet == null)
				throw new ArgumentOutOfRangeException(nameof(bodyPosition));

			return new ColonyController(game, game.States.Colonies.AtPlanet[planet], IsReadOnly, this.player);
		}

		public ColonizationController EmptyPlanetController(int bodyPosition)
		{
			var planet = game.States.Planets.At[this.star].FirstOrDefault(x => x.Position == bodyPosition);
			
			if (planet == null)
				throw new ArgumentOutOfRangeException(nameof(bodyPosition));
			
			return new ColonizationController(this.game, planet, IsReadOnly, this.playerController);
		}
		
		public StellarisAdminController StellarisController()
		{
			return new StellarisAdminController(game, game.States.Stellarises.At[this.star, this.player], IsReadOnly, this.player);
		}

		public bool CanSurveyStar => true;
	}
}
