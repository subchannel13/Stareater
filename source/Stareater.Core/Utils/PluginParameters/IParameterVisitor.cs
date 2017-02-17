using System;

namespace Stareater.Utils.PluginParameters
{
	public interface IParameterVisitor
	{
		void Visit(ContinuousRangeParameter parameter);
		void Visit(DiscreteRangeParameter parameter);
		void Visit(SelectorParameter parameter);
	}
}
