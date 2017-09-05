using Ikadn;
using Stareater.GameData.Databases;
using Stareater.GameData.Databases.Tables;
using Stareater.GameLogic;
using Stareater.Players;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using System.Collections.Generic;

namespace Stareater
{
	class MainGame
	{
		[StateProperty]
		public Player[] MainPlayers { get; private set; }
		[StateProperty]
		public Player StareaterOrganelles { get; private set; }
		[StateProperty]
		public int Turn { get; set; }

		[StateProperty]
		public StatesDB States { get; private set; }
		[StateProperty]
		public Dictionary<Player, PlayerOrders> Orders { get; private set; }
		[StateProperty(doSave: false)]
		public TemporaryDB Derivates { get; private set; }
		
		public StaticsDB Statics { get; private set; }

		public bool IsReadOnly { get; set; }
		public GameProcessor Processor { get; private set; }

		public MainGame(Player[] players, Player organellePlayer, StaticsDB statics, StatesDB states, TemporaryDB derivates)
		{
			this.Turn = 0;

			this.StareaterOrganelles = organellePlayer;
			this.MainPlayers = players;
			this.IsReadOnly = false;
			this.Statics = statics;
			this.States = states;
			this.Derivates = derivates;

			this.Processor = new GameProcessor(this);

			this.Orders = new Dictionary<Player, PlayerOrders>();
			foreach (var player in this.AllPlayers)
				this.Orders[player] = new PlayerOrders();
		}

		private MainGame()
		{ }
		
		public IEnumerable<Player> AllPlayers
		{
			get
			{
				foreach(var player in this.MainPlayers)
					yield return player;
				
				yield return this.StareaterOrganelles;
			}
		}

		public void Initialze(StaticsDB statics, TemporaryDB derivates)
		{
			this.Statics = statics;
			this.Derivates = derivates;
			this.Processor = new GameProcessor(this);
		}
		
		public MainGame ReadonlyCopy(StateManager stateManager)
		{
			var copy = stateManager.Copy(this);
			
			copy.IsReadOnly = true;
			copy.Statics = this.Statics;
			
			return copy;
		}

		//TODO(v0.7) leave or move to processor
		public void CalculateDerivedEffects()
		{
			Processor.CalculateBaseEffects();
			Processor.CalculateSpendings();
			Processor.CalculateDerivedEffects();
		}
		
		#region Saving
		internal IkadnBaseObject Save(StateManager stateManager)
		{
			var indexer = new ObjectIndexer();
			indexer.AddAll(Statics.Armors.Values);
			indexer.AddAll(Statics.Constructables);
			indexer.AddAll(Statics.DevelopmentTopics);
			indexer.AddAll(Statics.Hulls.Values);
			indexer.AddAll(Statics.IsDrives.Values);
			indexer.AddAll(Statics.MissionEquipment.Values);
			indexer.AddAll(Statics.PredeginedDesigns);
			indexer.AddAll(Statics.Reactors.Values);
			indexer.AddAll(Statics.ResearchTopics);
			indexer.AddAll(Statics.Sensors.Values);
			indexer.AddAll(Statics.Shields.Values);
			indexer.AddAll(Statics.SpecialEquipment.Values);
			indexer.AddAll(Statics.Thrusters.Values);
			indexer.AddAll(Statics.Traits.Values);

			//TODO(v0.7) is the method necessary?
			return stateManager.Save(this, indexer);
		}
		
		public const string SaveGameTag = "Game";
		#endregion
	}
}
