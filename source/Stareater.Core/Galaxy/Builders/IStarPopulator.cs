using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Utils.PluginParameters;

namespace Stareater.Galaxy.Builders
{
	public interface IStarPopulator
	{
		string Name { get; }
		string Description { get; }
		ParameterList Parameters { get; }
		IEnumerable<StarSystem> Generate(Random rng, StarPositions starPositions);
	}
}
