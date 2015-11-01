using System;
using System.Collections.Generic;
using System.Linq;

using NGenerics.Extensions;
using Stareater.Controllers.Views;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.GameData.Databases.Tables;
using Stareater.GameData.Ships;
using Stareater.Players;
using Stareater.Players.Reports;
using Stareater.Ships;
using Stareater.Utils.Collections;

namespace Stareater.GameLogic
{
	class PlayerProcessor
	{
		public const string LevelSufix = "Lvl";
		
		public IEnumerable<ScienceResult> DevelopmentPlan { get; protected set; }
		public IEnumerable<ScienceResult> ResearchPlan { get; protected set; }
		public StarData ResearchCenter { get; private set; }
		public Player Player { get; private set; }
		public Dictionary<Design, DesignStats> DesignStats { get; private set; }
		public Design ColonyShipDesign { get; private set; }
		public Design SystemColonizerDesign { get; private set; }
		
		public PlayerProcessor(Player player, IEnumerable<Technology> technologies)
		{
			this.Player = player;
			
			this.DevelopmentPlan = null;
			this.ResearchPlan = null;
			this.DesignStats = new Dictionary<Design, DesignStats>();
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
			var copy = new PlayerProcessor(playersRemap.Players[this.Player]);
			
			copy.DesignStats = new Dictionary<Design, DesignStats>(playersRemap.Designs.Keys.Where(x => x.Owner == this.Player).ToDictionary(x => x, x => this.DesignStats[x]));
			copy.DevelopmentPlan = (this.DevelopmentPlan != null) ? new List<ScienceResult>(this.DevelopmentPlan) : null;
			copy.ResearchPlan  = (this.ResearchPlan != null) ? new List<ScienceResult>(this.ResearchPlan) : null;
			copy.TechLevels = new Dictionary<string, double>(this.TechLevels);
			
			return copy;
		}
		
		#region Technology related
		private int technologyOrderKey(TechnologyProgress tech)
		{
			var playersOrder = Player.Orders.DevelopmentQueue;
				
			return playersOrder.ContainsKey(tech.Topic.IdCode) ? playersOrder[tech.Topic.IdCode] : int.MaxValue;
			
		}
		
		private int technologySort(TechnologyProgress leftTech, TechnologyProgress rightTech)
		{
			int primaryComparison = technologyOrderKey(leftTech).CompareTo(technologyOrderKey(rightTech));
			
			return primaryComparison == 0 ? 
				string.Compare(leftTech.Topic.IdCode, rightTech.Topic.IdCode, StringComparison.Ordinal) : 
				primaryComparison;
			
		}
		
		public void CalculateDevelopment(StaticsDB statics, StatesDB states, IList<ColonyProcessor> colonyProcessors)
		{
			double developmentPoints = 0;
			
			foreach (var colonyProc in colonyProcessors)
				developmentPoints += colonyProc.Development;
			
			var focus = statics.DevelopmentFocusOptions[Player.Orders.DevelopmentFocusIndex];
			var techLevels = states.TechnologyAdvances.Of(Player).ToDictionary(x => x.Topic.IdCode, x => x.Level);
			var advanceOrder = this.DevelopmentOrder(states.TechnologyAdvances).ToList();
			
			var results = new List<ScienceResult>();
			for (int i = 0; i < advanceOrder.Count && i < focus.Weights.Length; i++) {
				results.Add(advanceOrder[i].SimulateInvestment(
					developmentPoints * focus.Weights[i],
					techLevels
				));
			}
			
			this.DevelopmentPlan = results;
		}
		
