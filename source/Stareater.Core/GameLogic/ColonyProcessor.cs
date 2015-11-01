using System;
using System.Collections.Generic;
using System.Linq;

using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;
using Stareater.Utils;
using Stareater.Utils.Collections;

namespace Stareater.GameLogic
{
	class ColonyProcessor : AConstructionSiteProcessor
	{
		private const string NewBuidingPrefix = "_delta";
		
		private const string MaxPopulationKey = "maxPop";
		public const string PlanetSizeKey = "size";
		private const string PopulationGrowthKey = "popGrowth";
		private const string PopulationKey = "pop";
		
		public Colony Colony { get; set; }
		
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
		
		internal ColonyProcessor Copy(PlayersRemap playerRemap)
		{
			return new ColonyProcessor(playerRemap.Colonies[this.Colony], this);
		}
				
		public Player Owner 
		{ 
			get {
				return Colony.Owner;
			}
		}
		
		public double MaxPopulation { get; private set; }
		public double PopulationGrowth { get; private set; }
		public double Organization { get; private set; }
		
		public double FarmerEfficiency { get; private set; }
		public double GardenerEfficiency { get; private set; }
		public double MinerEfficiency { get; private set; }
		public double BuilderEfficiency { get; private set; }
		public double ScientistEfficiency { get; private set; }

		public double WorkingPopulation { get; private set; }
		public double Development { get; private set; }
		
		private IDictionary<string, double> calcVars(StaticsDB statics, PlayerProcessor playerProcessor)
		{
			var vars = base.LocalEffects(statics);
			vars.And(PlanetSizeKey, Colony.Location.Planet.Size);
			vars.And(PopulationKey, Colony.Population);
			vars.UnionWith(playerProcessor.TechLevels);

			foreach(var constructable in statics.Constructables)
				if (constructable.ConstructableAt == SiteType.Colony)
					vars.And(constructable.IdCode.ToLower() + NewBuidingPrefix, 0);

			return vars.Get;

		}
		
		public void CalculateBaseEffects(StaticsDB statics, PlayerProcessor playerProcessor)
		{
			var vars = calcVars(statics, playerProcessor);
			var formulas = statics.ColonyFormulas;
			
			this.MaxPopulation = formulas.MaxPopulation.Evaluate(vars);
			this.Organization = formulas.Organization.Evaluate(vars);
			
			this.FarmerEfficiency = formulas.Farming.Evaluate(this.Organization, vars);
			this.GardenerEfficiency = formulas.Gardening.Evaluate(this.Organization, vars);
			this.MinerEfficiency = formulas.Mining.Evaluate(this.Organization, vars);
			//TODO(v0.5): factor in miners
			this.BuilderEfficiency = formulas.Industry.Evaluate(this.Organization, vars);
			this.ScientistEfficiency = formulas.Development.Evaluate(this.Organization, vars);

			//TODO(v0.5): factor in farmers
			this.WorkingPopulation = this.Colony.Population;
		}
		
		public void CalculateDerivedEffects(StaticsDB statics, PlayerProcessor playerProcessor)
		{
			var vars = calcVars(statics, playerProcessor);
			var formulas = statics.ColonyFormulas;
			
			foreach(var construction in SpendingPlan)
				if (construction.CompletedCount > 0)
					vars[construction.Type.IdCode.ToLower() + NewBuidingPrefix] = construction.CompletedCount;
			
			this.PopulationGrowth = formulas.PopulationGrowth.Evaluate(vars);
		}
		
		public void CalculateSpending(StaticsDB statics, PlayerProcessor playerProcessor)
		{
			var vars = this.LocalEffects(statics).UnionWith(playerProcessor.TechLevels).Get;
			ColonyFormulaSet formulas = statics.ColonyFormulas;

			double industryPotential = Colony.Population * this.BuilderEfficiency;
			double industryPoints = 
				Colony.Owner.Orders.ConstructionPlans[Colony].SpendingRatio * 
				industryPotential;
			
			this.SpendingPlan = SimulateSpending(
				Colony, 
				industryPoints, 
				Colony.Owner.Orders.ConstructionPlans[Colony].Queue, 
				vars
			);
			this.Production = this.SpendingPlan.Sum(x => x.InvestedPoints);

			this.SpendingRatioEffective = (industryPotential > 0) ?
				this.Production / industryPotential :
				0;
		}
		
		public void CalculateDevelopment(double systemSpandingRatio)
		{
			this.Development = 
				(1 - this.SpendingRatioEffective) * 
				(1 - systemSpandingRatio) *
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

		public override void ProcessPrecombat(StatesDB states, TemporaryDB derivates)
		{
			base.ProcessPrecombat(states, derivates);
			Colony.Population = Methods.Clamp(Colony.Population + PopulationGrowth, 0, MaxPopulation);
			
			/* TODO(v0.5): Colonies, 1st pass
			 * - Build (consume construction queue)
			 * - Apply instant effect buildings
			 * - Apply terraforming
			 * - Grow population
			 */
		}
	}
}
