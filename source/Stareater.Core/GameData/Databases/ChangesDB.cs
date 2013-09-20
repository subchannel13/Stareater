using System;
using System.Collections.Generic;

namespace Stareater.GameData.Databases
{
	class ChangesDB
	{
		public IDictionary<string, int> DevelopmentQueue { get; private set; }
		
		public ChangesDB()
		{
			DevelopmentQueue = new Dictionary<string, int>();
		}
		
		public void Clear()
		{
			DevelopmentQueue.Clear();
		}
	}
}
