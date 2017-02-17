using System;
using System.Linq;

namespace Stareater.Utils.PluginParameters
{
	public abstract class ARangeParameter<T> : AParameterBase
	{
		public T Minimum { get; private set; }
		public T Maximum { get; private set; }

		private T selectedValue;
		private readonly Func<T, string> valueDescriptor;

		protected ARangeParameter(string contextKey, string nameKey, T minimum, T maximum, T current, Func<T, string> valueDescriptor) :
			base(contextKey, nameKey)
		{
			this.Minimum = minimum;
			this.Maximum = maximum;
			this.Value = current;
			this.valueDescriptor = valueDescriptor;
		}

		public T Value 
		{ 
			get
			{
				return this.selectedValue;
			}
			
			set
			{
				this.selectedValue = Methods.Clamp(value, this.Minimum, this.Maximum);
			}
		}
		
		public string ValueDescription
		{
			get { return valueDescriptor(Value); }
		}
	}
}
