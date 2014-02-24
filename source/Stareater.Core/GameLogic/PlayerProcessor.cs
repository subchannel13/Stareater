using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;
using Stareater.Galaxy;
using Stareater.Ships;

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
			foreach (var colonyProc in colonyProcessors) {
				developmentPoints += colonyProc.Development;
				colonyProc.ProcessPrecombat();
			}
			
			/*
			 * TODO: Process stars
			 * - Calculate effects from colonies
			 * - Build
			 * - Perform migration
			 */
		}
		#endregion
		
		public void ProcessPostcombat(StaticsDB statics, StatesDB states, TemporaryDB derivates)
		{
			var techLevels = states.TechnologyAdvances.Of(Player).ToDictionary(x => x.Topic.IdCode, x => x.Level);
			var advanceOrder = this.DevelopmentOrder(states.TechnologyAdvances);
			
			foreach(var tech in advanceOrder)
			{
				developmentPoints = tech.Invest(developmentPoints, techLevels);
			}
			this.Calculate(states.TechnologyAdvances.Of(Player));

			var newTechLevels = states.TechnologyAdvances.Of(Player).ToDictionary(x => x.Topic.IdCode, x => x.Level);
			var validTechs = new HashSet<string>(
					states.TechnologyAdvances
					.Where(x => x.CanProgress(newTechLevels))
					.Select(x => x.Topic.IdCode)
				);

			Player.Orders.DevelopmentQueue = updateTechQueue(Player.Orders.DevelopmentQueue, validTechs);
			Player.Orders.ResearchQueue = updateTechQueue(Player.Orders.ResearchQueue, validTechs);

			var oldPlans = Player.Orders.ConstructionPlans;
			Player.Orders.ConstructionPlans = new Dictionary<AConstructionSite, ConstructionOrders>();

			foreach (var colony in states.Colonies.OwnedBy(Player))
				if (oldPlans.ContainsKey(colony)) {
					var updatedPlans = updateConstructionPlans(
						statics,
						oldPlans[colony],
						derivates.Of(colony),
						this.TechLevels
					);

					Player.Orders.ConstructionPlans.Add(colony, updatedPlans);
				}
				else
					Player.Orders.ConstructionPlans.Add(colony, new ConstructionOrders(ChangesDB.DefaultSiteSpendingRatio));

			foreach (var stellaris in states.Stellarises.OwnedBy(Player))
				if (oldPlans.ContainsKey(stellaris)) {
					var updatedPlans = updateConstructionPlans(
						statics,
						oldPlans[stellaris],
						derivates.Of(stellaris),
						this.TechLevels
					);

					Player.Orders.ConstructionPlans.Add(stellaris, updatedPlans);
				}
				else
					Player.Orders.ConstructionPlans.Add(stellaris, new ConstructionOrders(ChangesDB.DefaultSiteSpendingRatio));
				
			UnlockPredefinedDesigns(statics, states);
		}

		public void UnlockPredefinedDesigns(StaticsDB statics, StatesDB states)
		{
			var playerTechs = states.TechnologyAdvances.Of(Player);
			var techLevels = playerTechs.ToDictionary(x => x.Topic.IdCode, x => x.Level);
				
			foreach(var predefDesign in statics.PredeginedDesigns)
				if (!Player.UnlockedDesigns.Contains(predefDesign) && Prerequisite.AreSatisfied(predefDesign.Prerequisites(statics), 0, techLevels))
				{
					Player.UnlockedDesigns.Add(predefDesign);
					states.Designs.Add(new Design(Player, predefDesign.Name, 
					                              statics.Hulls[predefDesign.HullCode].MakeHull(techLevels)
					                             ));
				}
					
		}
		
		private static IDictionary<string, int> updateTechQueue(IDictionary<string, int> queue, ISet<string> validItems)
		{
			var newOrder = queue
				.Where(x => validItems.Contains(x.Key))
				.OrderBy(x => x.Value)
				.Select(x => x.Key).ToArray();

			var newQueue = new Dictionary<string, int>();
			for (int i = 0; i < newOrder.Length; i++)
				newQueue.Add(newOrder[i], i);

			return newQueue;
		}

		private ConstructionOrders updateConstructionPlans(StaticsDB statics, ConstructionOrders oldOrders, AConstructionSiteProcessor processor, IDictionary<string, double> playersVars)
		{
			var newOrders = new ConstructionOrders(oldOrders.SpendingRatio);
			var vars = processor.LocalEffects(statics).UnionWith(playersVars).Get;

			foreach (var item in oldOrders.Queue)
				if (item.Condition.Evaluate(vars) >= 0)
					newOrders.Queue.Add(item);

			return newOrders;
		}
	}
}
