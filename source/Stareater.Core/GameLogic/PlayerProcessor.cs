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
		
		public IEnumerable<DevelopmentResult> DevelopmentPlan { get; protected set; }
		public IEnumerable<ResearchResult> ResearchPlan { get; protected set; }
		public Player Player { get; private set; }
		public Dictionary<Design, DesignStats> DesignStats { get; private set; }
		public Design ColonyShipDesign { get; private set; }
		public Design SystemColonizerDesign { get; private set; }
		
		private Queue<ResearchResult> breakthroughs = new Queue<ResearchResult>();
		
		public PlayerProcessor(Player player, IEnumerable<DevelopmentTopic> technologies)
		{
			this.Player = player;
			
			this.DevelopmentPlan = null;
			this.ResearchPlan = null;
			this.DesignStats = new Dictionary<Design, DesignStats>();
			this.TechLevels = new Dictionary<string, double>();
			foreach (var tech in technologies) {
				this.TechLevels.Add(tech.IdCode + LevelSufix, DevelopmentProgress.NotStarted);
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
			copy.DevelopmentPlan = (this.DevelopmentPlan != null) ? new List<DevelopmentResult>(this.DevelopmentPlan) : null;
			copy.ResearchPlan  = (this.ResearchPlan != null) ? new List<ResearchResult>(this.ResearchPlan) : null;
			copy.TechLevels = new Dictionary<string, double>(this.TechLevels);
			
			return copy;
		}
		
		public void Initialize(StaticsDB statics, StatesDB states)
		{
			this.Calculate(states.DevelopmentAdvances.Of[this.Player]);
			this.unlockPredefinedDesigns(statics, states);
		}
		
		#region Technology related
		private int technologyOrderKey(DevelopmentProgress tech)
		{
			var playersOrder = Player.Orders.DevelopmentQueue;
				
			return playersOrder.ContainsKey(tech.Topic.IdCode) ? playersOrder[tech.Topic.IdCode] : int.MaxValue;
			
		}
		
		private int technologySort(DevelopmentProgress leftTech, DevelopmentProgress rightTech)
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
			var techLevels = states.DevelopmentAdvances.Of[Player].ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
			var advanceOrder = this.DevelopmentOrder(states.DevelopmentAdvances, states.ResearchAdvances, statics).ToList();
			
			var results = new List<DevelopmentResult>();
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
			var techLevels = states.DevelopmentAdvances.Of[Player].ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
			
			var advanceOrder = this.ResearchOrder(states.ResearchAdvances).ToList();
			string focused = Player.Orders.ResearchFocus;
			
			if (advanceOrder.Count > 0 && advanceOrder.All(x => x.Topic.IdCode != focused))
				focused = advanceOrder[0].Topic.IdCode;
			
			double focusWeight = statics.PlayerFormulas.FocusedResearchWeight;
			var results = new List<ResearchResult>();
			for (int i = 0; i < advanceOrder.Count; i++) {
				double weight = advanceOrder[i].Topic.IdCode == focused ? focusWeight : 1;
				weight /= advanceOrder.Count + focusWeight - 1;
				
				results.Add(advanceOrder[i].SimulateInvestment(weight));
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
		
		public IEnumerable<DevelopmentProgress> DevelopmentOrder(DevelopmentProgressCollection developmentAdvances, ResearchProgressCollection researchAdvances, StaticsDB statics)
		{
			var researchLevels = researchAdvances.Of[Player].ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
			var playerTechs = developmentAdvances
				.Of[Player]
				.Where(x => (x.CanProgress(researchLevels, statics)))
				.ToList();
			playerTechs.Sort(technologySort);
			
			return playerTechs;
		}
		
		public IEnumerable<ResearchProgress> ResearchOrder(ResearchProgressCollection techAdvances)
		{
			var playerTechs = techAdvances
				.Of[Player]
				.Where(x =>	x.CanProgress())
				.OrderBy(x => x.Topic.IdCode)
				.ToList();
			
			return playerTechs;
		}
		#endregion
		
		#region Galaxy phase
		
		public IDictionary<string, double> TechLevels { get; private set; }

		public void Calculate(IEnumerable<DevelopmentProgress> techAdvances)
		{
			foreach (var tech in techAdvances) {
				TechLevels[tech.Topic.IdCode + LevelSufix] = tech.Level;
			}
		}
		#endregion
		
		#region Precombat processing

		public void ProcessPrecombat(StaticsDB statics, StatesDB states, TemporaryDB derivates)
		{
			this.updateColonizationOrders(states);

			foreach (var colonyProc in derivates.Colonies.OwnedBy[this.Player])
				colonyProc.ProcessPrecombat(states, derivates);

			foreach (var stellarisProc in derivates.Stellarises.OwnedBy[this.Player])
				stellarisProc.ProcessPrecombat(states, derivates);
			
			this.breakthroughs = new Queue<ResearchResult>(this.ResearchPlan.Where(x => x.CompletedCount > 0));
			
			/*
			 * TODO(later): Process stars
			 * - Perform migration
			 */
		}

		private void updateColonizationOrders(StatesDB states)
		{
			foreach(var project in states.ColonizationProjects.OwnedBy[this.Player])
				if (!this.Player.Orders.ColonizationOrders.ContainsKey(project.Destination))
					states.ColonizationProjects.PendRemove(project);
			
			foreach(var order in this.Player.Orders.ColonizationOrders)
				if (states.ColonizationProjects.Of[order.Key].All(x => x.Owner != this.Player))
					states.ColonizationProjects.PendAdd(new ColonizationProject(this.Player, order.Value.Destination));
			
			states.ColonizationProjects.ApplyPending();
		}
		#endregion
		
		#region Postcombat processing
		public void BreakthroughReviewed(IList<string> selectedPriorities, StatesDB states)
		{
			for(int priority = 0; priority < selectedPriorities.Count; priority++)
			{
				var progress = states.DevelopmentAdvances.Of[this.Player].First(x => x.Topic.IdCode == selectedPriorities[priority]);
				progress.Priority = priority;
			}
		}
		
		public bool HasBreakthrough
		{
			get
			{
				return this.breakthroughs.Any();
			}
		}
		
		public ResearchResult NextBreakthrough()
		{
			return this.breakthroughs.Dequeue();
		}
		
		public void ProcessPostcombat(StaticsDB statics, StatesDB states, TemporaryDB derivates)
		{
			this.advanceTechnologies(states, statics);
			this.checkColonizationValidity(states);
			this.doConstruction(statics, states, derivates);
			this.unlockPredefinedDesigns(statics, states);
			this.updateDesigns(statics, states, derivates);
		}

		private void advanceTechnologies(StatesDB states, StaticsDB statics)
		{
			foreach(var techProgress in this.DevelopmentPlan) {
				techProgress.Item.Progress(techProgress);
				if (techProgress.CompletedCount > 0)
					states.Reports.Add(new DevelopmentReport(techProgress));
			}
			foreach(var techProgress in this.ResearchPlan) {
				techProgress.Item.Progress(techProgress);
				if (techProgress.CompletedCount > 0)
					states.Reports.Add(new ResearchReport(techProgress));
			}
			this.Calculate(states.DevelopmentAdvances.Of[Player]);

			var researchLevels = states.ResearchAdvances.Of[Player].ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
			var validTechs = new HashSet<string>(
					states.DevelopmentAdvances
					.Where(x => x.CanProgress(researchLevels, statics))
					.Select(x => x.Topic.IdCode)
				);

			Player.Orders.DevelopmentQueue = updateTechQueue(Player.Orders.DevelopmentQueue, validTechs);
		}

		private void checkColonizationValidity(StatesDB states)
		{
			var occupiedTargets = new HashSet<Planet>();
			foreach(var order in this.Player.Orders.ColonizationOrders)
				if (states.Colonies.AtPlanet.Contains(order.Key)) //TODO(later) use intelligence instead
					occupiedTargets.Add(order.Key);
			foreach(var planet in occupiedTargets)
				this.Player.Orders.ColonizationOrders.Remove(planet);
		}

		private void doConstruction(StaticsDB statics, StatesDB states, TemporaryDB derivates)
		{
			var oldPlans = Player.Orders.ConstructionPlans;
			Player.Orders.ConstructionPlans = new Dictionary<AConstructionSite, ConstructionOrders>();

			foreach (var colony in states.Colonies.OwnedBy[Player])
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

				foreach (var stellaris in states.Stellarises.OwnedBy[Player])
					if (oldPlans.ContainsKey(stellaris)) 
					{
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
		}
		
		private void updateDesigns(StaticsDB statics, StatesDB states, TemporaryDB derivates)
		{
			//Generate upgraded designs
			var upgradesTo = new Dictionary<Design, Design>();
			var newDesigns = new HashSet<Design>();
			foreach(var design in states.Designs.OwnedBy[this.Player])
			{
				var upgrade = this.DesignUpgrade(design, statics, states);
				if (states.Designs.Contains(upgrade))
					continue;
				
				if (newDesigns.Contains(upgrade))
					upgrade = newDesigns.First(x => x == upgrade);
				else
					this.Analyze(upgrade, statics);
				
				design.IsObsolete = true;
				upgradesTo[design] = upgrade;
				newDesigns.Add(upgrade);
			}
			states.Designs.Add(newDesigns);
			
			//Update refit orders to upgrade obsolete designs
			foreach(var upgrade in upgradesTo)
			{
				var orders = this.Player.Orders.RefitOrders;
				
				if (!orders.ContainsKey(upgrade.Key))
					orders[upgrade.Key] = upgrade.Value;
				else if (orders[upgrade.Key] != null && orders[upgrade.Key].IsObsolete)
					orders[upgrade.Key] = upgradesTo[orders[upgrade.Key]];
			}
			
			foreach(var site in this.Player.Orders.ConstructionPlans.Keys.ToList())
			{
				var updater = new ShipConstructionUpdater(this.Player.Orders.ConstructionPlans[site].Queue, this.Player.Orders.RefitOrders);
				this.Player.Orders.ConstructionPlans[site].Queue.Clear();
				this.Player.Orders.ConstructionPlans[site].Queue.AddRange(updater.Run());
			}

			//Removing inactive discarded designs
			var shipConstruction = new ShipConstructionCounter();
			shipConstruction.Check(derivates.Stellarises.OwnedBy[this.Player]);
			
			var activeDesigns = new HashSet<Design>(states.Fleets.
			                                        SelectMany(x => x.Ships).
			                                        Select(x => x.Design).
			                                        Concat(shipConstruction.Designs));
			var discardedDesigns = this.Player.Orders.RefitOrders.
				Where(x => x.Value == null && !activeDesigns.Contains(x.Key)).
				Select(x => x.Key).ToList();
			
			foreach(var design in discardedDesigns)
			{
				design.Owner.Orders.RefitOrders.Remove(design);
				states.Designs.Remove(design);
				this.DesignStats.Remove(design);
			}
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
			var techLevels = states.DevelopmentAdvances.Of[Player].ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
			
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
		
		private void unlockPredefinedDesigns(StaticsDB statics, StatesDB states)
		{
			var playerTechs = states.DevelopmentAdvances.Of[Player];
			var techLevels = playerTechs.ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
				
			foreach(var predefDesign in statics.PredeginedDesigns)
				if (!Player.UnlockedDesigns.Contains(predefDesign) && Prerequisite.AreSatisfied(predefDesign.Prerequisites(statics), 0, techLevels))
				{
					Player.UnlockedDesigns.Add(predefDesign);
					makeDesign(statics, states, predefDesign, techLevels, false);
				}
			
			this.ColonyShipDesign = updateVirtualDesign(this.ColonyShipDesign, statics, states, statics.ColonyShipDesigns, techLevels);
			this.SystemColonizerDesign = updateVirtualDesign(this.SystemColonizerDesign, statics, states, statics.SystemColonizerDesigns, techLevels);
		}
		
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

		private Design updateVirtualDesign(Design oldDesign, StaticsDB statics, StatesDB states, IEnumerable<PredefinedDesign> predefDesigns, Dictionary<string, double> techLevels)
		{
			var newDesign = makeDesign(
				statics, states,
				predefDesigns.Last(x => Prerequisite.AreSatisfied(x.Prerequisites(statics), 0, techLevels)),
				techLevels, true);
			
			if (newDesign != oldDesign && oldDesign != null)
			{
				oldDesign.IsObsolete = true;
				oldDesign.Owner.Orders.RefitOrders[oldDesign] = null;
			}
			
			return newDesign;
		}
	}
}
