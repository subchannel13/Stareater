using System;
using System.Collections.Generic;
using System.IO;
using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using Stareater.GameData.Databases.Tables;
using Stareater.Utils;

namespace Stareater.GameData.Databases
{
	internal class StaticsDB
	{
		public TechnologiesCollection Technologies { get; private set; }
		
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
				using (var parser = new IkonParser(new StreamReader(paths[i]))) {
					var dataSet = parser.ParseAll();
					double progressOffset = i * progressScale + fileReadWeight;
					yield return progressOffset;
					
					foreach (double p in Methods.ProgressReportHelper(progressOffset, dataTranslateWeight, dataSet.Count)) {
						var data = dataSet.Dequeue().To<IkonComposite>();
					}
				}
			}
			
			yield return 1;
		}
	}
}
