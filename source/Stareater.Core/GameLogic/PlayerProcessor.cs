using System;
using System.Collections.Generic;
using System.Linq;

using Stareater.Controllers.Views;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.GameData.Databases.Tables;
using Stareater.GameData.Ships;
using Stareater.Players;
using Stareater.Players.Reports;
using Stareater.Ships;
using Stareater.Utils;
using Stareater.Utils.Collections;

namespace Stareater.GameLogic
{
	class PlayerProcessor
	{
		public const string LevelSufix = "Lvl";
		
		public IEnumerable<ScienceResult> DevelopmentPlan { get; protected set; }
		public IEnumerable<ScienceResult> ResearchPlan { get; protected set; }
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
			var techLevels = states.TechnologyAdvances.Of(Player).ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
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
			var techLevels = states.TechnologyAdvances.Of(Player).ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
			
			var advanceOrder = this.ResearchOrder(states.TechnologyAdvances).ToList();
			string focused = Player.Orders.ResearchFocus;
			
			if (advanceOrder.Count > 0 && advanceOrder.All(x => x.Topic.IdCode != focused))
				focused = advanceOrder[0].Topic.IdCode;
			
			double focusWeight = statics.PlayerFormulas.FocusedResearchWeight;
			var results = new List<ScienceResult>();
			for (int i = 0; i < advanceOrder.Count; i++) {
				double weight = advanceOrder[i].Topic.IdCode == focused ? focusWeight : 1;
				weight /= advanceOrder.Count + focusWeight - 1;
				
				results.Add(advanceOrder[i].SimulateInvestment(
					weight,
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
			var techLevels = techAdvances.Of(Player).ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
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
			var techLevels = techAdvances.Of(Player).ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
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
			this.updateColonizationOrders(states);

			foreach (var colonyProc in derivates.Colonies.OwnedBy(this.Player))
				colonyProc.ProcessPrecombat(states, derivates);

			foreach (var stellarisProc in derivates.Stellarises.OwnedBy(this.Player))
				stellarisProc.ProcessPrecombat(states, derivates);
			
			/*
			 * TODO(later): Process stars
			 * - Perform migration
			 */
		}

		private void updateColonizationOrders(StatesDB states)
		{
			foreach(var project in states.ColonizationProjects.OwnedBy(this.Player))
				if (!this.Player.Orders.ColonizationOrders.ContainsKey(project.Destination))
					states.ColonizationProjects.PendRemove(project);
			
			foreach(var order in this.Player.Orders.ColonizationOrders)
				if (states.ColonizationProjects.Of(order.Key).All(x => x.Owner != this.Player))
					states.ColonizationProjects.PendAdd(new ColonizationProject(this.Player, order.Value.Destination));
			
			states.ColonizationProjects.ApplyPending();
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

			var newTechLevels = states.TechnologyAdvances.Of(Player).ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
			var validTechs = new HashSet<string>(
					states.TechnologyAdvances
					.Where(x => x.CanProgress(newTechLevels))
					.Select(x => x.Topic.IdCode)
				);

			Player.Orders.DevelopmentQueue = updateTechQueue(Player.Orders.DevelopmentQueue, validTechs);
			
			var occupiedTargets = new HashSet<Planet>();
			foreach(var order in this.Player.Orders.ColonizationOrders)
				if (states.Colonies.AtPlanetContains(order.Key)) //TODO(later) use intelligence instead
					occupiedTargets.Add(order.Key);
			foreach(var planet in occupiedTargets)
				this.Player.Orders.ColonizationOrders.Remove(planet);

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
		
		public void UnlockPredefinedDesigns(StaticsDB statics, StatesDB states)
		{
			var playerTechs = states.TechnologyAdvances.Of(Player);
			var techLevels = playerTechs.ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
				
			foreach(var predefDesign in statics.PredeginedDesigns)
				if (!Player.UnlockedDesigns.Contains(predefDesign) && Prerequisite.AreSatisfied(predefDesign.Prerequisites(statics), 0, techLevels))
				{
					Player.UnlockedDesigns.Add(predefDesign);
					makeDesign(statics, states, predefDesign, techLevels, false);
				}
			
			this.ColonyShipDesign = makeDesign(
				statics, states,
				statics.ColonyShipDesigns.Last(x => Prerequisite.AreSatisfied(x.Prerequisites(statics), 0, techLevels)),
				techLevels, true);
			this.SystemColonizerDesign = makeDesign(
				statics, states,
				statics.SystemColonizerDesigns.Last(x => Prerequisite.AreSatisfied(x.Prerequisites(statics), 0, techLevels)),
				techLevels, true);
		}
		
		private Design makeDesign(StaticsDB statics, StatesDB states, PredefinedDesign predefDesign, Dictionary<string, double> techLevels, bool isVirtual)
		{
			var hull = statics.Hulls[predefDesign.HullCode].MakeHull(techLevels);
			var specials = predefDesign.SpecialEquipment.OrderBy(x => x.Key).Select(
				x => statics.SpecialEquipment[x.Key].MakeBest(techLevels, x.Value)
			).ToList();
			
			var armor = AComponentType.MakeBest(statics.Armors.Values, techLevels);
			var reactor = ReactorType.MakeBest(techLevels, hull, specials, statics);
			var isDrive = predefDesign.HasIsDrive ? IsDriveType.MakeBest(techLevels, hull, reactor, specials, statics) : null;
			var sensor = AComponentType.MakeBest(statics.Sensors.Values, techLevels);
			var shield = predefDesign.ShieldCode != null ? statics.Shields[predefDesign.ShieldCode].MakeBest(techLevels) : null;
			var equipment = predefDesign.MissionEquipment.Select(
				x => statics.MissionEquipment[x.Key].MakeBest(techLevels, x.Value)
			).ToList();
			
			var thruster = AComponentType.MakeBest(statics.Thrusters.Values, techLevels);

			var design = new Design(
				states.MakeDesignId(), Player, false, isVirtual, predefDesign.Name, predefDesign.HullImageIndex,
			    armor, hull, isDrive, reactor, sensor, shield, equipment, specials, thruster
			);
			design.CalcHash(statics);
			
			if (!states.Designs.Contains(design))
			{
				states.Designs.Add(design);
				this.Analyze(design, statics);
				return design;
			}
			
			return states.Designs.First(x => x == design);
		}
		#endregion
		
		#region Design stats
		public static Var DesignBaseVars(Component<HullType> hull, IEnumerable<Component<SpecialEquipmentType>> specialEquipment, StaticsDB statics)
		{
			var hullVars = new Var(AComponentType.LevelKey, hull.Level).Get;
			
			var shipVars = new Var("hullHp", hull.TypeInfo.ArmorBase.Evaluate(hullVars)).
				And("hullSensor", hull.TypeInfo.SensorsBase.Evaluate(hullVars)).
				And("hullCloak", hull.TypeInfo.CloakingBase.Evaluate(hullVars)).
				And("hullJamming", hull.TypeInfo.JammingBase.Evaluate(hullVars)).
				And("hullInertia", hull.TypeInfo.InertiaBase.Evaluate(hullVars)).
				And(HullType.IsDriveSizeKey, hull.TypeInfo.SizeIS.Evaluate(hullVars)).
				And(HullType.ReactorSizeKey, hull.TypeInfo.SizeReactor.Evaluate(hullVars)).
				Init(statics.SpecialEquipment.Keys, 0).
				Init(statics.SpecialEquipment.Keys.Select(x => x + AComponentType.LevelSuffix), 0).
				UnionWith(specialEquipment, x => x.TypeInfo.IdCode, x => x.Quantity).
				UnionWith(specialEquipment, x => x.TypeInfo.IdCode + AComponentType.LevelSuffix, x => x.Level);
			
			return shipVars;
		}
		
		public static Var DesignPoweredVars(Component<HullType> hull, Component<ReactorType> reactor, IEnumerable<Component<SpecialEquipmentType>> specialEquipment, StaticsDB statics)
		{
			var shipVars = DesignBaseVars(hull, specialEquipment, statics);
			
			shipVars[AComponentType.LevelKey] = reactor.Level;
			shipVars[ReactorType.TotalPowerKey] = reactor.TypeInfo.Power.Evaluate(shipVars.Get);
			
			return shipVars;
		}

		public Design DesignUpgrade(Design oldDesign, StaticsDB statics, StatesDB states)
		{
			var techLevels = states.TechnologyAdvances.Of(Player).ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
			
			var hull = statics.Hulls[oldDesign.Hull.TypeInfo.IdCode].MakeHull(techLevels);
			var specials = oldDesign.SpecialEquipment.Select(
				x => statics.SpecialEquipment[x.TypeInfo.IdCode].MakeBest(techLevels, x.Quantity)
			).ToList();
			
			var armor = AComponentType.MakeBest(statics.Armors.Values, techLevels);
			var reactor = ReactorType.MakeBest(techLevels, hull, specials, statics);
			var isDrive = oldDesign.IsDrive != null ? IsDriveType.MakeBest(techLevels, hull, reactor, specials, statics) : null;
			var sensor = AComponentType.MakeBest(statics.Sensors.Values, techLevels);
			var shield = oldDesign.Shield != null ? statics.Shields[oldDesign.Shield.TypeInfo.IdCode].MakeBest(techLevels) : null;
			var equipment = oldDesign.MissionEquipment.Select(
				x => statics.MissionEquipment[x.TypeInfo.IdCode].MakeBest(techLevels, x.Quantity)
			).ToList();
			
			var thruster = AComponentType.MakeBest(statics.Thrusters.Values, techLevels);

			var design = new Design(
				states.MakeDesignId(), Player, false, oldDesign.IsVirtual, oldDesign.Name, oldDesign.ImageIndex,
			    armor, hull, isDrive, reactor, sensor, shield, equipment, specials, thruster
			);
			design.CalcHash(statics);
			
			return design;
		}
		
		public void Analyze(Design design, StaticsDB statics)
		{
			var shipVars = DesignPoweredVars(design.Hull, design.Reactor, design.SpecialEquipment, statics);
			var hullVars = new Var(AComponentType.LevelKey, design.Hull.Level).Get;
			var armorVars = new Var(AComponentType.LevelKey, design.Armor.Level).Get;
			var thrusterVars = new Var(AComponentType.LevelKey, design.Thrusters.Level).Get;
			var sensorVars = new Var(AComponentType.LevelKey, design.Sensors.Level).Get;
			
			shipVars.And("armorFactor", design.Armor.TypeInfo.ArmorFactor.Evaluate(armorVars)).
				And("baseEvasion", design.Thrusters.TypeInfo.Evasion.Evaluate(thrusterVars)).
				And("thrust", design.Thrusters.TypeInfo.Speed.Evaluate(thrusterVars)).
				And("sensor", design.Sensors.TypeInfo.Detection.Evaluate(sensorVars));
			
			double galaxySpeed = 0;
			if (design.IsDrive != null)
			{
				shipVars[AComponentType.LevelKey] = design.IsDrive.Level;
				galaxySpeed = design.IsDrive.TypeInfo.Speed.Evaluate(shipVars.Get);
			}
			
			var buildings = new Dictionary<string, double>();
			foreach(var colonyBuilding in statics.ShipFormulas.ColonizerBuildings)
			{
				double amount = colonyBuilding.Value.Evaluate(shipVars.Get);
				if (amount > 0)
					buildings.Add(colonyBuilding.Key, amount);
			}
			
			shipVars[AComponentType.LevelKey] = design.Armor.Level;
			double baseArmorReduction = design.Armor.TypeInfo.Absorption.Evaluate(shipVars.Get);
			double hullArFactor = design.Hull.TypeInfo.ArmorAbsorption.Evaluate(shipVars.Get);
			double maxArmorReduction = design.Armor.TypeInfo.AbsorptionMax.Evaluate(shipVars.Get);
				
			double shieldCloaking = 0;
			double shieldJamming = 0;
			double shieldHp = 0;
			double shieldReduction = 0;
			double shieldRegeneration = 0;
			double shieldThickness = 0;
			double shieldPower = 0;
			if (design.Shield != null)
			{
				shipVars[AComponentType.LevelKey] = design.Shield.Level;
				shipVars[AComponentType.SizeKey] = design.Hull.TypeInfo.Size.Evaluate(hullVars);
				var hullShieldHp = design.Hull.TypeInfo.ShieldBase.Evaluate(hullVars);
				
				shieldCloaking = design.Shield.TypeInfo.Cloaking.Evaluate(shipVars.Get) * hullShieldHp;
				shieldJamming = design.Shield.TypeInfo.Jamming.Evaluate(shipVars.Get) * hullShieldHp;
				shieldHp = design.Shield.TypeInfo.HpFactor.Evaluate(shipVars.Get) * hullShieldHp;
				shieldReduction = design.Shield.TypeInfo.Reduction.Evaluate(shipVars.Get);
				shieldRegeneration = design.Shield.TypeInfo.RegenerationFactor.Evaluate(shipVars.Get) * hullShieldHp;
				shieldThickness = design.Shield.TypeInfo.Thickness.Evaluate(shipVars.Get);
				shieldPower = design.Shield.TypeInfo.PowerUsage.Evaluate(shipVars.Get);
			}
			shipVars.And("shieldCloak", shieldCloaking);
			shipVars.And("shieldJamming", shieldJamming);
			
			var abilities = new List<AbilityStats>(design.MissionEquipment.SelectMany(
				equip => equip.TypeInfo.Abilities.Select(
					x => AbilityStatsFactory.Create(x, equip.Level, equip.Quantity)
				)
			));
			
			this.DesignStats.Add(
				design,
				new DesignStats(
					galaxySpeed,
					shipVars[ReactorType.TotalPowerKey],
					statics.ShipFormulas.CombatSpeed.Evaluate(shipVars.Get),
					shipVars[ReactorType.TotalPowerKey] - shieldPower,
					abilities,
	                statics.ShipFormulas.ColonizerPopulation.Evaluate(shipVars.Get),
	                buildings,
	                statics.ShipFormulas.HitPoints.Evaluate(shipVars.Get),
	                shieldHp,
	                statics.ShipFormulas.Evasion.Evaluate(shipVars.Get),
	                Methods.Clamp(baseArmorReduction * hullArFactor, 0, maxArmorReduction),
	                shieldReduction,
	                shieldRegeneration,
	                shieldThickness,
	                statics.ShipFormulas.Detection.Evaluate(shipVars.Get),
	                statics.ShipFormulas.Cloaking.Evaluate(shipVars.Get),
	                statics.ShipFormulas.Jamming.Evaluate(shipVars.Get)
	            )
			);
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
