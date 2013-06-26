using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Players;
using Stareater.Galaxy;

namespace Stareater
{
	class Game
	{
		public Player[] Players { get; private set; }
		public Map GalaxyMap { get; private set; }
		public int Turn { get; private set; }
		public int CurrentPlayer { get; private set; }
		private IEnumerable<object> conflicts;
		private object phase;

		public Game(Map map, Player[] players)
		{
			this.GalaxyMap = map;
			this.Players = players;
			
			this.Turn = 0;
			this.CurrentPlayer = 0;
		}
	}
}
