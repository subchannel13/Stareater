using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Controllers.Data;
using Stareater.Galaxy;

namespace Stareater.Controllers
{
	public class GameController
	{
		private Game game;

		public GameState State { get; private set; }

		public GameController()
		{
			State = GameState.NoGame;
		}

		public void CreateGame(NewGameController controller)
		{
			Random rng = new Random();
			
			var starPositions = controller.StarPositioner.Generate(rng, controller.PlayerList.Count);

			game = new Game(new Galaxy.Map(
				controller.StarPopulator.Generate(rng, starPositions), 
				controller.StarConnector.Generate(rng, starPositions))
				);

			State = GameState.Running;
		}

		public IEnumerable<StarData> Stars
		{
			get
			{
				foreach (var star in game.map.Stars)
					yield return star;
			}
		}

		public IEnumerable<Tuple<StarData, StarData>> Wormholes
		{
			get
			{
				foreach (var wormhole in game.map.Wormholes)
					yield return wormhole;
			}
		}
	}
}
