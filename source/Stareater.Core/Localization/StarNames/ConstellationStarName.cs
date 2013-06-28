using System;

namespace Stareater.Localization.StarNames
{
	public class ConstellationStarName : IStarName
	{
		const string ConstellationsContext = "ConstellationNames";
		const string ConstellationKeyPrefix = "constellation";
		const string NominativeSufix = "Nom";
		const string GenitiveSufix = "Gen";
		const string DesignationContext = "starDesignations";
		const string DesignationKeyPrefix = "designation";
		
		const int NoDesignation = -1;
		
		int constellation;
		int designation;
		
		public ConstellationStarName(int constellationNameIndex, int designationIndex)
		{
			this.constellation = constellationNameIndex;
			this.designation = designationIndex;
		}
		
		public ConstellationStarName(int constellationNameIndex)
		{
			this.constellation = constellationNameIndex;
			this.designation = NoDesignation;
		}
		
		public string Name(Language language)
		{
			if (designation == NoDesignation)
				return language[ConstellationsContext][ConstellationKeyPrefix + constellation.ToString() + NominativeSufix].Text();
			else
				return language[DesignationContext][DesignationKeyPrefix + designation.ToString()].Text() +
					" " +
					language[ConstellationsContext][ConstellationKeyPrefix + constellation.ToString() + GenitiveSufix].Text();
		}
	}
}
