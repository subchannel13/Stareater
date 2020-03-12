using System.Collections.Generic;
using System.Linq;
using Stareater.Galaxy;
using Stareater.Players;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using Stareater.GameData.Construction;
using Stareater.GameData;
using Stareater.GameLogic.Planning;
using System;
using Stareater.Ships;
using Stareater.Utils;

namespace Stareater.GameLogic
{
	class StellarisProcessor : AConstructionSiteProcessor
	{
		[StatePropertyAttribute]
		public StellarisAdmin Stellaris { get; set; }

		[StatePropertyAttribute]
		public Dictionary<Colony, double> EmigrantionPlan = new Dictionary<Colony, double>();
		[StatePropertyAttribute]
		public Dictionary<Colony, double> ImmigrantionPlan = new Dictionary<Colony, double>();
		[StatePropertyAttribute]
		public double IsMigrants;
		[StatePropertyAttribute]
		public Dictionary<StarData, double> IsMigrationPlan = new Dictionary<StarData, double>();
		[StatePropertyAttribute]
		public double MissingPopulation;

		[StatePropertyAttribute]
		public double ScanRange { get; private set; }

		public StellarisProcessor(StellarisAdmin stellaris) : base()
		{
			this.Stellaris = stellaris;
		}

		private StellarisProcessor()
		{ }

		public Player Owner
		{
			get
			{
				return Stellaris.Owner;
			}
		}

		public StarData Location
		{
			get
			{
				return Stellaris.Location.Star;
			}
		}

		public void ApplyPolicy(MainGame game, SystemPolicy policy)
		{
			//TODO(v0.8) remove previous policy buildings
			var playerProc = game.Derivates[this.Owner];
			var playerTechs = game.States.DevelopmentAdvances.Of[this.Owner].ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
			var comparer = new ConstructionComparer();

			foreach (var colony in game.States.Colonies.AtStar[this.Location, this.Owner])
			{
				var plan = game.Orders[this.Owner].ConstructionPlans[colony];
				plan.SpendingRatio = policy.SpendingRatio;

				var colonyProc = game.Derivates[colony];
				var colonyVars = colonyProc.LocalEffects(game.Statics).
					UnionWith(playerProc.TechLevels).Get;

				//TODO(0.8) conver Statics.Constructables to dictionary
				foreach (var project in policy.Queue.Select(x => game.Statics.Constructables.First(p => p.IdCode == x)))
					if (plan.Queue.All(x => !comparer.Compare(x, project)) &&
						Prerequisite.AreSatisfied(project.Prerequisites, 0, playerTechs) &&
						project.Condition.Evaluate(colonyVars) >= 0)
					{
						plan.Queue.Add(new StaticProject(project));
					}

				colonyProc.CalculateSpending(game, playerProc);
			}

			this.CalculateSpending(game);

			foreach (var colony in game.States.Colonies.AtStar[this.Location, this.Owner])
				game.Derivates[colony].CalculateDerivedEffects(game.Statics, playerProc);
		}

		public void UndoPolicy(MainGame game)
		{
			var policy = game.Orders[this.Owner].Policies[this.Site as StellarisAdmin];
			var comparer = new ConstructionComparer();

			foreach (var colony in game.States.Colonies.AtStar[this.Location, this.Owner])
			{
				var toRemove = new HashSet<IConstructionProject>();
				foreach (var project in game.Orders[this.Owner].ConstructionPlans[colony].Queue)
					if (policy.Queue.Any(x => !comparer.Compare(project, game.Statics.Constructables.First(p => p.IdCode == x))))
						toRemove.Add(project);

				game.Orders[this.Owner].ConstructionPlans[colony].Queue.RemoveAll(toRemove.Contains);
			}
		}

		public void CalculateBaseEffects(MainGame game)
		{
			var vars = this.LocalEffects(game.Statics).
				UnionWith(game.Derivates[this.Owner].TechLevels).Get;

			this.ScanRange = game.Statics.StellarisFormulas.ScanRange.Evaluate(vars);
		}

