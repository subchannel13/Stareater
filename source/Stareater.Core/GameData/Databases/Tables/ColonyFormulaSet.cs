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
		public Formula Emigrants { get; private set; }
		public Formula Organization { get; private set; }
		public Formula SpaceliftFactor { get; private set; }
		public Formula Desirability { get; private set; }

		public PopulationActivityFormulas Farming { get; private set; }
		public PopulationActivityFormulas Gardening { get; private set; }
		public PopulationActivityFormulas Mining { get; private set; }
		
		public PopulationActivityFormulas Development { get; private set; }
		public PopulationActivityFormulas Industry { get; private set; }

		public Formula FuelProduction { get; private set; }
		public Formula RepairPoints { get; private set; }
		public Formula PopulationHitPoints { get; private set; }

		public ColonyFormulaSet(
			Formula colonizationPopThreshold, Formula uncolonizedMaxPopulation, Formula victoryPointWorth, 
			Formula farmFields, Formula environmentFactor, 
			Formula maxPopulation, DerivedStatistic populationGrowth, Formula emigrants,
			Formula organization, Formula spaceliftFactor, Formula desirability,
			PopulationActivityFormulas farming, PopulationActivityFormulas gardening,
			PopulationActivityFormulas mining, PopulationActivityFormulas development, 
			PopulationActivityFormulas industry,
            Formula fuelProduction, Formula repairPoints, Formula populationHitPoints)
		{
			this.ColonizationPopulationThreshold = colonizationPopThreshold;
			this.UncolonizedMaxPopulation = uncolonizedMaxPopulation;
			this.VictoryPointWorth = victoryPointWorth;

            this.FarmFields = farmFields;
			this.EnvironmentFactor = environmentFactor;
			this.MaxPopulation = maxPopulation;
			this.PopulationGrowth = populationGrowth;
			this.Emigrants = emigrants;
			this.Organization = organization;
			this.SpaceliftFactor = spaceliftFactor;
			this.Desirability = desirability;
			
			this.Farming = farming;
			this.Gardening = gardening;
			this.Mining = mining;
			this.Development = development;
			this.Industry = industry;

			this.FuelProduction = fuelProduction;
			this.RepairPoints = repairPoints;
			this.PopulationHitPoints = populationHitPoints;
		}
	}
}
