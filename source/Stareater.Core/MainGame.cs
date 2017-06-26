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
	class MainGame
	{
		public Player[] MainPlayers { get; private set; }
		public Player StareaterOrganelles { get; private set; }
		public int Turn { get; set; }

		public bool IsReadOnly { get; set; }
		public StaticsDB Statics { get; private set; }
		public StatesDB States { get; private set; }
		public TemporaryDB Derivates { get; private set; }

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
		
		public GameCopy ReadonlyCopy()
		{
			var copy = new MainGame();

			GalaxyRemap galaxyRemap = this.States.CopyGalaxy();
			PlayersRemap playersRemap = this.States.CopyPlayers(
				this.AllPlayers.ToDictionary(x => x, x => x.Copy(galaxyRemap)),
				galaxyRemap);

			foreach (var playerPair in playersRemap.Players)
				playerPair.Value.Orders = playerPair.Key.Orders.Copy(playersRemap, galaxyRemap);

			copy.MainPlayers = this.MainPlayers.Select(p => playersRemap.Players[p]).ToArray();
			copy.StareaterOrganelles = playersRemap.Players[this.StareaterOrganelles];
			copy.Turn = this.Turn;

			copy.IsReadOnly = true;
			copy.Statics = this.Statics;
			copy.States = this.States.Copy(playersRemap, galaxyRemap);
			copy.Derivates = this.Derivates.Copy(playersRemap);

			return new GameCopy(copy, playersRemap, galaxyRemap);
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
		
		internal IkonComposite Save()
		{
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
			
			return gameData;
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
