using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Utils.PluginParameters;

namespace Stareater.Galaxy.Builders
{
	public interface IStarPopulator : IMapBuilderPiece
	{
		string Name { get; }
		string Description { get; }
		IEnumerable<StarSystem> Generate(Random rng, StarPositions starPositions, IEnumerable<BodyTraitType> planetTraits);
	}
}
