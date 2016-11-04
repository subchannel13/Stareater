using Stareater.GameData;

namespace Stareater.Controllers.Data 
{
	//TODO(later) investigate if this class is needed at all
	class GameCopy 
	{
		public MainGame Game;
		public PlayersRemap Players;
		public GalaxyRemap Map;

		public GameCopy(MainGame game, PlayersRemap players, GalaxyRemap map) 
		{
			this.Game = game;
			this.Players = players;
			this.Map = map;
		}
	}
}
