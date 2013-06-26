using System;
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
			var map = new Galaxy.Map(
				controller.StarPopulator.Generate(rng, starPositions),
				controller.StarConnector.Generate(rng, starPositions)
			);

			Player[] players = controller.PlayerList.Select(info =>
				new Player(info.Name, info.Color, info.Organization, info.ControlType)
			).ToArray();

			this.game = new Game(map, players);

			this.State = GameState.Running;

			// TODO: find most populated player's system
			foreach (var player in players)
				this.lastSelectedStar.Add(player, game.GalaxyMap.Stars[0]);
		}

		public GameState State { get; private set; }

		public IEnumerable<StarData> Stars
		{
			get
			{
				foreach (var star in game.GalaxyMap.Stars)
					yield return star;
			}
		}

		public IEnumerable<Tuple<StarData, StarData>> Wormholes
		{
			get
			{
				foreach (var wormhole in game.GalaxyMap.Wormholes)
					yield return wormhole;
			}
		}

		public StarData SelectedStar
		{
			get
			{
				return this.lastSelectedStar[game.Players[game.CurrentPlayer]];
			}
		}
	}
}
