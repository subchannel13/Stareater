using System;

namespace Stareater.Utils.StateEngine
{
	class StateBaseType : Attribute
	{
		public string LoadMethod { get; private set; }
		public Type LoaderClass { get; private set; }

		public StateBaseType(Type loaderClass = null, string loadMethod = null)
		{
			this.LoaderClass = loaderClass;
			this.LoadMethod = loadMethod;
		}
	}
}
