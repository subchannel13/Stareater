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
			
			var techProgress = new TechProgressCollection();
			foreach(Player player in players)
				foreach(Technology tech in statics.Technologies)
					techProgress.Add(new TechnologyProgress(tech, player));
			
			this.States = new StatesDB(stars, wormholes, planets, techProgress);
			
			this.Players = players;
		}
		
		#region Technology related
		private int technologyOrderKey(TechnologyProgress tech)
		{
			if (tech.Owner.Orders.DevelopmentQueue.ContainsKey(tech.Topic.IdCode))
				return tech.Owner.Orders.DevelopmentQueue[tech.Topic.IdCode];
			
			if (tech.Order != TechnologyProgress.Unordered)
				return tech.Order;
			
			return int.MaxValue;
		}
		
		private int technologySort(TechnologyProgress leftTech, TechnologyProgress rightTech)
		{
			int primaryComparison = technologyOrderKey(leftTech).CompareTo(technologyOrderKey(rightTech));
			
			if (primaryComparison == 0)
				return leftTech.Topic.IdCode.CompareTo(rightTech.Topic.IdCode);
			
			return primaryComparison;
		}
		
		public IEnumerable<TechnologyProgress> AdvancmentOrder(Player player)
		{
			var playerTechs = States.TechnologyProgresses.Players(player).ToList();
			playerTechs.Sort(technologySort);
			
			return playerTechs;
		}
		#endregion
	}
}
