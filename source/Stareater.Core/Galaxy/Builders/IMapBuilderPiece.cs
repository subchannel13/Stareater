using System;
using Stareater.Utils.PluginParameters;

namespace Stareater.Galaxy.Builders
{
	public interface IMapBuilderPiece
	{
		string Code { get; }
		ParameterList Parameters { get; }
	}
}
