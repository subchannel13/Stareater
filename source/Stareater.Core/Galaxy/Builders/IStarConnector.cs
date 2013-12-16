using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Utils.PluginParameters;
using NGenerics.DataStructures.Mathematical;

namespace Stareater.Galaxy.Builders
{
	public interface IStarConnector
	{
		string Name { get; }
		string Description { get; }
		ParameterList Parameters { get; }
		IEnumerable<WormholeEndpoints> Generate(Random rng, StarPositions starPositions);
	}
}
