using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Utils.PluginParameters;

namespace Stareater.Galaxy.Builders
{
	public interface IStarConnector : IMapBuilderPiece
	{
		string Name { get; }
		string Description { get; }
		IEnumerable<WormholeEndpoints> Generate(Random rng, StarPositions starPositions);
	}
}
