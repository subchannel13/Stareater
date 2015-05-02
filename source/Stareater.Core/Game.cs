using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Galaxy;
using Stareater.Galaxy.Builders;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.GameData.Databases.Tables;
using Stareater.GameLogic;
using Stareater.Players;
using Stareater.Utils;
using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using Stareater.Controllers.Data;
using Stareater.Ships.Missions;

namespace Stareater
{
	class Game
	{
		public Player[] Players { get; private set; }
		public int Turn { get; private set; }
		public int CurrentPlayerIndex { get; private set; } //FIXME(later): assumes single player, remove
		private IEnumerable<object> conflicts; //TODO(v0.5): make type

		public StaticsDB Statics { get; private set; }
		public StatesDB States { get; private set; }
		public TemporaryDB Derivates { get; private set; }

		private GameProcessor processor = null;
			
		public Game(Player[] players, StaticsDB statics, StatesDB states, TemporaryDB derivates)
		{
			this.Turn = 0;
			this.CurrentPlayerIndex = 0;
			
			this.Players = players;
			this.Statics = statics;
			this.States = states;
			this.Derivates = derivates;

			this.processor = new GameProcessor(this);
		}

		private Game()
		{ }

		public Player CurrentPlayer
		{ 
			get { return this.Players[CurrentPlayerIndex]; }
		}
		
		public GameCopy ReadonlyCopy()
		{
			Game copy = new Game();

			GalaxyRemap galaxyRemap = this.States.CopyGalaxy();
			PlayersRemap playersRemap = this.States.CopyPlayers(
				this.Players.ToDictionary(x => x, x => x.Copy(galaxyRemap)),
				galaxyRemap);

			foreach (var playerPair in playersRemap.Players)
				playerPair.Value.Orders = playerPair.Key.Orders.Copy(playersRemap);

			copy.Players = this.Players.Select(p => playersRemap.Players[p]).ToArray();
			copy.Turn = this.Turn;
			copy.CurrentPlayerIndex = this.CurrentPlayerIndex;

			copy.Statics = this.Statics;
			copy.States = this.States.Copy(playersRemap, galaxyRemap);
			copy.Derivates = this.Derivates.Copy(playersRemap);

			return new GameCopy(copy, playersRemap, galaxyRemap);
		}

		public void CalculateDerivedEffects()
		{
			processor.CalculateBaseEffects();
			processor.CalculateSpendings();
			processor.CalculateDerivedEffects();
		}

		public void ProcessPrecombat()
		{
			this.processor.ProcessPrecombat();
		}
		
		public void ProcessPostcombat()
		{
			this.processor.ProcessPostcombat();

			this.Turn++;
		}

		#region Saving
		public ObjectIndexer GenerateIndices()
		{
			ObjectIndexer indexer = new ObjectIndexer();
			
			indexer.AddAll(this.Players);
			indexer.AddAll(Statics.PredeginedDesigns);
			
			indexer.AddAll(States.Designs);
			indexer.AddAll(States.TechnologyAdvances);
			
			indexer.AddAll(States.Planets);
			indexer.AddAll(States.Stars);
			
			indexer.AddAll(States.Colonies);
			indexer.AddAll(States.Stellarises);
		
			return indexer;
		}
		
		internal IkonComposite Save()
		{
			ObjectIndexer indexer = this.GenerateIndices();
			
			IkonComposite gameData = new IkonComposite(SaveGameTag);
			var playersData = new IkonArray();
			var ordersData = new IkonArray();

			gameData.Add(TurnKey, new IkonInteger(this.Turn));

			gameData.Add(StatesKey, this.States.Save(indexer));

			foreach(var player in this.Players)
				playersData.Add(player.Save(indexer));
			gameData.Add(PlayersKey, playersData);
			
			foreach(var player in this.Players)
				ordersData.Add(player.Orders.Save(indexer));
			gameData.Add(OrdersKey, ordersData);
			
			return gameData;
		}
		
		public const string SaveGameTag = "Game";
		public const string TurnKey = "turn";
		public const string OrdersKey = "orders";
		public const string PlayersKey = "players";
		public const string StatesKey = "states";
		#endregion
	}
}
