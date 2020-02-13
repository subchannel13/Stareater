using System;

namespace Stareater.Utils.PluginParameters
{
	public class DiscreteRangeParameter : ARangeParameter<int>
	{
		public DiscreteRangeParameter(string contextKey, string nameKey, int minimum, int maximum, int current, Func<int, string> valueDescriptor) :
			base(contextKey, nameKey, minimum, maximum, current, valueDescriptor)
		{ }
		
		#region implemented abstract members of AParameterBase
		
		public override void Accept(IParameterVisitor visitor)
		{
			if (visitor == null)
				throw new ArgumentNullException(nameof(visitor));

			visitor.Visit(this);
		}
		
		#endregion
	}
}
