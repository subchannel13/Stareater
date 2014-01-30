using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Galaxy;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;
using Stareater.Utils;
using Stareater.Utils.Collections;
using Stareater.GameData;

namespace Stareater.GameLogic
{
	class ColonyProcessor : AConstructionSiteProcessor
	{
		private const string InfrastructureKey = "factories";
		private const string MaxPopulationKey = "maxPop";
		private const string PlanetSizeKey = "size";
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
		public double Organization { get; private set; }
		
		public double FarmerEfficiency { get; private set; }
		public double GardenerEfficiency { get; private set; }
		public double MinerEfficiency { get; private set; }
		public double BuilderEfficiency { get; private set; }
		public double ScientistEfficiency { get; private set; }

		public double WorkingPopulation { get; private set; }
		public double Development { get; private set; }
		
		private IDictionary<string, double> calcVars(PlayerProcessor playerProcessor)
		{
			return new Var(PlanetSizeKey, Colony.Location.Size)
				.And(PopulationKey, Colony.Population)
				.UnionWith(playerProcessor.TechLevels).Get;
		}
		
		public void CalculateBaseEffects(ColonyFormulaSet formulas, PlayerProcessor playerProcessor)
		{
			//TODO: add colony buildings
			var vars = calcVars(playerProcessor);
			
			this.MaxPopulation = formulas.MaxPopulation.Evaluate(vars);
			this.Organization = formulas.Organization.Evaluate(vars);
			
			this.FarmerEfficiency = formulas.Farming.Evaluate(this.Organization, vars);
			this.GardenerEfficiency = formulas.Gardening.Evaluate(this.Organization, vars);
			this.MinerEfficiency = formulas.Mining.Evaluate(this.Organization, vars);
			//TODO: factor in miners
			this.BuilderEfficiency = formulas.Industry.Evaluate(this.Organization, vars);
			this.ScientistEfficiency = formulas.Development.Evaluate(this.Organization, vars);

			//TODO: factor in farmers
			this.WorkingPopulation = this.Colony.Population;
		}
		
		public void CalculateSpending(ColonyFormulaSet formulas, PlayerProcessor playerProcessor)
		{
			var vars = this.LocalEffects().UnionWith(playerProcessor.TechLevels).Get;

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

		public override Var LocalEffects()
		{
			var vars = new Var(PlanetSizeKey, Colony.Location.Size);
			vars.And(InfrastructureKey, 0); //TODO: add as building count
			vars.And(MaxPopulationKey, MaxPopulation);
			vars.And(PopulationKey, Colony.Population);
			
			return vars;
		}
		
		public void ProcessPrecombat()
		{
			/*
			 * TODO: Colonies, 1st pass
			 * - Build (consume construction queue)
			 * - Apply instant effect buildings
			 * - Apply terraforming
			 * - Grow population
			 */
			//throw new NotImplementedException();
		}
	}
}
