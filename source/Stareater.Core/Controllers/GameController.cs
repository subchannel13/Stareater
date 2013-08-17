﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Controllers.Data;
using Stareater.Galaxy;
using NGenerics.DataStructures.Mathematical;
using Stareater.Players;

namespace Stareater.Controllers
{
	public class GameController
	{
		private Game game;
		private Dictionary<Player, StarData> lastSelectedStar = new Dictionary<Player, StarData>();

		public GameController()
		{
			State = GameState.NoGame;
		}

		public void CreateGame(NewGameController controller)
		{
			Random rng = new Random();
			
			var starPositions = controller.StarPositioner.Generate(rng, controller.PlayerList.Count);
			
			Player[] players = controller.PlayerList.Select(info =>
				new Player(info.Name, info.Color, info.Organization, info.ControlType)
			).ToArray();

			this.game = new Game(controller.StarPopulator.Generate(rng, starPositions), controller.StarConnector.Generate(rng, starPositions), players);

			this.State = GameState.Running;

			// TODO: find most populated player's system
			foreach (var player in players)
				this.lastSelectedStar.Add(player, game.States.Stars.First());
		}

		public GameState State { get; private set; }

		public IEnumerable<StarData> Stars
		{
			get
			{
				return game.States.Stars;
			}
		}

		public IEnumerable<Tuple<StarData, StarData>> Wormholes
		{
			get
			{
				foreach (var wormhole in game.States.Wormholes)
					yield return wormhole;
			}
		}

		public int StarCount
		{
			get { return game.States.Stars.Count; }
		}

		public StarData SelectedStar
		{
			get
			{
				return this.lastSelectedStar[game.Players[game.CurrentPlayer]];
			}
			private set
			{
				this.lastSelectedStar[game.Players[game.CurrentPlayer]] = value;
			}
		}
		
		public void EndTurn()
		{
			//UNDONE: stub
		}
		
		private StarData closestStar(float x, float y, float searchRadius)
		{
			Vector2D point = new Vector2D(x, y);
			StarData closestStar = game.States.Stars.First();
			foreach (var star in game.States.Stars)
				if ((star.Position - point).Magnitude() < (closestStar.Position - point).Magnitude())
					closestStar = star;
			
			if ((closestStar.Position - point).Magnitude() <= searchRadius)
				return closestStar;
			else
				return null;
		}
		
		public void SelectClosest(float x, float y, float searchRadius)
		{
			StarData closest = closestStar(x, y, searchRadius);
			
			if (closest != null)
				SelectedStar = closest;
		}
		
		public StarSystemController OpenStarSystem(float x, float y, float searchRadius)
		{
			StarData closest = closestStar(x, y, searchRadius);
			
			if (closest != null)
				return new StarSystemController(game, closest);
			else
				return null;
		}
		
		public StarSystemController OpenStarSystem(StarData star)
		{
			return new StarSystemController(game, star);
		}
	}
}
