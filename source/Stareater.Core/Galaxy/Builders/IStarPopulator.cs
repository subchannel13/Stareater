using System;
using System.Collections.Generic;
using Stareater.Galaxy.BodyTraits;

namespace Stareater.Galaxy.Builders
{
	public interface IStarPopulator : IMapBuilderPiece
	{
		string Name { get; }
		string Description { get; }

		void SetGameData(Dictionary<string, StarTraitType> starTraits, Dictionary<string, PlanetTraitType> planetTraits, Dictionary<PlanetType, IEnumerable<string>> implicitTraits);

		IEnumerable<StarSystemBuilder> Generate(Random rng, SystemEvaluator evaluator, StarPositions starPositions);

		double MinScore { get; }
		double MaxScore { get; }
	}
}
