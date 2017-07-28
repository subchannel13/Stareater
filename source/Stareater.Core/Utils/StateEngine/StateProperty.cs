using System;

namespace Stareater.Utils.StateEngine
{
	class StateProperty : Attribute
	{
		public bool DoCopy { get; private set; }
		public bool DoSave { get; private set; }

		public StateProperty(bool doCopy = true, bool doSave = true)
		{
			this.DoCopy = doCopy;
			this.DoSave = doSave;
		}
	}
}
