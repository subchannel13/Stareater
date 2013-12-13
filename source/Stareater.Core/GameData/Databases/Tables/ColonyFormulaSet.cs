using System;
using Stareater.AppData.Expressions;

namespace Stareater.GameData.Databases.Tables
{
	class ColonyFormulaSet
	{
		public Formula MaxPopulation { get; private set; }
		public Formula Organization { get; private set; }
		
		public PopulationActivityFormulas Farming { get; private set; }
		public PopulationActivityFormulas Mining { get; private set; }
		
		public PopulationActivityFormulas Development { get; private set; }
		public PopulationActivityFormulas Industry { get; private set; }
		
		public ColonyFormulaSet(Formula maxPopulation, Formula organization, 
			PopulationActivityFormulas farming, PopulationActivityFormulas mining, 
			PopulationActivityFormulas development, PopulationActivityFormulas industry)
		{
			this.MaxPopulation = maxPopulation;
			this.Organization = organization;
			
			this.Farming = farming;
			this.Mining = mining;
			this.Development = development;
			this.Industry = industry;
		}
	}
}
