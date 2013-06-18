using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Utils.PluginParameters;
using System.Drawing;

namespace Stareater.Galaxy
{
	public interface IStarPositioner
	{
		string Name { get; }
		string Description { get; }
		ParameterList Parameters { get; }
		StarPositions Generate(Random rng, int playerCount);
	}
}
