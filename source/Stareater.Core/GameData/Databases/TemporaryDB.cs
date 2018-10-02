using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.GameData.Databases.Tables;
using Stareater.GameLogic;
using Stareater.Players;
using Stareater.Galaxy;
using Stareater.Utils.StateEngine;

namespace Stareater.GameData.Databases
{
	class TemporaryDB
	{
		[StateProperty]
		public ColonyProcessorCollection Colonies { get; private set; }
		[StateProperty]
		public StellarisProcessorCollection Stellarises { get; private set; }
		[StateProperty]
		public PlayerProcessorCollection Players { get; private set; }
		[StateProperty]
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
		
		public PlayerProcessor this[Player player]
		{
			get
			{
				return this.Players.Of[player];
			}
		}

		public ColonyProcessor this[Colony colony]
		{
			get
			{
				return this.Colonies.Of[colony];
			}
		}

		public StellarisProcessor this[StellarisAdmin stellaris]
		{
			get
			{
				return this.Stellarises.Of[stellaris];
			}
		}
	}
}
