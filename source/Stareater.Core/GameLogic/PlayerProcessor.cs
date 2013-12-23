using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;

namespace Stareater.GameLogic
{
	class PlayerProcessor
	{
		public const string LevelSufix = "Lvl";
		
		public Player Player { get; private set; }
		
		public PlayerProcessor(Player player, IEnumerable<Technology> technologies)
		{
			this.Player = player;
			
			this.TechLevels = new Dictionary<string, double>();
			foreach (var tech in technologies) {
				this.TechLevels.Add(tech.IdCode + LevelSufix, TechnologyProgress.NotStarted);
			}
		}

		public PlayerProcessor(Player player)
		{
			this.Player = player;
		}

		internal PlayerProcessor Copy(PlayersRemap playersRemap)
		{
			PlayerProcessor copy = new PlayerProcessor(playersRemap.Players[this.Player]);
			
			copy.TechLevels = new Dictionary<string, double>(this.TechLevels);

			return copy;
		}
		
		#region Technology related
		private int technologyOrderKey(TechnologyProgress tech)
		{
			var playersOrder = (tech.Topic.Category == TechnologyCategory.Development) ? 
				Player.Orders.DevelopmentQueue : 
				Player.Orders.ResearchQueue;
				
			if (playersOrder.ContainsKey(tech.Topic.IdCode))
				return playersOrder[tech.Topic.IdCode];
			
			return int.MaxValue;
		}
		
		private int technologySort(TechnologyProgress leftTech, TechnologyProgress rightTech)
		{
			int primaryComparison = technologyOrderKey(leftTech).CompareTo(technologyOrderKey(rightTech));
			
			if (primaryComparison == 0)
				return leftTech.Topic.IdCode.CompareTo(rightTech.Topic.IdCode);
			
			return primaryComparison;
		}
		
		public IEnumerable<TechnologyProgress> DevelopmentOrder(TechProgressCollection techAdvances)
		{
			var techLevels = techAdvances.Of(Player).ToDictionary(x => x.Topic.IdCode, x => x.Level);
			var playerTechs = techAdvances
				.Of(Player)
				.Where(x => (
					x.Topic.Category == TechnologyCategory.Development && 
					x.CanProgress(techLevels))
				).ToList();
			playerTechs.Sort(technologySort);
			
			return playerTechs;
		}
		
		public IEnumerable<TechnologyProgress> ResearchOrder(TechProgressCollection techAdvances)
		{
			var techLevels = techAdvances.Of(Player).ToDictionary(x => x.Topic.IdCode, x => x.Level);
			var playerTechs = techAdvances
				.Of(Player)
				.Where(x => (
					x.Topic.Category == TechnologyCategory.Research &&
					x.CanProgress(techLevels))
				).ToList();
			playerTechs.Sort(technologySort);
			
			return playerTechs;
		}
		#endregion
		
		#region Galaxy phase
		
		public IDictionary<string, double> TechLevels { get; private set; }

		public void Calculate(IEnumerable<TechnologyProgress> techAdvances)
		{
			foreach (var tech in techAdvances) {
				TechLevels[tech.Topic.IdCode + LevelSufix] = tech.Level;
			}
		}
		#endregion
		
		#region Precombat processing
		
		private double developmentPoints = 0;
		
		public void ProcessPrecombat(IList<ColonyProcessor> colonyProcessors)
		{
			foreach (var colonyProc in colonyProcessors)
				developmentPoints += colonyProc.Development;
		}
		#endregion
		
		public void ProcessPostcombat(StatesDB states)
		{
			var techLevels = states.TechnologyAdvances.Of(Player).ToDictionary(x => x.Topic.IdCode, x => x.Level);
			var advanceOrder = this.DevelopmentOrder(states.TechnologyAdvances);
			
			foreach(var tech in advanceOrder)
			{
				developmentPoints = tech.Invest(developmentPoints, techLevels);
			}
			
			var newTechLevels = states.TechnologyAdvances.Of(Player).ToDictionary(x => x.Topic.IdCode, x => x.Level);
			Player.Orders.Reset(
				new HashSet<string>(
					states.TechnologyAdvances
					.Where(x => x.CanProgress(newTechLevels))
					.Select(x => x.Topic.IdCode)
				),
				states.Colonies.OwnedBy(Player),
				states.Stellarises.OwnedBy(Player)
			);
		}
	}
}
