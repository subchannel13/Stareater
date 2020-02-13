using System;

namespace Stareater.Utils.StateEngine
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public class StateBaseTypeAttribute : Attribute
	{
		public string LoadMethod { get; private set; }
		public Type LoaderClass { get; private set; }

		public StateBaseTypeAttribute(string loadMethod, Type loaderClass)
		{
			this.LoaderClass = loaderClass;
			this.LoadMethod = loadMethod;
		}
	}
}
