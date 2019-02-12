using System.Collections.Generic;
using System.Linq;
using Stareater.Galaxy;
using Stareater.Galaxy.BodyTraits;
using Stareater.Galaxy.Builders;
using Stareater.GameData.Databases;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.NewGameHelpers
{
	class SystemEvaluator
	{
		private readonly StaticsDB statics;

		public SystemEvaluator(StaticsDB statics)
		{
			this.statics = statics;
		}

		public double StartingScore(StarData star, IEnumerable<Planet> planets)
		{
			//TODO(later) what about star?
			return planets.Sum(x => this.startingScore(x));
		}

		public double PotentialScore(StarData star, IEnumerable<Planet> planets)
		{
			//TODO(v0.8) remove negative traits
			return planets.Sum(x => this.potentialScore(x));
		}

		public double BestSystemScore(IStarPopulator starPopulator)
		{
			//TODO(later) what about star?
			//TODO(v0.8) load traits from statics
			return starPopulator.MaxPlanets * bodyTypes().Max(x => this.potentialScore(
				new Planet(null, 0, x, starPopulator.MaxPlanetSize(x), new List<TraitType>())
			));
		}

		public double WorstSystemScore(IStarPopulator starPopulator)
		{
			//TODO(later) what about star?
			//TODO(v0.8) load traits from statics
			return starPopulator.MinPlanets * bodyTypes().Max(x => this.startingScore(
				new Planet(null, 0, x, starPopulator.MinPlanetSize(x), new List<TraitType>())
			));
		}

		private double startingScore(Planet planet)
		{
			var vars = new Var().
				Init(this.statics.Traits.Keys, -1).
				UnionWith(planet.Traits.Select(x => x.Type.IdCode)).
				Get;

			return this.statics.PlanetForumlas[planet.Type].StartingScore.Evaluate(vars);
		}

		private double potentialScore(Planet planet)
		{
			var vars = new Var(GameLogic.ColonyProcessor.PlanetSizeKey, planet.Size).
				Init(this.statics.Traits.Keys, -1).
				UnionWith(planet.Traits.Select(x => x.Type.IdCode)).
				Get;

			return this.statics.PlanetForumlas[planet.Type].PotentialScore.Evaluate(vars);
		}

		private static PlanetType[] bodyTypes()
		{
			return new[] { PlanetType.Asteriod, PlanetType.GasGiant, PlanetType.Rock };
		}
	}
}
