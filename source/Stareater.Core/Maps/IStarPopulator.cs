using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Utils.PluginParameters;

namespace Stareater.Maps
{
	public interface IStarPopulator
	{
		string Name { get; }
		ParameterList Parameters { get; }
	}
}
