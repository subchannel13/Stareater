using System;
using System.Collections.Generic;
using Stareater.GameData.Databases.Tables;
using Stareater.GameLogic;
using Stareater.Players;

namespace Stareater.GameData.Databases
{
	class TemporaryDB
	{
		public ColonyProcessorCollection Colonies { get; private set; }
		public PlayerProcessorCollection Players { get; private set; }
		
		public TemporaryDB(Player[] players, IEnumerable<Technology> technologies)
		{
			this.Colonies = new ColonyProcessorCollection();
			this.Players = new PlayerProcessorCollection();
			
			foreach (var player in players)
				this.Players.Add(new PlayerProcessor(player, technologies));
		}
	}
}
