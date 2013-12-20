using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ikadn.Ikon.Types;
using Stareater.AppData.Expressions;
using Stareater.GameData.Databases.Tables;
using Stareater.GameData.Reading;
using Stareater.Utils;

namespace Stareater.GameData.Databases
{
	internal class StaticsDB
	{
		public List<Constructable> Constructables { get; private set; }
		public List<Technology> Technologies { get; private set; }
		public ColonyFormulaSet ColonyFormulas { get; private set; }
		
		public StaticsDB()
		{
			this.Constructables = new List<Constructable>();
			this.Technologies = new List<Technology>();
		}
		
		public IEnumerable<double> Load(params string[] paths)
		{
			double progressScale = 1.0 / paths.Length;
			double fileReadWeight = 0.5 / paths.Length;
			double dataTranslateWeight = 0.5 / paths.Length;
			
			for(int i = 0; i < paths.Length; i++) {
				using (var parser = new Parser(new StreamReader(paths[i]))) {
					var dataSet = parser.ParseAll();
					double progressOffset = i * progressScale + fileReadWeight;
					yield return progressOffset;
					
					foreach (double p in Methods.ProgressReportHelper(progressOffset, dataTranslateWeight, dataSet.Count)) {
						var data = dataSet.Dequeue().To<IkonComposite>();
						
						switch((string)data.Tag) {
							case ColonyFormulasTag:
								ColonyFormulas = loadColonyFormulas(data);
								break;
							case ConstructableTag:
								Constructables.Add(loadConstructable(data));
								break;
							case DevelopmentTag:
								Technologies.Add(loadTech(data, TechnologyCategory.Development));
								break;
							case ResearchTag:
								Technologies.Add(loadTech(data, TechnologyCategory.Research));
								break;
							default:
								throw new FormatException("Invalid game data object with tag " + data.Tag);
						}
						
						yield return p;
					}
				}
			}
			
			yield return 1;
		}
		
		private ColonyFormulaSet loadColonyFormulas(IkonComposite data)
		{
			return new ColonyFormulaSet(
				data[ColonyMaxPopulation].To<Formula>(),
				data[ColonyOrganization].To<Formula>(),
				loadPopulationActivity(data, ColonyFarming),
				loadPopulationActivity(data, ColonyGardening),
				loadPopulationActivity(data, ColonyMining),
				loadPopulationActivity(data, ColonyDevelopment),
				loadPopulationActivity(data, ColonyIndustry)
			);
		}
		
		private PopulationActivityFormulas loadPopulationActivity(IkonComposite data, string key)
		{
			return new PopulationActivityFormulas(
				data[key].To<IkonComposite>()[PopulationActivityImprovised].To<Formula>(),
				data[key].To<IkonComposite>()[PopulationActivityOrganized].To<Formula>()
			);
		}
		
		private Constructable loadConstructable(IkonComposite data)
		{
			return new Constructable(
				data[GeneralNameKey].To<string>(),
				data[GeneralDescriptionKey].To<string>(),
				false,
				data[GeneralImageKey].To<string>(),
				data[GeneralCodeKey].To<string>(),
				loadPrerquisites(data[GeneralPrerequisitesKey].To<IkonArray>()).ToArray(), 
				siteType(data[ConstructableSiteKey].To<string>()),
				data[ConstructableConditionKey].To<Formula>(),
				data[GeneralCostKey].To<Formula>(),
				data[ConstructableLimitKey].To<Formula>(),
				new object[0] //TODO: make loader
			);
		}
		
		SiteType siteType(string rawData)
		{
			switch(rawData.ToLower())
			{
				case SiteColony:
					return SiteType.Colony;
				case SiteSystem:
					return SiteType.StarSystem;
				default:
					throw new FormatException("Invalid building site type: " + rawData);
			}
		}
		
		private IEnumerable<Prerequisite> loadPrerquisites(IkonArray dataArray)
		{
			for(int i = 0; i < dataArray.Count; i += 2)
				yield return new Prerequisite(
					dataArray[i].To<string>(), 
					dataArray[i + 1].To<Formula>()
				);
		}
		
		private Technology loadTech(IkonComposite data, TechnologyCategory category)
		{
			return new Technology(
				data[GeneralNameKey].To<string>(),
				data[GeneralDescriptionKey].To<string>(),
				data[GeneralImageKey].To<string>(),
				data[GeneralCodeKey].To<string>(),
				data[GeneralCostKey].To<Formula>(),
				loadPrerquisites(data[GeneralPrerequisitesKey].To<IkonArray>()).ToArray(),
				data[TechnologyMaxLevelKey].To<int>(),
				category
			 );
		}
		
		#region Loading tags and keys
		private const string ColonyFormulasTag = "ColonyFormulas";
		private const string ConstructableTag = "Constructable";
		private const string DevelopmentTag = "DevelopmentTopic";
		private const string ResearchTag = "ResearchTopic";
		
		private const string ColonyMaxPopulation = "maxPopulation";
		private const string ColonyOrganization = "organization";
		private const string ColonyDevelopment = "development";
		private const string ColonyFarming = "farming";
		private const string ColonyGardening = "gardening";
		private const string ColonyIndustry = "industry";
		private const string ColonyMining = "mining";

		private const string ConstructableCostKey = "cost";
		private const string ConstructableSiteKey = "site";
		private const string ConstructableConditionKey = "condition";
		private const string ConstructableLimitKey = "turnLimit";
		private const string ConstructableEffectsKey = "effects";
		private const string SiteColony = "colony";
		private const string SiteSystem = "system";
		
		private const string GeneralNameKey = "nameCode";
		private const string GeneralDescriptionKey = "descCode";
		private const string GeneralImageKey = "image";
		private const string GeneralCodeKey = "code";
		private const string GeneralPrerequisitesKey = "prerequisites";
		private const string GeneralCostKey = "cost";
		
		private const string PopulationActivityImprovised = "improvised";
		private const string PopulationActivityOrganized = "organized";
			
		private const string TechnologyMaxLevelKey = "maxLvl";
		#endregion
	}
}
