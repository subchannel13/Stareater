using System;

namespace Stareater.Utils.PluginParameters
{
	public class ContinuousRangeParameter : ARangeParameter<double>
	{
		public ContinuousRangeParameter(string contextKey, string nameKey, double minimum, double maximum, double current, Func<double, string> valueDescriptor) :
			base(contextKey, nameKey, minimum, maximum, current, valueDescriptor)
		{ }

		#region implemented abstract members of AParameterBase

		public override void Accept(IParameterVisitor visitor)
		{
			visitor.Visit(this);
		}

		#endregion
	}
}
