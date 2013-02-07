using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Utils.PluginParameters
{
	public class RangeParameter<T> : ParameterBase
	{
		public T Minimum { get; private set; }
		public T Maximum { get; private set; }
		public T Value { get; set; }

		private Func<T, string> valueDescriptor;

		public RangeParameter(string contextKey, string nameKey, T minimum, T maximum, T current, Func<T, string> valueDescriptor) :
			base(contextKey, nameKey)
		{
			this.Minimum = minimum;
			this.Maximum = maximum;
			this.Value = current;
			this.valueDescriptor = valueDescriptor;
		}

		public string ValueDescription
		{
			get { return valueDescriptor(Value); }
		}
	}
}