		public void CalculateResearch(StaticsDB statics, StatesDB states, IList<ColonyProcessor> colonyProcessors)
		{
			var techLevels = states.TechnologyAdvances.Of(Player).ToDictionary(x => x.Topic.IdCode, x => x.Level);
			
			double researchPoints = 0;
			this.ResearchCenter = null;
			
			var researchCenters = colonyProcessors.GroupBy(x => x.Colony.Star);
			foreach (var center in researchCenters)
			{
				var centerVars = new Var("pop", center.Sum(x => x.Colony.Population))
					.UnionWith(techLevels);
				
				double localResearch = statics.PlayerFormulas.Research.Evaluate(centerVars.Get);
				if (localResearch > researchPoints) {
					researchPoints = localResearch;
					this.ResearchCenter = center.Key;
				}
			}
			
			var advanceOrder = this.ResearchOrder(states.TechnologyAdvances).ToList();
			string focused = Player.Orders.ResearchFocus;
			
			if (advanceOrder.Count > 0 && advanceOrder.All(x => x.Topic.IdCode != focused))
				focused = advanceOrder[0].Topic.IdCode;
			
			double focusWeight = statics.PlayerFormulas.FocusedResearchWeight;
			var results = new List<ScienceResult>();
			for (int i = 0; i < advanceOrder.Count; i++) {
				double weight = advanceOrder[i].Topic.IdCode == focused ? focusWeight : 1;
				weight /= advanceOrder.Count + focusWeight;
				
				results.Add(advanceOrder[i].SimulateInvestment(
					researchPoints * weight,
					techLevels
				));
			}
			
			this.ResearchPlan = results;
		}
		
		public void InvalidateDevelopment()
		{
			this.DevelopmentPlan = null;
		}
		
