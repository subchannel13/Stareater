using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;

namespace Stareater.GameData.Databases
{
	internal class StatesDB
	{
		public StarsCollection Stars { get; private set; }
		public WormholeCollection Wormholes { get; private set; }
		public PlanetsCollection Planets { get; private set; }
		
		public TechProgressCollection TechnologyProgresses { get; private set; }
		
		public StatesDB(StarsCollection stars, WormholeCollection wormholes, PlanetsCollection planets, TechProgressCollection technologyProgresses)
		{
			this.Planets = planets;
			this.Stars = stars;
			this.Wormholes = wormholes;
			this.TechnologyProgresses = technologyProgresses;
		}
		
		private int technologySort(TechnologyProgress leftTech, TechnologyProgress rightTech)
		{
			if (leftTech.Order == rightTech.Order)
				return leftTech.Topic.IdCode.CompareTo(rightTech.Topic.IdCode);
			
			if (leftTech.Order == TechnologyProgress.Unordered && rightTech.Order != TechnologyProgress.Unordered)
				return 1;
			if (leftTech.Order != TechnologyProgress.Unordered && rightTech.Order == TechnologyProgress.Unordered)
				return -1;
			
			return leftTech.Order - rightTech.Order;
		}
		
		public IEnumerable<TechnologyProgress> AdvancmentOrder(Player player)
		{
			var playerTechs = TechnologyProgresses.Players(player).ToList();
			playerTechs.Sort(technologySort);
			
			return playerTechs;
		}
	}
}
