using System;
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
			return planets.Sum(planet =>
			{
				var typeInfo = this.statics.PlanetForumlas[planet.Type];
				var traits = new HashSet<string>(planet.Traits.Select(x => x.Type.IdCode));
				traits.RemoveWhere(x => typeInfo.WorstTraits.Contains(x) && !typeInfo.UnchangeableTraits.Contains(x));
				traits.UnionWith(typeInfo.BestTraits.Where(x=> !typeInfo.UnchangeableTraits.Contains(x)));

				return this.potentialScore(new Planet(
					planet.Star, planet.Position, planet.Type, planet.Size, 
					traits.Select(x => this.statics.Traits[x]).ToList()
				));
			});
		}

		public double BestSystemScore(IStarPopulator starPopulator)
		{
			//TODO(later) what about star?
			return starPopulator.MaxPlanets * bodyTypes().Max(x => this.potentialScore(
				new Planet(null, 0, x, starPopulator.MaxPlanetSize(x), this.traitList(this.statics.PlanetForumlas[x].BestTraits))
			));
		}

		public double WorstSystemScore(IStarPopulator starPopulator)
		{
			//TODO(later) what about star?
			return starPopulator.MinPlanets * bodyTypes().Max(x => this.startingScore(
				new Planet(null, 0, x, starPopulator.MinPlanetSize(x), this.traitList(this.statics.PlanetForumlas[x].WorstTraits))
			));
		}

		private double startingScore(Planet planet)
		{
			var vars = new Var().
				Init(this.statics.Traits.Keys, -1).
				UnionWith(this.statics.PlanetForumlas[planet.Type].ImplicitTraits).
				UnionWith(planet.Traits.Select(x => x.Type.IdCode)).
				Get;

			return this.statics.PlanetForumlas[planet.Type].StartingScore.Evaluate(vars);
		}

		private double potentialScore(Planet planet)
		{
			var vars = new Var(GameLogic.ColonyProcessor.PlanetSizeKey, planet.Size).
				Init(this.statics.Traits.Keys, -1).
				UnionWith(this.statics.PlanetForumlas[planet.Type].ImplicitTraits).
				UnionWith(planet.Traits.Select(x => x.Type.IdCode)).
				Get;

			return this.statics.PlanetForumlas[planet.Type].PotentialScore.Evaluate(vars);
		}

		private List<TraitType> traitList(IEnumerable<string> traitIds)
		{
			return traitIds.Select(x => this.statics.Traits[x]).ToList();
		}

		private static PlanetType[] bodyTypes()
		{
			return new[] { PlanetType.Asteriod, PlanetType.GasGiant, PlanetType.Rock };
		}
	}
}
