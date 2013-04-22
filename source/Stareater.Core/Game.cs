using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Players;
using Stareater.Maps;

namespace Stareater
{
	class Game
	{
		private Player[] Players;
		public Map map { get; private set; }
		private int turn;
		private int currentPlayer;
		private IEnumerable<object> conflicts;
		private object phase;

		public Game(Map map)
		{
			this.map = map;
		}
	}
}
