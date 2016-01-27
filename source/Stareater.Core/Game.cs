using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.GameLogic;
using Stareater.Players;
using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using Stareater.Controllers.Data;

namespace Stareater
{
	class Game
	{
		public Player[] Players { get; private set; }
		public int Turn { get; private set; }

		public StaticsDB Statics { get; private set; }
		public StatesDB States { get; private set; }
		public TemporaryDB Derivates { get; private set; }

		public GameProcessor Processor = null;
			
		public Game(Player[] players, StaticsDB statics, StatesDB states, TemporaryDB derivates)
		{
			this.Turn = 0;
			
			this.Players = players;
			this.Statics = statics;
			this.States = states;
			this.Derivates = derivates;

			this.Processor = new GameProcessor(this);
		}

		private Game()
		{ }
		
		public GameCopy ReadonlyCopy()
		{
			var copy = new Game();

			GalaxyRemap galaxyRemap = this.States.CopyGalaxy();
			PlayersRemap playersRemap = this.States.CopyPlayers(
				this.Players.ToDictionary(x => x, x => x.Copy(galaxyRemap)),
				galaxyRemap);

			foreach (var playerPair in playersRemap.Players)
				playerPair.Value.Orders = playerPair.Key.Orders.Copy(playersRemap, galaxyRemap);

			copy.Players = this.Players.Select(p => playersRemap.Players[p]).ToArray();
			copy.Turn = this.Turn;

			copy.Statics = this.Statics;
			copy.States = this.States.Copy(playersRemap, galaxyRemap);
			copy.Derivates = this.Derivates.Copy(playersRemap);

			return new GameCopy(copy, playersRemap, galaxyRemap);
		}

		public void CalculateDerivedEffects()
		{
			Processor.CalculateBaseEffects();
			Processor.CalculateSpendings();
			Processor.CalculateDerivedEffects();
		}

		public void ProcessPrecombat()
		{
			this.Processor.ProcessPrecombat();
		}
		
		public void ProcessPostcombat()
		{
			this.Processor.ProcessPostcombat();

			this.Turn++;
		}

		#region Saving
		public ObjectIndexer GenerateIndices()
		{
			var indexer = new ObjectIndexer();
			
			indexer.AddAll(this.Players);
			indexer.AddAll(Statics.PredeginedDesigns);
			
			indexer.AddAll(States.Designs);
			indexer.AddAll(States.TechnologyAdvances);
			
			indexer.AddAll(States.Planets);
			indexer.AddAll(States.Stars);
			indexer.AddAll(States.Wormholes);
			
			indexer.AddAll(States.Colonies);
			indexer.AddAll(States.Stellarises);
		
			return indexer;
		}
		
		internal IkonComposite Save()
		{
			ObjectIndexer indexer = this.GenerateIndices();
			
			var gameData = new IkonComposite(SaveGameTag);
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
