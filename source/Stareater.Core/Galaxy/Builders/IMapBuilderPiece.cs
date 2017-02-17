using System;
using System.Collections.Generic;
using Stareater.Utils.PluginParameters;

namespace Stareater.Galaxy.Builders
{
	public interface IMapBuilderPiece
	{
		string Code { get; }
		IEnumerable<AParameterBase> Parameters { get; }
	}
}
