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

namespace Stareater.GameLogic
{
	class ColonyProcessor : AConstructionSiteProcessor
	{
		internal const string NewBuidingPrefix = "_delta";
		
		private const string MaxPopulationKey = "maxPop";
		public const string PlanetSizeKey = "size";
		private const string PopulationGrowthKey = "popGrowth";
		private const string PopulationKey = "pop";
		
		[StateProperty]
		public Colony Colony { get; set; }
		
		[StateProperty]
		public double Environment { get; private set; }
		[StateProperty]
		public double MaxPopulation { get; private set; }
		[StateProperty]
		public double PopulationGrowth { get; private set; }
		[StateProperty]
		public double Organization { get; private set; }
		[StateProperty]
		public double SpaceliftFactor { get; private set; }

		[StateProperty]
		public double FarmerEfficiency { get; private set; }
		[StateProperty]
		public double GardenerEfficiency { get; private set; }
		[StateProperty]
		public double MinerEfficiency { get; private set; }
		[StateProperty]
		public double BuilderEfficiency { get; private set; }
		[StateProperty]
		public double ScientistEfficiency { get; private set; }

		[StateProperty]
		public double Farmers { get; private set; }
		[StateProperty]
		public double Gardeners { get; private set; }
		[StateProperty]
		public double WorkingPopulation { get; private set; }

		[StateProperty]
		public double Development { get; private set; }
		[StateProperty]
		public double RepairPoints { get; protected set; }
		[StateProperty]
		public double MaintenanceCost { get; protected set; }
		[StateProperty]
		public double MaintenanceLimit { get; protected set; }

		public ColonyProcessor(Colony colony) : base()
		{
			this.Colony = colony;
		}
		
		private ColonyProcessor(Colony colony, ColonyProcessor original) : base(original)
		{
			this.Colony = colony;
			
			this.BuilderEfficiency = original.BuilderEfficiency;
			this.Development = original.Development;
			this.FarmerEfficiency = original.FarmerEfficiency;
			this.GardenerEfficiency = original.GardenerEfficiency;
			this.MaxPopulation = original.MaxPopulation;
			this.MinerEfficiency = original.MinerEfficiency;
			this.Organization = original.Organization;
			this.ScientistEfficiency = original.ScientistEfficiency;
			this.WorkingPopulation = original.WorkingPopulation;
		}

        private ColonyProcessor()
        { }
				
		public Player Owner 
		{ 
			get {
				return Colony.Owner;
			}
		}
		
		private IDictionary<string, double> calcVars(StaticsDB statics, PlayerProcessor playerProcessor)
		{
			var vars = base.LocalEffects(statics).
				And(PlanetSizeKey, Colony.Location.Planet.Size).
				And(PopulationKey, Colony.Population).
				UnionWith(playerProcessor.TechLevels).
				Init(statics.Traits.Keys, false).
				UnionWith(Colony.Location.Planet.Traits, x => x.Type.IdCode, x => 1);
				
			vars.Init(statics.Constructables.Where(x => x.ConstructableAt == SiteType.Colony).Select(x => x.IdCode.ToLower() + NewBuidingPrefix), false);

			return vars.Get;
		}
		
		public void CalculateBaseEffects(StaticsDB statics, PlayerProcessor playerProcessor)
		{
			var vars = calcVars(statics, playerProcessor);
			var formulas = statics.ColonyFormulas;
			var planetFormulas = statics.PlanetForumlas[this.Colony.Location.Planet.Type];
			
			this.Environment = formulas.EnvironmentFactor.Evaluate(vars);
			this.MaxPopulation = formulas.MaxPopulation.Evaluate(vars);
			this.Organization = formulas.Organization.Evaluate(vars);
			this.SpaceliftFactor = planetFormulas.SpaceliftFactor.Evaluate(vars);
			
			this.FarmerEfficiency = formulas.Farming.Evaluate(this.Organization, vars);
			this.GardenerEfficiency = formulas.Gardening.Evaluate(this.Organization, vars);
			this.MinerEfficiency = formulas.Mining.Evaluate(this.Organization, vars);
			
			var minersPerIndustry = 1 / this.MinerEfficiency;
			this.BuilderEfficiency = formulas.Industry.Evaluate(this.Organization, vars) / (1 + minersPerIndustry);
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

			this.MaintenanceCost = this.Colony.Population * (
				planetFormulas.MaintenanceCost.Evaluate(vars) + 
                this.Colony.Location.Planet.Traits.Sum(x => x.Type.MaintenanceCost)
			);
			this.MaintenanceLimit = this.WorkingPopulation * this.BuilderEfficiency * this.SpaceliftFactor;
        }
		
		public void CalculateDerivedEffects(StaticsDB statics, PlayerProcessor playerProcessor)
		{
			var vars = calcVars(statics, playerProcessor);
			var counter = new NewBuildingsCounter(vars); //TODO(v0.7) rename class and variable?

			foreach (var construction in SpendingPlan)
				if (construction.CompletedCount > 0)
					counter.Count(construction.Project, construction.CompletedCount);

			var formulas = statics.ColonyFormulas;
			this.PopulationGrowth = formulas.PopulationGrowth.Evaluate(vars);
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
			
			return vars;
		}

		public void AddPopulation(double arrivedPopulation)
		{
			Colony.Population = Methods.Clamp(Colony.Population + arrivedPopulation, 0, MaxPopulation);
		}
		
		public override void ProcessPrecombat(StatesDB states, TemporaryDB derivates)
		{
			base.ProcessPrecombat(states, derivates);
			Colony.Population = Methods.Clamp(Colony.Population + PopulationGrowth, 0, MaxPopulation);			
		}
	}
}
