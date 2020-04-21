using System.Collections.Generic;
using System.Linq;

using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;
using Stareater.Utils;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using Stareater.GameLogic.Planning;
using System;

namespace Stareater.GameLogic
{
	class ColonyProcessor : AConstructionSiteProcessor
	{
		internal const string NewBuidingPrefix = "_delta";
		
		private const string MaxPopulationKey = "maxPop";
		public const string PlanetSizeKey = "size";
		private const string PopulationKey = "pop";
		private const string MaintenancePenaltyKey = "maintenancePenalty";

		[StatePropertyAttribute]
		public Colony Colony { get; set; }
		
		[StatePropertyAttribute]
		public double Environment { get; private set; }
		[StatePropertyAttribute]
		public double MaxPopulation { get; private set; }
		[StatePropertyAttribute]
		public double PopulationGrowth { get; private set; }
		[StatePropertyAttribute]
		public double Emigrants { get; private set; }
		[StatePropertyAttribute]
		public double Organization { get; private set; }
		[StatePropertyAttribute]
		public double SpaceliftFactor { get; private set; }
		[StatePropertyAttribute]
		public double Desirability { get; private set; }

		[StatePropertyAttribute]
		public double FarmerEfficiency { get; private set; }
		[StatePropertyAttribute]
		public double GardenerEfficiency { get; private set; }
		[StatePropertyAttribute]
		public double MiningEfficiency { get; private set; }
		[StatePropertyAttribute]
		public double BuilderEfficiency { get; private set; }
		[StatePropertyAttribute]
		public double ScientistEfficiency { get; private set; }

		[StatePropertyAttribute]
		public double Farmers { get; private set; }
		[StatePropertyAttribute]
		public double Gardeners { get; private set; }
		[StatePropertyAttribute]
		public double WorkingPopulation { get; private set; }

		[StatePropertyAttribute]
		public double Development { get; private set; }
		[StatePropertyAttribute]
		public double RepairPoints { get; private set; }
		[StatePropertyAttribute]
		public double MaintenanceCost { get; private set; }
		[StatePropertyAttribute]
		public double MaintenancePerPop { get; private set; }
		[StatePropertyAttribute]
		public double MaintenanceLimit { get; private set; }
		[StatePropertyAttribute]
		public double MaintenancePenalty { get; set; }
		[StatePropertyAttribute]
		public double FuelProduction { get; internal set; }
		[StatePropertyAttribute]
		public Dictionary<string, double> ExtraStats { get; internal set; }

		public ColonyProcessor(Colony colony) : base()
		{
			this.Colony = colony;
		}

        private ColonyProcessor()
        { }
				
		public Player Owner 
		{ 
			get {
				return Colony.Owner;
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "ToLower is required")]
		private IDictionary<string, double> calcVars(StaticsDB statics, PlayerProcessor playerProcessor)
		{
			var vars = base.LocalEffects(statics).
				And(PlanetSizeKey, Colony.Location.Planet.Size).
				And(PopulationKey, Colony.Population).
				UnionWith(playerProcessor.TechLevels).
				Init(statics.PlanetTraits.Keys, false).
				UnionWith(Colony.Location.Planet.Traits.Select(x => x.IdCode)).
				UnionWith(statics.PlanetForumlas[this.Colony.Location.Planet.Type].ImplicitTraits);
				
			vars.Init(
				statics.Constructables.
					Where(x => x.ConstructableAt == SiteType.Colony).
					Select(x => x.IdCode.ToLowerInvariant() + NewBuidingPrefix), 
				false
			);

			return vars.Get;
		}
		
		public void CalculateBaseEffects(StaticsDB statics, PlayerProcessor playerProcessor)
		{
			var vars = calcVars(statics, playerProcessor);
			var formulas = statics.ColonyFormulas;
			var planetEffects = statics.PlanetForumlas[this.Colony.Location.Planet.Type];

			this.Environment = formulas.EnvironmentFactor.Evaluate(vars);
			this.MaxPopulation = formulas.MaxPopulation.Evaluate(vars);
			this.Organization = formulas.Organization.Evaluate(vars);
			this.SpaceliftFactor = formulas.SpaceliftFactor.Evaluate(vars);

			this.FarmerEfficiency = formulas.Farming.Evaluate(this.Organization, vars);
			this.GardenerEfficiency = formulas.Gardening.Evaluate(this.Organization, vars);
			this.MiningEfficiency = formulas.Minerals.Evaluate(vars);
			
			this.BuilderEfficiency = formulas.Industry.Evaluate(this.Organization, vars) * MiningEfficiency;
			this.ScientistEfficiency = formulas.Development.Evaluate(this.Organization, vars);

			this.Farmers = this.Colony.Population / this.FarmerEfficiency;
			this.Gardeners = 0;
			
			var farmFields = formulas.FarmFields.Evaluate(vars);
			if (this.Farmers > farmFields)
			{
				this.Gardeners = (this.Colony.Population - this.FarmerEfficiency * farmFields) / this.GardenerEfficiency;
				this.Farmers = farmFields + this.Gardeners;
			}
			
			this.WorkingPopulation = this.Colony.Population - this.Farmers;
			this.RepairPoints = formulas.RepairPoints.Evaluate(vars);

			this.MaintenancePerPop = this.Colony.Location.Planet.Traits.
				Concat(planetEffects.ImplicitTraits.Select(x => statics.PlanetTraits[x])).
				Sum(x => x.MaintenanceCost.Evaluate(vars));
			this.MaintenanceCost = this.Colony.Population * this.MaintenancePerPop;
            this.MaintenanceLimit = this.WorkingPopulation * this.BuilderEfficiency * this.SpaceliftFactor;
			this.MaintenancePenalty = 0;

			vars[MaxPopulationKey] = this.MaxPopulation;
			this.PopulationGrowth = formulas.PopulationGrowth.Evaluate(vars);
			this.Desirability = formulas.Desirability.Evaluate(vars);
			this.Emigrants = formulas.Emigrants.Evaluate(vars);
			this.FuelProduction = formulas.FuelProduction.Evaluate(vars);
		}
		