		public void CalculateDerivedEffects(MainGame game)
		{
			var systemColonies = this.systemColonies(game).ToList();
            var colonyStats = systemColonies.
                Where(x => x.Colony.Population < x.MaxPopulation).
                OrderBy(x => x.Desirability).
                ToList();

			this.EmigrantionPlan = systemColonies.ToDictionary(x => x.Colony, x => 0.0);
			this.ImmigrantionPlan = systemColonies.ToDictionary(x => x.Colony, x => 0.0);

			for (int fromI = 0; fromI < colonyStats.Count; fromI++)
			{
				var origin = colonyStats[fromI];
				var destinations = colonyStats.Where(x => x.Desirability > origin.Desirability).ToList();
				var weightSum = destinations.Sum(x => x.Desirability);
				var emigrants = origin.Emigrants;

				foreach (var site in destinations)
				{
					var immigrants = Math.Min(
						emigrants * site.Desirability / weightSum,
						site.MaxPopulation - site.Colony.Population
					);

					this.ImmigrantionPlan[site.Colony] += immigrants;
					this.EmigrantionPlan[colonyStats[fromI].Colony] += immigrants;
				}
			}

			this.MissingPopulation = systemColonies.Sum(x => x.MaxPopulation - x.Colony.Population - this.ImmigrantionPlan[x.Colony]);
			this.IsMigrants = Math.Max(
				systemColonies.Sum(x => x.Emigrants - this.EmigrantionPlan[x.Colony]) - this.MissingPopulation,
				0
			);
		}

		public void CalculateSpending(MainGame game)
		{
			var playerProcessor = game.Derivates[this.Owner];
			var vars = new Var().UnionWith(playerProcessor.TechLevels).Get;
			var colonies = this.systemColonies(game).ToList();

			var automationPlan = game.Orders[Stellaris.Owner].AutomatedConstruction[Stellaris];
			var normalPlan = game.Orders[Stellaris.Owner].ConstructionPlans[Stellaris];

			double industryPotential = colonies.Sum(x =>
				(1 - x.SpendingRatioEffective) *
				(1 - playerProcessor.MaintenanceRatio) * 
                x.WorkingPopulation *
				x.BuilderEfficiency *
				x.SpaceliftFactor
			);
			double industryPoints = 
				Math.Max(normalPlan.SpendingRatio, automationPlan.SpendingRatio) *
				industryPotential;

			this.SpendingPlan = SimulateSpending(
				Stellaris,
				industryPoints,
				automationPlan.Queue.Concat(normalPlan.Queue),
				vars
			);
			this.Production = this.SpendingPlan.Sum(x => x.InvestedPoints);

			this.SpendingRatioEffective = (industryPotential > 0) ?
				this.Production / industryPotential :
				0;

			foreach (var colonyProc in colonies)
				colonyProc.CalculateDevelopment(this.SpendingRatioEffective, playerProcessor.MaintenanceRatio);
		}

		protected override AConstructionSite Site 
		{
			get 
			{
				return Stellaris;
			}
		}

		public override void ProcessPrecombat(MainGame game)
		{
			base.ProcessPrecombat(game);

			foreach (var plan in this.EmigrantionPlan)
				plan.Key.Population -= plan.Value;
			foreach (var plan in this.ImmigrantionPlan)
				plan.Key.Population += plan.Value;

			var playerProc = game.Derivates[this.Owner];
			var mockTransports = new Dictionary<Design, long>
			{
				{game.Orders[this.Owner].ColonizerDesign, 1 }
			};
			foreach (var plan in this.IsMigrationPlan)
			{
				var turn = game.Turn + playerProc.
					ShortestPathTo(this.Location, plan.Key, mockTransports, game).
					Max(m => (int)Math.Ceiling(m.Cost));

				var destination = game.States.Stellarises.At[plan.Key, this.Owner];

				if (destination.IncomingMigrants.ContainsKey(turn))
					destination.IncomingMigrants[turn] += plan.Value;
				else
					destination.IncomingMigrants.Add(turn, plan.Value);
			}
		}

		public void ProcessPostCombat(MainGame game)
		{
			var arrivedGroups = this.Stellaris.IncomingMigrants.
				Where(x => x.Key <= game.Turn + 1).
				ToList();
			var arrivedPop = arrivedGroups.Sum(x => x.Value);

			if (arrivedPop > 0)
				Methods.DistributePoints(
					arrivedPop,
					this.systemColonies(game).Select(colony => new PointReceiver(
						colony.Desirability,
						colony.MaxPopulation - colony.Colony.Population,
						x => { colony.Colony.Population += x; }
					))
				);

			foreach (var group in arrivedGroups)
				this.Stellaris.IncomingMigrants.Remove(group.Key);
		}

		private IEnumerable<ColonyProcessor> systemColonies(MainGame game)
		{
			return game.Derivates.Colonies.At[this.Location, this.Owner];
		}
	}
}
