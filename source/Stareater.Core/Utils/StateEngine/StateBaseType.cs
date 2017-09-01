using System;

namespace Stareater.Utils.StateEngine
{
	class StateBaseType : Attribute
	{
		public string LoadMethod { get; private set; }
		public Type LoaderClass { get; private set; }

		public StateBaseType(string loadMethod, Type loaderClass)
		{
			this.LoaderClass = loaderClass;
			this.LoadMethod = loadMethod;
		}
	}
}
