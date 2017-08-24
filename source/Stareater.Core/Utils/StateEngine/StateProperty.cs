using System;

namespace Stareater.Utils.StateEngine
{
	class StateProperty : Attribute
	{
		public bool DoCopy { get; private set; }
		public bool DoSave { get; private set; }
		public string SaveKey { get; private set; }

		public StateProperty(bool doCopy = true, bool doSave = true, string saveKey = null)
		{
			this.DoCopy = doCopy;
			this.DoSave = doSave;
			this.SaveKey = saveKey;
		}
	}
}
