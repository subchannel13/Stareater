using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.GameData.Databases.Tables;
using Stareater.GameLogic;
using Stareater.Players;
using Stareater.Galaxy;

namespace Stareater.GameData.Databases
{
	class TemporaryDB
	{
		public ColonyProcessorCollection Colonies { get; private set; }
		public StellarisProcessorCollection Stellarises { get; private set; }
		public PlayerProcessorCollection Players { get; private set; }
		public NativesProcessor Natives { get; private set; }
		
		public TemporaryDB(Player[] players, Player organellePlayer, IEnumerable<DevelopmentTopic> technologies)
		{
			this.Colonies = new ColonyProcessorCollection();
			this.Stellarises = new StellarisProcessorCollection();
			this.Players = new PlayerProcessorCollection();
			this.Natives = new NativesProcessor(organellePlayer);
			
			foreach (var player in players)
				this.Players.Add(new PlayerProcessor(player, technologies));
			this.Players.Add(new PlayerProcessor(organellePlayer, new DevelopmentTopic[0]));
		}

		public TemporaryDB()
		{ }

		internal TemporaryDB Copy(PlayersRemap playersRemap)
		{
			var copy = new TemporaryDB();

			copy.Colonies = new ColonyProcessorCollection();
			copy.Colonies.Add(this.Colonies.Select(x => x.Copy(playersRemap)));

			copy.Stellarises = new StellarisProcessorCollection();
			copy.Stellarises.Add(this.Stellarises.Select(x => x.Copy(playersRemap)));

			copy.Players = new PlayerProcessorCollection();
			copy.Players.Add(this.Players.Select(x => x.Copy(playersRemap)));

			copy.Natives = this.Natives.Copy(playersRemap);

			return copy;
		}
		
		internal PlayerProcessor Of(Player player)
		{
			return this.Players.Of[player];
		}

		internal ColonyProcessor Of(Colony colony)
		{
			return this.Colonies.Of[colony];
		}

		internal StellarisProcessor Of(StellarisAdmin stellaris)
		{
			return this.Stellarises.Of[stellaris];
		}
	}
}
