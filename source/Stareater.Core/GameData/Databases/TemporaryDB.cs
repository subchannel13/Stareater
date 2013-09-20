using System;
using Stareater.GameData.Databases.Tables;

namespace Stareater.GameData.Databases
{
	class TemporaryDB
	{
		public ColonyProcessorCollection Colonies { get; private set; }
		
		public TemporaryDB()
		{
			this.Colonies = new ColonyProcessorCollection();
		}
	}
}
