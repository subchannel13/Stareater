using System;

namespace Stareater.Utils.StateEngine
{
	public class StateType : Attribute
	{
		public bool NotStateData { get; private set; }
		public string SaveTag { get; private set; }
		public string SaveMethod { get; private set; }
		public string LoadMethod { get; private set; }

		public StateType(bool notStateData = false, string saveTag = null, string saveMethod = null, string loadMethod = null)
		{
			this.NotStateData = notStateData;
			this.SaveMethod = saveMethod;
			this.SaveTag = saveTag;
			this.LoadMethod = loadMethod;
		}
	}
}
