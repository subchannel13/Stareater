using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Utils.PluginParameters;

namespace Stareater.Galaxy
{
	public interface IStarPopulator
	{
		string Name { get; }
		string Description { get; }
		ParameterList Parameters { get; }
		IEnumerable<StarData> Generate(Random rng, StarPositions starPositions);
	}
}
