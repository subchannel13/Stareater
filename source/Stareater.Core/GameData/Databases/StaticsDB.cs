using System;
using System.Collections.Generic;
using System.IO;
using Ikadn.Ikon.Types;
using Stareater.AppData.Expressions;
using Stareater.GameData.Databases.Tables;
using Stareater.GameData.Reading;
using Stareater.Utils;

namespace Stareater.GameData.Databases
{
	internal class StaticsDB
	{
		public TechnologiesCollection Technologies { get; private set; }
		
		private const string DevelopmentTag = "DevelopmentTopic";
		private const string ResearchTag = "ResearchTopic";
		
		private const string TechnologyNameKey = "nameCode";
		private const string TechnologyDescriptionKey = "descCode";
		private const string TechnologyImageKey = "image";
		
		private const string TechnologyCodeKey = "code";
		private const string TechnologyCostKey = "cost";
		private const string TechnologyPrerequisitesKey = "prerequisites";
		private const string TechnologyMaxLevelKey = "maxLvl";
			
		public StaticsDB()
		{
			this.Technologies = new TechnologiesCollection();
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
							case DevelopmentTag:
								Technologies.Add(loadTech(data, TechnologyCategory.Development));
								break;
							case ResearchTag:
								Technologies.Add(loadTech(data, TechnologyCategory.Research));
								break;
							default:
								throw new FormatException("Invalid game data object with tag " + data.Tag);
						}
					}
				}
			}
			
			yield return 1;
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
				data[TechnologyNameKey].To<string>(),
				data[TechnologyDescriptionKey].To<string>(),
				data[TechnologyImageKey].To<string>(),
				data[TechnologyCodeKey].To<string>(),
				data[TechnologyCostKey].To<Formula>(),
				loadPrerquisites(data[TechnologyPrerequisitesKey].To<IkonArray>()),
             	data[TechnologyMaxLevelKey].To<int>(),
             	category
             );
		}
	}
}
