using System;
using Stareater.AppData.Expressions;

namespace Stareater.GameData.Databases.Tables
{
	class ColonyFormulaSet
	{
		public Formula ColonizationPopulationThreshold { get; private set; }
		public Formula UncolonizedMaxPopulation { get; private set; }
		public Formula FarmFields { get; private set; }
		
		public Formula MaxPopulation { get; private set; }
		public DerivedStatistic PopulationGrowth { get; private set; }
		public Formula Organization { get; private set; }
		public Formula SpaceliftFactor { get; private set; }
		
		public PopulationActivityFormulas Farming { get; private set; }
		public PopulationActivityFormulas Gardening { get; private set; }
		public PopulationActivityFormulas Mining { get; private set; }
		
		public PopulationActivityFormulas Development { get; private set; }
		public PopulationActivityFormulas Industry { get; private set; }
		
		public Formula PopulationHitPoints { get; private set; }
		
		public ColonyFormulaSet(Formula colonizationPopThreshold, Formula uncolonizedMaxPopulation, Formula farmFields,
		                        Formula maxPopulation, DerivedStatistic populationGrowth, Formula organization, Formula spaceliftFactor, 
		                        PopulationActivityFormulas farming, PopulationActivityFormulas gardening,
		                        PopulationActivityFormulas mining, PopulationActivityFormulas development, 
		                        PopulationActivityFormulas industry,
		                        Formula populationHitPoints)
		{
			this.ColonizationPopulationThreshold = colonizationPopThreshold;
			this.UncolonizedMaxPopulation = uncolonizedMaxPopulation;
			this.FarmFields = farmFields;
			this.MaxPopulation = maxPopulation;
			this.PopulationGrowth = populationGrowth;
			this.Organization = organization;
			this.SpaceliftFactor = spaceliftFactor;
			
			this.Farming = farming;
			this.Gardening = gardening;
			this.Mining = mining;
			this.Development = development;
			this.Industry = industry;
			
			this.PopulationHitPoints = populationHitPoints;
		}
	}
}
