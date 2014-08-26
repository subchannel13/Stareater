using System;
using Ikadn.Ikon.Types;

namespace Stareater.Localization.StarNames
{
	public class ConstellationStarName : IStarName
	{
		internal const string ConstellationsContext = "ConstellationNames";
		private const string ConstellationKeyPrefix = "constellation";
		private const string NominativeSufix = "Nom";
		private const string GenitiveSufix = "Gen";
		internal const string DesignationContext = "StarDesignations";
		private const string DesignationKeyPrefix = "designation";
		
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
		
		public string ToText(Language language)
		{
			if (designation == NoDesignation)
				return language[ConstellationsContext][ConstellationKeyPrefix + constellation.ToString() + NominativeSufix].Text();
			else
				return language[DesignationContext][DesignationKeyPrefix + designation.ToString()].Text() +
					" " +
					language[ConstellationsContext][ConstellationKeyPrefix + constellation.ToString() + GenitiveSufix].Text();
		}

		#region Saving
		public Ikadn.IkadnBaseObject Save()
		{
			IkonComposite data = new IkonComposite(SaveTag);
			data.Add(ConstellationKey, new IkonInteger(this.constellation));
			data.Add(DesignationKey, new IkonInteger(this.designation));

			return data;
		}

		public static IStarName Load(IkonComposite rawData)
		{
			return new ConstellationStarName(
				rawData[ConstellationKey].To<int>(),
				rawData[DesignationKey].To<int>()
			);
		}
		
		public const string SaveTag = "Constell";
		private const string ConstellationKey = "const";
		private const string DesignationKey = "desig";
		#endregion
	}
}
