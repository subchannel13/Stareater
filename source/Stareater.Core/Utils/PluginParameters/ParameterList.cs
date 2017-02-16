using System;
using System.Collections.Generic;
using System.Linq;

namespace Stareater.Utils.PluginParameters
{
	//TODO convert to visitor pattern
	public class ParameterList
	{
		public SelectorParameter[] Selectors { get; private set; }
		public RangeParameter<int>[] IntegerRanges { get; private set; }
		public RangeParameter<double>[] RealRanges { get; private set; }

		private readonly Dictionary<ParameterBase, int> parameterOrder = new Dictionary<ParameterBase, int>();

		public ParameterList(params ParameterBase[] parameters)
		{
			var selectorList = new List<SelectorParameter>();
			var integerRangeList = new List<RangeParameter<int>>();
			var realRangeList = new List<RangeParameter<double>>();

			for (int i = 0; i < parameters.Length; i++) {
				if (parameters[i] is SelectorParameter)
					selectorList.Add(parameters[i] as SelectorParameter);
				else if (parameters[i] is RangeParameter<int>)
					integerRangeList.Add(parameters[i] as RangeParameter<int>);
				else if (parameters[i] is RangeParameter<double>)
					realRangeList.Add(parameters[i] as RangeParameter<double>);

				parameterOrder.Add(parameters[i], i);
			}

			Selectors = selectorList.ToArray();
			IntegerRanges = integerRangeList.ToArray();
			RealRanges = realRangeList.ToArray();
		}

		public int IndexOf(ParameterBase parameter)
		{
			return parameterOrder[parameter];
		}
	}
}
