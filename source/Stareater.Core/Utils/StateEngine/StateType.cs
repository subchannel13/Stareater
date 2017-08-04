using System;

namespace Stareater.Utils.StateEngine
{
	class StateType : Attribute
	{
		public bool NotStateData { get; private set; }
		public string SaveMethod { get; private set; }
		public string LoadMethod { get; private set; }
		public Type LoaderClass { get; private set; }

		public StateType(bool notStateData = false, string saveMethod = null, Type loaderClass = null, string loadMethod = null)
		{
			this.NotStateData = notStateData;
			this.SaveMethod = saveMethod;
			this.LoaderClass = loaderClass;
			this.LoadMethod = loadMethod;
		}
	}
}
