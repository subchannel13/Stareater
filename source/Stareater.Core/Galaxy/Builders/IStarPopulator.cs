using System;
using System.Collections.Generic;
using Stareater.Galaxy.BodyTraits;

namespace Stareater.Galaxy.Builders
{
	public interface IStarPopulator : IMapBuilderPiece
	{
		string Name { get; }
		string Description { get; }

		void SetGameData(IEnumerable<TraitType> planetTraits);

		IEnumerable<StarSystemBuilder> Generate(Random rng, StarPositions starPositions);

		double MinPlanets { get; }
		double MaxPlanets { get; }
		double MinPlanetSize(PlanetType type);
		double MaxPlanetSize(PlanetType type);
	}
}
