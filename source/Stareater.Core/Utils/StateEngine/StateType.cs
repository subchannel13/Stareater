using System;

namespace Stareater.Utils.StateEngine
{
	class StateType : Attribute
	{
		public bool NotStateData { get; private set; }
		public string SaveMethod { get; private set; }

		public StateType(bool notStateData = false, string saveMethod = null)
		{
			this.NotStateData = notStateData;
			this.SaveMethod = saveMethod;
		}
	}
}
