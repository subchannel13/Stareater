using System;
using System.Collections.Generic;
using Stareater.Galaxy.BodyTraits;

namespace Stareater.Galaxy.Builders
{
	public interface IStarPopulator : IMapBuilderPiece
	{
		string Name { get; }
		string Description { get; }
		IEnumerable<StarSystemBuilder> Generate(Random rng, StarPositions starPositions, IEnumerable<TraitType> planetTraits);
	}
}
