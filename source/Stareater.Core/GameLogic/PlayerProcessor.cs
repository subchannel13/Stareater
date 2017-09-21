using System;
using System.Collections.Generic;
using System.Linq;

using Stareater.AppData.Expressions;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.GameData.Databases.Tables;
using Stareater.GameData.Ships;
using Stareater.Players;
using Stareater.Players.Reports;
using Stareater.Ships;
using Stareater.Ships.Missions;
using Stareater.Utils;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;

namespace Stareater.GameLogic
{
	class PlayerProcessor
	{
		public const string LevelSufix = "Lvl";
		
		[StateProperty]
		public IEnumerable<DevelopmentResult> DevelopmentPlan { get; protected set; }
		[StateProperty]
		public IEnumerable<ResearchResult> ResearchPlan { get; protected set; }
		[StateProperty]
		public Player Player { get; private set; }
		[StateProperty]
		public Dictionary<Design, DesignStats> DesignStats { get; private set; }
		[StateProperty]
		public Dictionary<Design, Dictionary<Design, double>> RefitCosts { get; private set; }
		[StateProperty]
		public Design ColonyShipDesign { get; private set; }
		[StateProperty]
		public Design SystemColonizerDesign { get; private set; }

		private Queue<ResearchResult> breakthroughs = new Queue<ResearchResult>();
		
		public PlayerProcessor(Player player, IEnumerable<DevelopmentTopic> technologies)
		{
			this.Player = player;
			
			this.DevelopmentPlan = null;
			this.ResearchPlan = null;
			this.DesignStats = new Dictionary<Design, DesignStats>();
			this.RefitCosts = new Dictionary<Design, Dictionary<Design, double>>();
			this.TechLevels = new Dictionary<string, double>();

            foreach (var tech in technologies)
				this.TechLevels.Add(tech.IdCode + LevelSufix, DevelopmentProgress.NotStarted);
		}

		public PlayerProcessor(Player player)
		{
			this.Player = player;
		}

        private PlayerProcessor()
        { }

        internal PlayerProcessor Copy(PlayersRemap playersRemap)
		{
			var copy = new PlayerProcessor(playersRemap.Players[this.Player]);
			
			copy.DesignStats = this.DesignStats.ToDictionary(x => playersRemap.Designs[x.Key], x => x.Value);
			copy.DevelopmentPlan = (this.DevelopmentPlan != null) ? new List<DevelopmentResult>(this.DevelopmentPlan) : null;
			copy.RefitCosts = this.RefitCosts.ToDictionary(x => playersRemap.Designs[x.Key], x => x.Value.ToDictionary(y => playersRemap.Designs[y.Key], y => y.Value));
			copy.ResearchPlan  = (this.ResearchPlan != null) ? new List<ResearchResult>(this.ResearchPlan) : null;
			copy.TechLevels = new Dictionary<string, double>(this.TechLevels);
			
			return copy;
		}
		
		public void Initialize(MainGame game)
		{
			this.Calculate(game.States.DevelopmentAdvances.Of[this.Player]);
			this.unlockPredefinedDesigns(game);
		}
		
		#region Technology related
		public void CalculateDevelopment(MainGame game, IList<ColonyProcessor> colonyProcessors)
		{
			double developmentPoints = 0;
			
			foreach (var colonyProc in colonyProcessors)
				developmentPoints += colonyProc.Development;
			
			var focus = game.Statics.DevelopmentFocusOptions[game.Orders[Player].DevelopmentFocusIndex];
			var techLevels = game.States.DevelopmentAdvances.Of[Player].ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
			var advanceOrder = this.DevelopmentOrder(game.States.DevelopmentAdvances, game.States.ResearchAdvances, game).ToList();
			
			var results = new List<DevelopmentResult>();
			for (int i = 0; i < advanceOrder.Count && i < focus.Weights.Length; i++) {
				results.Add(advanceOrder[i].SimulateInvestment(
					developmentPoints * focus.Weights[i],
					techLevels
				));
			}
			
			this.DevelopmentPlan = results;
		}
		