		public void InvalidateResearch()
		{
			this.ResearchPlan = null;
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

		public void ProcessPrecombat(StaticsDB statics, StatesDB states, TemporaryDB derivates)
		{
			this.CalculateDevelopment(statics, states, derivates.Colonies.OwnedBy(this.Player));

			foreach (var colonyProc in derivates.Colonies.OwnedBy(this.Player))
				colonyProc.ProcessPrecombat(states, derivates);

			foreach (var stellarisProc in derivates.Stellarises.OwnedBy(this.Player))
				stellarisProc.ProcessPrecombat(states, derivates);
			
			/*
			 * TODO(v0.5): Process stars
			 * - Calculate effects from colonies
			 * - Build
			 * - Perform migration
			 */
		}

		#endregion
		
		#region Postcombat processing
		public void ProcessPostcombat(StaticsDB statics, StatesDB states, TemporaryDB derivates)
		{
			foreach(var techProgress in this.DevelopmentPlan.Concat(this.ResearchPlan)) {
				techProgress.Item.Progress(techProgress);
				if (techProgress.CompletedCount > 0)
					states.Reports.Add(new TechnologyReport(techProgress));
			}
			this.Calculate(states.TechnologyAdvances.Of(Player));

			var newTechLevels = states.TechnologyAdvances.Of(Player).ToDictionary(x => x.Topic.IdCode, x => x.Level);
			var validTechs = new HashSet<string>(
					states.TechnologyAdvances
					.Where(x => x.CanProgress(newTechLevels))
					.Select(x => x.Topic.IdCode)
				);

			Player.Orders.DevelopmentQueue = updateTechQueue(Player.Orders.DevelopmentQueue, validTechs);

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
					Player.Orders.ConstructionPlans.Add(colony, new ConstructionOrders(PlayerOrders.DefaultSiteSpendingRatio));

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
					Player.Orders.ConstructionPlans.Add(stellaris, new ConstructionOrders(PlayerOrders.DefaultSiteSpendingRatio));
				
			UnlockPredefinedDesigns(statics, states);
		}

		public void Analyze(Design design, StaticsDB statics)
		{
			var hullVars = new Var(AComponentType.LevelKey, design.Hull.Level).Get;
			
			var reactorVars = new Var(AComponentType.LevelKey, design.Reactor.Level).
				And(AComponentType.SizeKey, design.Hull.TypeInfo.SizeReactor.Evaluate(hullVars)).Get;
			double shipPower = design.Reactor.TypeInfo.Power.Evaluate(reactorVars);
			
			double galaxySpeed = 0;
			if (design.IsDrive != null)
			{
				var driveVars = new Var(AComponentType.LevelKey, design.IsDrive.Level).
					And(AComponentType.SizeKey, design.Hull.TypeInfo.SizeIS.Evaluate(hullVars)).
					And("power", shipPower).Get; 
				
				galaxySpeed = design.IsDrive.TypeInfo.Speed.Evaluate(driveVars);
			}
			
			var shipVars = new Var();
			foreach(var special in statics.SpecialEquipment.Keys)
			{
				shipVars.And(special, 0);
				shipVars.And(special + "_lvl", 0); //TODO Make "_lvl" string constant
			}
			foreach(var special in design.SpecialEquipment)
			{
				shipVars[special.Key.TypeInfo.IdCode] = special.Value;
				shipVars[special.Key.TypeInfo.IdCode + "_lvl"] = special.Key.Level; //TODO Make string constant
			}
			var buildings = new Dictionary<string, double>();
			foreach(var colonyBuilding in statics.ShipFormulas.ColonizerBuildings)
			{
				double amount = colonyBuilding.Value.Evaluate(shipVars.Get);
				if (amount > 0)
					buildings.Add(colonyBuilding.Key, amount);
			}
			
			this.DesignStats.Add(
				design,
				new DesignStats(galaxySpeed,
				                statics.ShipFormulas.ColonizerPopulation.Evaluate(shipVars.Get),
				                buildings
				               )
			);
		}
		
		public void UnlockPredefinedDesigns(StaticsDB statics, StatesDB states)
		{
			var playerTechs = states.TechnologyAdvances.Of(Player);
			var techLevels = playerTechs.ToDictionary(x => x.Topic.IdCode, x => x.Level);
				
			foreach(var predefDesign in statics.PredeginedDesigns)
				if (!Player.UnlockedDesigns.Contains(predefDesign) && Prerequisite.AreSatisfied(predefDesign.Prerequisites(statics), 0, techLevels))
				{
					Player.UnlockedDesigns.Add(predefDesign);
					var design = makeDesign(statics, states, predefDesign, techLevels);
					states.Designs.Add(design);
				}
			
			//TODO(v0.5) skip step if theere is no update
			this.ColonyShipDesign = makeDesign(
				statics, states,
				statics.ColonyShipDesigns.Last(x => Prerequisite.AreSatisfied(x.Prerequisites(statics), 0, techLevels)),
				techLevels);
			this.SystemColonizerDesign = makeDesign(
				statics, states,
				statics.SystemColonizerDesigns.Last(x => Prerequisite.AreSatisfied(x.Prerequisites(statics), 0, techLevels)),
				techLevels);
		}
		
		private Design makeDesign(StaticsDB statics, StatesDB states, PredefinedDesign predefDesign, Dictionary<string, int> techLevels)
		{
			var armor = AComponentType.MakeBest(statics.Armors.Values, techLevels);
			var hull = statics.Hulls[predefDesign.HullCode].MakeHull(techLevels);
			var reactor = ReactorType.MakeBest(statics.Reactors.Values, techLevels, hull);
			var isDrive = predefDesign.HasIsDrive ? IsDriveType.MakeBest(statics.IsDrives.Values, techLevels, hull, ReactorType.PowerOf(reactor, hull)) : null;
			var sensor = AComponentType.MakeBest(statics.Sensors.Values, techLevels);
			var specials = predefDesign.SpecialEquipment.ToDictionary(
				x => statics.SpecialEquipment[x.Key].MakeItem(techLevels),
				x => x.Value
			);
			var thruster = AComponentType.MakeBest(statics.Thrusters.Values, techLevels);

			var design = new Design(
				states.MakeDesignId(), Player, predefDesign.Name, predefDesign.HullImageIndex,
			    armor, hull, isDrive, reactor, sensor, specials, thruster
			);
			this.Analyze(design, statics);
			
			return design;
		}
		#endregion
		
		private static Dictionary<string, int> updateTechQueue(IEnumerable<KeyValuePair<string, int>> queue, ICollection<string> validItems)
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
