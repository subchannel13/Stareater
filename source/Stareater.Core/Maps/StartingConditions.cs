using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.AppData;
using Ikadn;
using Ikadn.Ikon.Values;

namespace Stareater.Maps
{
	public class StartingConditions
	{
		public int Colonies { get; private set; }
		public long Population { get; private set; }
		public long Infrastructure { get; private set; }

		private string nameKey;

		public StartingConditions(long population, int colonies, long infrastructure, string nameKey)
		{
			this.Colonies = colonies;
			this.Population = population;
			this.Infrastructure = infrastructure;
			this.nameKey = nameKey;
		}

		public StartingConditions(ObjectValue ikstonData) :
			this((ikstonData[PopulationKey] as NumericValue).To<long>(),
				(ikstonData[ColoniesKey] as NumericValue).To<int>(),
				(ikstonData[InfrastructureKey] as NumericValue).To<long>(),
				(ikstonData[NameKey] as TextValue).To<string>())
		{ }

		public string Name
		{
			get
			{
				return Settings.Get.Language["StartingConditions"][nameKey].Text();
			}
		}

		public ObjectValue BuildSaveData()
		{
			ObjectValue lastGameData = new ObjectValue(ClassName);
			lastGameData.Add(ColoniesKey, new NumericValue(Colonies));
			lastGameData.Add(PopulationKey, new NumericValue(Population));
			lastGameData.Add(InfrastructureKey, new NumericValue(Infrastructure));
			
			return lastGameData;
		}

		public override bool Equals(object obj)
		{
			StartingConditions other = obj as StartingConditions;
			if (other == null)
				return false;

			return this.Colonies == other.Colonies &&
				this.Infrastructure == other.Infrastructure &&
				this.Population == other.Population;
		}

		public override int GetHashCode()
		{
			return Colonies.GetHashCode() + Population.GetHashCode() * 31 + Infrastructure.GetHashCode() * 967;
		}

		#region Attribute keys
		const string ClassName = "StartinConditions";
		const string ColoniesKey = "colonies";
		const string PopulationKey = "population";
		const string InfrastructureKey = "infrastructure";
		const string NameKey = "nameKey";
		#endregion
	}
}
