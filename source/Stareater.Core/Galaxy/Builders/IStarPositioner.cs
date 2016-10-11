using System;
using System.Linq;
using Stareater.Utils.PluginParameters;

namespace Stareater.Galaxy.Builders
{
	public interface IStarPositioner
	{
		string Name { get; }
		string Description { get; }
		ParameterList Parameters { get; }
		StarPositions Generate(Random rng, int playerCount);
	}
}
