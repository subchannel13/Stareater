using System.Collections.Generic;
using Stareater.GameData.Databases;
using Stareater.GameLogic;
using Stareater.Players;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using Ikadn;

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
		public ObjectIndexer GenerateIndices()
		{
			var indexer = new ObjectIndexer();
			
			indexer.AddAll(this.AllPlayers);
			indexer.AddAll(Statics.PredeginedDesigns);
			
			indexer.AddAll(States.Designs);
			indexer.AddAll(States.DevelopmentAdvances);
			
			indexer.AddAll(States.Planets);
			indexer.AddAll(States.Stars);
			indexer.AddAll(States.Wormholes);
			
			indexer.AddAll(States.Colonies);
			indexer.AddAll(States.Stellarises);
		
			return indexer;
		}
		
		internal IEnumerable<IkadnBaseObject> Save(StateManager stateManager)
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
		/*
			ObjectIndexer indexer = this.GenerateIndices();
			
			var gameData = new IkonComposite(SaveGameTag);
			var playersData = new IkonArray();
			var ordersData = new IkonArray();

			gameData.Add(TurnKey, new IkonInteger(this.Turn));

			gameData.Add(StatesKey, this.States.Save(indexer));

			foreach(var player in this.MainPlayers)
				playersData.Add(player.Save(indexer));
			gameData.Add(PlayersKey, playersData);
			gameData.Add(OrganellePlayerKey, this.StareaterOrganelles.Save(indexer));
			
			foreach(var player in this.MainPlayers)
				ordersData.Add(player.Orders.Save(indexer));
			gameData.Add(OrdersKey, ordersData);
			gameData.Add(OrganelleOrdersKey, this.StareaterOrganelles.Orders.Save(indexer));
			
			return gameData;*/
		}
		
		public const string SaveGameTag = "Game";
		public const string TurnKey = "turn";
		public const string OrdersKey = "orders";
		public const string OrganelleOrdersKey = "organelleOrders";
		public const string OrganellePlayerKey = "organelles";
		public const string PlayersKey = "players";
		public const string StatesKey = "states";
		#endregion
	}
}
