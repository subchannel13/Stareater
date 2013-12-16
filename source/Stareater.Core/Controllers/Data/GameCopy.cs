using Stareater.GameData;

namespace Stareater.Controllers.Data
{
	class GameCopy
	{
		public Game Game;
		public PlayersRemap Players;
		public GalaxyRemap Map;

		public GameCopy(Game game, PlayersRemap players, GalaxyRemap map) {
			this.Game = game;
			this.Players = players;
			this.Map = map;
		}
	}
}
