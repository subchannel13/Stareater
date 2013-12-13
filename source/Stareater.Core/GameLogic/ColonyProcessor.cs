using System;
using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;
using Stareater.Utils;
using Stareater.Utils.Collections;
using Stareater.GameData;

namespace Stareater.GameLogic
{
	class ColonyProcessor
	{
		private const string InfrastructureKey = "factories";
		private const string MaxPopulationKey = "maxPop";
		private const string PlanetSizeKey = "size";
		private const string PopulationKey = "pop";
		
		public Colony Colony { get; set; }
		
		public ColonyProcessor(Colony colony)
		{
			this.Colony = colony;
		}
		
		public Player Owner 
		{ 
			get {
				return Colony.Owner;
			}
		}
		
		public double MaxPopulation { get; private set; }
		public double Organization { get; private set; }
		
		public double Development { get; private set; }
		public double Production { get; private set; }
		
		private IDictionary<string, double> calcVars(PlayerProcessor playerProcessor)
		{
			return new Var(PlanetSizeKey, Colony.Location.Size)
				.And(PopulationKey, Colony.Population)
				.UnionWith(playerProcessor.TechLevels).Get;
		}
		
		public void Calculate(ColonyFormulaSet formulas, PlayerProcessor playerProcessor)
		{
			//TODO: add colony buildings
			var vars = calcVars(playerProcessor);
			
			this.MaxPopulation = formulas.MaxPopulation.Evaluate(vars);
			this.Organization = formulas.Organization.Evaluate(vars);
		}
		
		public void SimulateSpending(ColonyFormulaSet formulas, PlayerProcessor playerProcessor)
		{
			var vars = new Var(PlanetSizeKey, Colony.Location.Size).UnionWith(playerProcessor.TechLevels).Get;
			
			//TODO: include farming and mining
			double desiredSpendingRatio = Colony.Owner.Orders.Constructions[Colony].SpendingRatio;
			this.Development = (1 - desiredSpendingRatio) * Colony.Population * formulas.Development.Evaluate(Organization, vars);
			this.Production = desiredSpendingRatio * Colony.Population * formulas.Industry.Evaluate(Organization, vars);
		}
		
		public IDictionary<string, double> Effects()
		{
			var vars = new Var(PlanetSizeKey, Colony.Location.Size);
			vars.And(InfrastructureKey, 0); //TODO: add as building count
			vars.And(MaxPopulationKey, MaxPopulation);
			vars.And(PopulationKey, Colony.Population);
			
			return vars.Get;
		}

		internal ColonyProcessor Copy(PlayersRemap playerRemap)
		{
			ColonyProcessor copy = new ColonyProcessor(playerRemap.Colonies[this.Colony]);
			
			copy.Development = this.Development;
			copy.MaxPopulation = this.MaxPopulation;
			copy.Production = this.Production;

			return copy;
		}
		
		public double DevelopmentPoints()
		{
			//TODO: calculate from population and system
			//TODO: use Development getter
			return 30e9;
		}
	}
}
