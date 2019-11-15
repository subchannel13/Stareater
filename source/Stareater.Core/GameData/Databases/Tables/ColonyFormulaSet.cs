using Stareater.AppData.Expressions;

namespace Stareater.GameData.Databases.Tables
{
	class ColonyFormulaSet
	{
		public Formula ColonizationPopulationThreshold { get; private set; }
		public Formula UncolonizedMaxPopulation { get; private set; }
		public Formula MaxPopulation { get; private set; }
		public Formula VictoryPointWorth { get; private set; }

		public Formula FarmFields { get; private set; }
		public Formula EnvironmentFactor { get; private set; }
		public DerivedStatistic PopulationGrowth { get; private set; }
		public Formula Minerals { get; private set; }
		public Formula Emigrants { get; private set; }
		public Formula Organization { get; private set; }
		public Formula SpaceliftFactor { get; private set; }
		public Formula Desirability { get; private set; }

		public PopulationActivityFormulas Farming { get; private set; }
		public PopulationActivityFormulas Gardening { get; private set; }
		
		public PopulationActivityFormulas Development { get; private set; }
		public PopulationActivityFormulas Industry { get; private set; }

		public Formula FuelProduction { get; private set; }
		public Formula FuelCost { get; private set; }
		public Formula RepairPoints { get; private set; }
		public Formula PopulationHitPoints { get; private set; }
		public Formula MaintenanceTotalLimit { get; private set; }

		public ColonyFormulaSet(
			Formula colonizationPopThreshold, Formula uncolonizedMaxPopulation, Formula victoryPointWorth, 
			Formula farmFields, Formula environmentFactor, 
			Formula maxPopulation, DerivedStatistic populationGrowth, Formula minerals, Formula emigrants,
			Formula organization, Formula spaceliftFactor, Formula desirability,
			PopulationActivityFormulas farming, PopulationActivityFormulas gardening,
			PopulationActivityFormulas development, PopulationActivityFormulas industry,
            Formula fuelProduction, Formula fuelCost, Formula repairPoints, 
			Formula populationHitPoints, Formula MaintenanceTotalLimit)
		{
			this.ColonizationPopulationThreshold = colonizationPopThreshold;
			this.UncolonizedMaxPopulation = uncolonizedMaxPopulation;
			this.VictoryPointWorth = victoryPointWorth;

            this.FarmFields = farmFields;
			this.EnvironmentFactor = environmentFactor;
			this.MaxPopulation = maxPopulation;
			this.PopulationGrowth = populationGrowth;
			this.Minerals = minerals;
			this.Emigrants = emigrants;
			this.Organization = organization;
			this.SpaceliftFactor = spaceliftFactor;
			this.Desirability = desirability;
			
			this.Farming = farming;
			this.Gardening = gardening;
			
			this.Development = development;
			this.Industry = industry;

			this.FuelProduction = fuelProduction;
			this.FuelCost = fuelCost;
			this.RepairPoints = repairPoints;
			this.PopulationHitPoints = populationHitPoints;
			this.MaintenanceTotalLimit = MaintenanceTotalLimit;
		}
	}
}