		public void CalculateResearch(MainGame game, IList<ColonyProcessor> colonyProcessors)
		{
			var advanceOrder = this.ResearchOrder(game.States.ResearchAdvances).ToList();
			string focused = game.Orders[Player].ResearchFocus;
			
			if (advanceOrder.Count > 0 && advanceOrder.All(x => x.Topic.IdCode != focused))
				focused = advanceOrder[0].Topic.IdCode;
			
			double focusWeight = game.Statics.PlayerFormulas.FocusedResearchWeight;
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
		
		public IEnumerable<DevelopmentProgress> DevelopmentOrder(DevelopmentProgressCollection developmentAdvances, ResearchProgressCollection researchAdvances, MainGame game)
		{
			var researchLevels = researchAdvances.Of[Player].ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
			var playerTechs = developmentAdvances
				.Of[Player]
				.Where(x => (x.CanProgress(researchLevels, game.Statics)))
				.ToList();
			var orders = game.Orders[this.Player].DevelopmentQueue;

			var orderedTech = playerTechs.
				Where(x => orders.ContainsKey(x.Topic.IdCode)).
				OrderBy(x => orders[x.Topic.IdCode]).
				ToList();
			orderedTech.AddRange(
				playerTechs.
				Where(x => !orders.ContainsKey(x.Topic.IdCode)).
				OrderBy(x => x.Topic.IdCode)
            );
			
			return orderedTech;
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
		[StateProperty]
		public IDictionary<string, double> TechLevels { get; private set; }

		public void Calculate(IEnumerable<DevelopmentProgress> techAdvances)
		{
			foreach (var tech in techAdvances) {
				TechLevels[tech.Topic.IdCode + LevelSufix] = tech.Level;
			}
		}
		#endregion
		
		#region Precombat processing
		public void ProcessPrecombat(MainGame game)
		{
			this.updateColonizationOrders(game);

			foreach (var colonyProc in game.Derivates.Colonies.OwnedBy[this.Player])
				colonyProc.ProcessPrecombat(game.States, game.Derivates);

			foreach (var stellarisProc in game.Derivates.Stellarises.OwnedBy[this.Player])
				stellarisProc.ProcessPrecombat(game.States, game.Derivates);
			
			this.breakthroughs = new Queue<ResearchResult>(this.ResearchPlan.Where(x => x.CompletedCount > 0));
			
			/*
			 * TODO(later)
			 * - Perform migration
			 */
		}
		
		public void SpawnShip(StarData star, Design design, long quantity, IEnumerable<AMission> missions, StatesDB states)
		{
			var missionList = new LinkedList<AMission>(missions);
			var fleet = states.Fleets.At[star.Position].FirstOrDefault(x => x.Owner == this.Player && x.Missions.SequenceEqual(missionList));
			
			if (fleet == null)
			{
				fleet = new Fleet(this.Player, star.Position, missionList);
				states.Fleets.Add(fleet);
			}

			if (fleet.Ships.WithDesign.Contains(design))
				fleet.Ships.WithDesign[design].Quantity += quantity;
			else
				fleet.Ships.Add(new ShipGroup(design, quantity, 0, 0));
		}

		private void updateColonizationOrders(MainGame game)
		{
			foreach(var project in game.States.ColonizationProjects.OwnedBy[this.Player])
				if (!game.Orders[this.Player].ColonizationOrders.ContainsKey(project.Destination))
					game.States.ColonizationProjects.PendRemove(project);
			
			foreach(var order in game.Orders[this.Player].ColonizationOrders)
				if (game.States.ColonizationProjects.Of[order.Key].All(x => x.Owner != this.Player))
					game.States.ColonizationProjects.PendAdd(new ColonizationProject(this.Player, order.Value.Destination));

			game.States.ColonizationProjects.ApplyPending();
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
		
		public void ProcessPostcombat(MainGame game)
		{
			this.advanceTechnologies(game);
			this.checkColonizationValidity(game);
			this.doConstruction(game);
			this.unlockPredefinedDesigns(game);
			this.updateDesigns(game);
		}

		private void advanceTechnologies(MainGame game)
		{
			foreach(var techProgress in this.DevelopmentPlan) {
				techProgress.Item.Progress(techProgress);
				if (techProgress.CompletedCount > 0)
					game.States.Reports.Add(new DevelopmentReport(techProgress));
			}
			foreach(var techProgress in this.ResearchPlan) {
				techProgress.Item.Progress(techProgress);
				if (techProgress.CompletedCount > 0)
					game.States.Reports.Add(new ResearchReport(techProgress));
			}
			this.Calculate(game.States.DevelopmentAdvances.Of[Player]);

			var researchLevels = game.States.ResearchAdvances.Of[Player].ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
			var validTechs = new HashSet<string>(
					game.States.DevelopmentAdvances
					.Where(x => x.CanProgress(researchLevels, game.Statics))
					.Select(x => x.Topic.IdCode)
				);

			game.Orders[this.Player].DevelopmentQueue = updateTechQueue(game.Orders[this.Player].DevelopmentQueue, validTechs);
		}

		private void checkColonizationValidity(MainGame game)
		{
			var occupiedTargets = new HashSet<Planet>();
			foreach(var order in game.Orders[this.Player].ColonizationOrders)
				if (game.States.Colonies.AtPlanet.Contains(order.Key)) //TODO(later) use intelligence instead
					occupiedTargets.Add(order.Key);
			foreach(var planet in occupiedTargets)
				game.Orders[this.Player].ColonizationOrders.Remove(planet);
		}

		private void doConstruction(MainGame game)
		{
			var oldPlans = game.Orders[this.Player].ConstructionPlans;
			game.Orders[this.Player].ConstructionPlans = new Dictionary<AConstructionSite, ConstructionOrders>();

			foreach (var colony in game.States.Colonies.OwnedBy[Player])
				if (oldPlans.ContainsKey(colony)) {
					var updatedPlans = updateConstructionPlans(
						game.Statics,
						oldPlans[colony],
						game.Derivates.Of(colony),
						this.TechLevels
					);

					game.Orders[this.Player].ConstructionPlans.Add(colony, updatedPlans);
				}
				else
					game.Orders[this.Player].ConstructionPlans.Add(colony, new ConstructionOrders(PlayerOrders.DefaultSiteSpendingRatio));

				foreach (var stellaris in game.States.Stellarises.OwnedBy[Player])
					if (oldPlans.ContainsKey(stellaris)) 
					{
						var updatedPlans = updateConstructionPlans(
							game.Statics,
							oldPlans[stellaris],
							game.Derivates.Of(stellaris),
							this.TechLevels
						);

						game.Orders[this.Player].ConstructionPlans.Add(stellaris, updatedPlans);
					}
					else
						game.Orders[this.Player].ConstructionPlans.Add(stellaris, new ConstructionOrders(PlayerOrders.DefaultSiteSpendingRatio));
		}
		
		private void updateDesigns(MainGame game)
		{
			//Generate upgraded designs
			var upgradesTo = new Dictionary<Design, Design>();
			var newDesigns = new HashSet<Design>();
			foreach(var design in game.States.Designs.OwnedBy[this.Player])
			{
				var upgrade = this.DesignUpgrade(design, game.Statics, game.States);
				if (game.States.Designs.Contains(upgrade))
					continue;
				
				if (newDesigns.Contains(upgrade))
					upgrade = newDesigns.First(x => x == upgrade);
				else
					this.Analyze(upgrade, game.Statics);
				
				design.IsObsolete = true;
				upgradesTo[design] = upgrade;
				newDesigns.Add(upgrade);
			}
			game.States.Designs.Add(newDesigns);
			
			//Update refit orders to upgrade obsolete designs
			foreach(var upgrade in upgradesTo)
			{
				var orders = game.Orders[this.Player].RefitOrders;
				
				if (!orders.ContainsKey(upgrade.Key))
					orders[upgrade.Key] = upgrade.Value;
				else if (orders[upgrade.Key] != null && orders[upgrade.Key].IsObsolete)
					orders[upgrade.Key] = upgradesTo[orders[upgrade.Key]];
			}
			
			foreach(var site in game.Orders[this.Player].ConstructionPlans.Keys.ToList())
			{
				var updater = new ShipConstructionUpdater(
					game.Orders[this.Player].ConstructionPlans[site].Queue,
					game.Orders[this.Player].RefitOrders
				);
				game.Orders[this.Player].ConstructionPlans[site].Queue.Clear();
				game.Orders[this.Player].ConstructionPlans[site].Queue.AddRange(updater.Run());
			}

			//Removing inactive discarded designs
			var shipConstruction = new ShipConstructionCounter();
			shipConstruction.Check(game.Derivates.Stellarises.OwnedBy[this.Player]);
			
			var activeDesigns = new HashSet<Design>(game.States.Fleets.
			                                        SelectMany(x => x.Ships).
			                                        Select(x => x.Design).
			                                        Concat(shipConstruction.Designs));
			var discardedDesigns = game.Orders[this.Player].RefitOrders.
				Where(x => x.Value == null && !activeDesigns.Contains(x.Key)).
				Select(x => x.Key).ToList();
			
			foreach(var design in discardedDesigns)
			{
				game.Orders[design.Owner].RefitOrders.Remove(design);
				game.States.Designs.Remove(design);
				this.DesignStats.Remove(design);
				this.RefitCosts.Remove(design);
			}
			foreach(var design in this.RefitCosts.Keys)
				foreach(var discarded in discardedDesigns)
					this.RefitCosts[design].Remove(discarded);
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
			this.calcDesignStats(design, statics);
			this.calcRefitCosts(design, statics.ShipFormulas);
		}
		
		private void calcDesignStats(Design design, StaticsDB statics)
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
					x => AbilityStatsFactory.Create(x, equip.Level, equip.Quantity, statics)
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
		
		private void calcRefitCosts(Design design, ShipFormulaSet shipFormulas)
		{
			this.RefitCosts.Add(design, new Dictionary<Design, double>());
			
			var otherDesigns = this.DesignStats.Keys.Where(x => x.Hull.TypeInfo == design.Hull.TypeInfo && x != design).ToList();
			foreach(var otherDesign in otherDesigns)
			{
				this.RefitCosts[design].Add(otherDesign, refitCost(design, otherDesign, shipFormulas));
				this.RefitCosts[otherDesign].Add(design, refitCost(otherDesign, design, shipFormulas));
			}
		}
		
		private double refitCost(Design fromDesign, Design toDesign, ShipFormulaSet shipFormulas)
		{
			double cost = 0;
			var hullVars = new Var(AComponentType.LevelKey, toDesign.Hull.Level).Get;
			
			var hullCost = refitComponentCost(fromDesign.Hull, toDesign.Hull, x => x.Cost, null, shipFormulas);
			if (hullCost > 0)
				cost += hullCost;
			else
			{
				hullCost = toDesign.Hull.TypeInfo.Cost.Evaluate(new Var(AComponentType.LevelKey, toDesign.Hull.Level).Get);
				cost += hullCost * shipFormulas.ArmorCostPortion *  refitComponentCost(fromDesign.Armor, toDesign.Armor, x => new Formula(1), null, shipFormulas);
				cost += hullCost * shipFormulas.ReactorCostPortion *  refitComponentCost(fromDesign.Reactor, toDesign.Reactor, x => new Formula(1), null, shipFormulas);
				cost += hullCost * shipFormulas.SensorCostPortion *  refitComponentCost(fromDesign.Sensors, toDesign.Sensors, x => new Formula(1), null, shipFormulas);
				cost += hullCost * shipFormulas.ThrustersCostPortion *  refitComponentCost(fromDesign.Thrusters, toDesign.Thrusters, x => new Formula(1), null, shipFormulas);
			}
			
			cost += refitComponentCost(fromDesign.IsDrive, toDesign.IsDrive, x => x.Cost, new Var(AComponentType.SizeKey, toDesign.Hull.TypeInfo.SizeIS.Evaluate(hullVars)), shipFormulas);
			cost += refitComponentCost(fromDesign.Shield, toDesign.Shield, x => x.Cost, new Var(AComponentType.SizeKey, toDesign.Hull.TypeInfo.SizeShield.Evaluate(hullVars)), shipFormulas);
			cost += refitComponentCost(fromDesign.MissionEquipment, toDesign.MissionEquipment, x => x.Cost, null, shipFormulas);
			cost += refitComponentCost(fromDesign.SpecialEquipment, toDesign.SpecialEquipment, x => x.Cost, new Var(AComponentType.SizeKey, toDesign.Hull.TypeInfo.Size.Evaluate(hullVars)), shipFormulas);
			
			return cost;
		}
		
		private double refitComponentCost<T>(Component<T> fromComponent, Component<T> toComponent, Func<T, Formula> costFormula, Var extraVars, ShipFormulaSet shipFormulas) where T : AComponentType
		{
			if (toComponent == null)
				return 0;

			if (extraVars == null)
				extraVars = new Var();
			var fullCost = costFormula(toComponent.TypeInfo).Evaluate(new Var(AComponentType.LevelKey, toComponent.Level).UnionWith(extraVars.Get).Get);
			
			if (fromComponent == null || fromComponent.TypeInfo != toComponent.TypeInfo)
				return fullCost;
			
			if (fromComponent.Level < toComponent.Level)
				return fullCost * shipFormulas.LevelRefitCost.Evaluate(new Var(AComponentType.LevelKey, toComponent.Level - fromComponent.Level).Get);

			//refit from higher to lower or the same level is free
			return 0;
		}

		private double refitComponentCost<T>(List<Component<T>> fromComponents, List<Component<T>> toComponents, Func<T, Formula> costFormula, Var extraVars, ShipFormulaSet shipFormulas) where T : AComponentType
		{
			double cost = 0;

			var oldEquipment = fromComponents.
				GroupBy(x => x).
				ToDictionary(
					x => x.Key.TypeInfo,
					x => new Component<T>(x.Key.TypeInfo, x.Key.Level, x.Sum(y => y.Quantity))
				);
			foreach (var equip in toComponents)
			{
				var similarQuantity = 0;
				var oldEquip = oldEquipment.ContainsKey(equip.TypeInfo) ? oldEquipment[equip.TypeInfo] : null;

				if (oldEquip != null)
				{
					similarQuantity = Math.Min(oldEquip.Quantity, equip.Quantity);

					if (similarQuantity < oldEquip.Quantity)
						oldEquipment[equip.TypeInfo] = new Component<T>(equip.TypeInfo, oldEquip.Level, oldEquip.Quantity - similarQuantity);
					else
						oldEquipment.Remove(equip.TypeInfo);
				}

				cost += similarQuantity * refitComponentCost(oldEquip, equip, costFormula, extraVars, shipFormulas);
				cost += (equip.Quantity - similarQuantity) * refitComponentCost(null, equip, costFormula, extraVars, shipFormulas);
			}

			return cost;
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
		
		private void unlockPredefinedDesigns(MainGame game)
		{
			var playerTechs = game.States.DevelopmentAdvances.Of[Player];
			var techLevels = playerTechs.ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
				
			foreach(var predefDesign in game.Statics.PredeginedDesigns)
				if (!Player.UnlockedDesigns.Contains(predefDesign) && Prerequisite.AreSatisfied(predefDesign.Prerequisites(game.Statics), 0, techLevels))
				{
					Player.UnlockedDesigns.Add(predefDesign);
					makeDesign(game.Statics, game.States, predefDesign, techLevels, false);
				}
			
			this.ColonyShipDesign = updateVirtualDesign(this.ColonyShipDesign, game, game.Statics.ColonyShipDesigns, techLevels);
			this.SystemColonizerDesign = updateVirtualDesign(this.SystemColonizerDesign, game, game.Statics.SystemColonizerDesigns, techLevels);
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

		private Design updateVirtualDesign(Design oldDesign, MainGame game, IEnumerable<PredefinedDesign> predefDesigns, Dictionary<string, double> techLevels)
		{
			var newDesign = makeDesign(
				game.Statics, game.States,
				predefDesigns.Last(x => Prerequisite.AreSatisfied(x.Prerequisites(game.Statics), 0, techLevels)),
				techLevels, true);
			
			if (newDesign != oldDesign && oldDesign != null)
			{
				oldDesign.IsObsolete = true;
				game.Orders[oldDesign.Owner].RefitOrders[oldDesign] = null;
			}
			
			return newDesign;
		}
	}
}
