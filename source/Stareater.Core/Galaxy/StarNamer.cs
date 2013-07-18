using System;
using System.Collections.Generic;
using Stareater.Localization.StarNames;
using Stareater.Localization;
using Stareater.Utils.Collections;

namespace Stareater.Galaxy
{
	public class StarNamer
	{
		PickList<IStarName> starNames = new PickList<IStarName>();
		
		public StarNamer(int starCount)
		{
			//UNDONE: Currently picks any subset of available names
			//TODO: Make namer respect constellation designations (no beta without alpha)
			Language lang = LocalizationManifest.Get.DefaultLanguage;

			int properNamesCount = lang[ProperStarName.ContextName].KeySet().Count;
			for (int i = 0; i < properNamesCount; i++)
				starNames.Add(new ProperStarName(i));

			int constellationCount = lang[ConstellationStarName.ConstellationsContext].KeySet().Count / 2;
			int designationCount = lang[ConstellationStarName.DesignationContext].KeySet().Count;

			for (int constell = 0; constell < constellationCount; constell++)
				for (int desig = 0; desig < designationCount; desig++)
					starNames.Add(new ConstellationStarName(constell, desig));
		}

		public IStarName NextName()
		{
			return starNames.Take();
		}
	}
}
