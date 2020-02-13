using Stareater.Utils;
using Stareater.Utils.StateEngine;

namespace Stareater.Localization.StarNames
{
	[StateTypeAttribute(saveTag: SaveTag)]
	class ConstellationStarName : IStarName
	{
		internal const string ConstellationsContext = "ConstellationNames";
		private const string ConstellationKeyPrefix = "constellation";
		private const string NominativeSufix = "Nom";
		private const string GenitiveSufix = "Gen";
		internal const string DesignationContext = "StarDesignations";
		private const string DesignationKeyPrefix = "designation";
		
		const int NoDesignation = -1;
		
		[StatePropertyAttribute]
		private int constellation { get; set; }
		[StatePropertyAttribute]
		private int designation { get; set; }

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

		private ConstellationStarName()
		{ }

		public string ToText(Language language)
		{
			if (designation == NoDesignation)
				return language[ConstellationsContext][ConstellationKeyPrefix + constellation.ToStringInvariant() + NominativeSufix].Text();
			else
				return language[DesignationContext][DesignationKeyPrefix + designation.ToStringInvariant()].Text() +
					" " +
					language[ConstellationsContext][ConstellationKeyPrefix + constellation.ToStringInvariant() + GenitiveSufix].Text();
		}

		public const string SaveTag = "Constell";
	}
}