		public void CalculateDerivedEffects(StaticsDB statics, PlayerProcessor playerProcessor)
		{
			var vars = calcVars(statics, playerProcessor);
			vars[MaintenancePenaltyKey] = this.MaintenancePenalty;
			var counter = new ConstructionCounterVisitor(vars);

			foreach (var construction in SpendingPlan)
				if (construction.CompletedCount > 0)
					counter.Count(construction.Project, construction.CompletedCount);

			vars[MaxPopulationKey] = this.MaxPopulation;
			this.ExtraStats = statics.ExtraColonyFormulas.ToDictionary(
				x => x.Key,
				x => x.Value.Evaluate(vars)
			);
		}
		
		public void CalculateSpending(MainGame game, PlayerProcessor playerProcessor)
		{
			var vars = this.LocalEffects(game.Statics).UnionWith(playerProcessor.TechLevels).Get;
			ColonyFormulaSet formulas = game.Statics.ColonyFormulas;
			var orders = game.Orders[this.Colony.Owner];

			double industryPotential =
				(1 - playerProcessor.MaintenanceRatio) *
				this.BuilderEfficiency *
				this.WorkingPopulation;
			double industryPoints =
				orders.ConstructionPlans[this.Colony].SpendingRatio * 
				industryPotential;
			
			this.SpendingPlan = SimulateSpending(
				Colony, 
				industryPoints,
				orders.ConstructionPlans[this.Colony].Queue, 
				vars
			);
			this.Production = this.SpendingPlan.Sum(x => x.InvestedPoints);

			this.SpendingRatioEffective = (industryPotential > 0) ?
				this.Production / industryPotential :
				0;
		}
		
		public void CalculateDevelopment(double systemSpandingRatio, double maintenanceRatio)
		{
			this.Development = 
				(1 - this.SpendingRatioEffective) * 
				(1 - systemSpandingRatio) *
				(1 - maintenanceRatio) *
				this.WorkingPopulation *
				this.ScientistEfficiency;
		}

		protected override AConstructionSite Site 
		{
			get 
			{
				return Colony;
			}
		}

		public override Var LocalEffects(StaticsDB statics)
		{
			var vars = base.LocalEffects(statics);
			vars.And(PlanetSizeKey, Colony.Location.Planet.Size);
			vars.And(MaxPopulationKey, MaxPopulation);
			vars.And(PopulationKey, Colony.Population);
			vars.Init(statics.PlanetTraits.Keys, false);
			vars.UnionWith(this.Colony.Location.Planet.Traits.Select(x => x.IdCode));
			
			return vars;
		}

		public void AddPopulation(double arrivedPopulation)
		{
			Colony.Population = Methods.Clamp(this.Colony.Population + arrivedPopulation, 0, this.MaxPopulation);
		}
		
		public override void ProcessPrecombat(MainGame game)
		{
			base.ProcessPrecombat(game);
			Colony.Population = Methods.Clamp(this.Colony.Population + this.PopulationGrowth, 0, this.MaxPopulation);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "ToLower is required")]
		public static double DesirabilityOf(Planet planet, StaticsDB statics)
		{
			var formulas = statics.ColonyFormulas;
			var vars = new Var(PopulationKey, 0).
				And(PlanetSizeKey, planet.Size).
				Init(statics.Buildings.Keys.Select(x => x.ToLowerInvariant() + BuidingCountPrefix), 0).
				Init(statics.DevelopmentTopics.Select(x => x.IdCode + PlayerProcessor.LevelSufix), 0).
				Init(statics.DevelopmentTopics.Select(x => x.IdCode + PlayerProcessor.UpgradeSufix), DevelopmentProgress.NotStarted).
				Init(statics.PlanetTraits.Keys, false).
				UnionWith(planet.Traits.Select(x => x.IdCode)).
				UnionWith(statics.PlanetForumlas[planet.Type].ImplicitTraits);

			vars.And(MaxPopulationKey, formulas.MaxPopulation.Evaluate(vars.Get));
			return formulas.Desirability.Evaluate(vars.Get);
		}
	}
}
