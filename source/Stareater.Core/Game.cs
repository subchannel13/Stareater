using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Galaxy;
using Stareater.Galaxy.Builders;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;

namespace Stareater
{
	class Game
	{
		public Player[] Players { get; private set; }
		public int Turn { get; private set; }
		public int CurrentPlayer { get; private set; }
		private IEnumerable<object> conflicts; //TODO: make type
		private object phase; //TODO: make type

		internal StaticsDB Statics { get; private set; }
		internal StatesDB States { get; private set; }
			
		public Game(StaticsDB statics, IEnumerable<StarSystem> starSystems, IEnumerable<Tuple<int, int>> wormholeEndpoints, Player[] players)
		{
			this.Turn = 0;
			this.CurrentPlayer = 0;
			this.Statics = statics;
			
			StarData[] starList = starSystems.Select(x => x.Star).ToArray();
			
			var stars = new StarsCollection();
			stars.Add(starList);
			
			var wormholes = new WormholeCollection();
			wormholes.Add(wormholeEndpoints.Select(x => new Tuple<StarData, StarData>(starList[x.Item1], starList[x.Item2])));
			
			var planets = new PlanetsCollection();
			foreach(var system in starSystems)
				planets.Add(system.Planets);
			
			this.States = new StatesDB(stars, wormholes, planets);
			
			this.Players = players;
		}
	}
}
