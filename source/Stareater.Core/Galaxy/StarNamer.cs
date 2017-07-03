using System;
using System.Collections.Generic;
using Stareater.Localization.StarNames;
using Stareater.Localization;
using Stareater.Utils.Collections;

namespace Stareater.Galaxy
{
	public class StarNamer
	{
		private readonly PickList<IStarName> starNames = new PickList<IStarName>();
		
		public StarNamer(int starCount, Random random)
		{
			var lang = LocalizationManifest.Get.DefaultLanguage;

			var constellationCount = lang[ConstellationStarName.ConstellationsContext].KeySet().Count / 2;
			var designationCount = LocalizationManifest.Get.DefaultLanguage[ConstellationStarName.DesignationContext].KeySet().Count;
			var usedDesignations = new Dictionary<int, int>();
			var constellationMaxNames = new Dictionary<int, int>();
			var constellationPool = new PickList<int>(random);

			for (int i = 0; i <= constellationCount; i++)
			{
				usedDesignations.Add(i, 0);
				constellationMaxNames.Add(i, designationCount - 1);
				constellationPool.Add(i);
			}
			constellationMaxNames[constellationCount] = lang[ProperStarName.ContextName].KeySet().Count - 1;
			
			for(int i = 0; i < starCount; i++)
				usedDesignations[constellationPool.PickOrTake(x => usedDesignations[x] >= constellationMaxNames[x])]++;
			
			// Proper names
			var namePool = new PickList<int>();
			for(int i = 0; i < lang[ProperStarName.ContextName].KeySet().Count; i++)
				namePool.Add(i);
			for(int i = 0; i < usedDesignations[constellationCount]; i++)
				this.starNames.Add(new ProperStarName(namePool.Take()));
			
			// Constellation names
			usedDesignations.Remove(constellationCount);
			foreach(var constellation in usedDesignations)
				for(int i = 0; i < constellation.Value; i++)
					this.starNames.Add((constellation.Value == 1) ? 
					                   (IStarName)new ConstellationStarName(constellation.Key) :
					                   (IStarName)new ConstellationStarName(constellation.Key, i)
					);
		}

		public IStarName NextName()
		{
			return this.starNames.Take();
		}
	}
}
