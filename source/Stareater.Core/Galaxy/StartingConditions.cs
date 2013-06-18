using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.AppData;
using Ikadn;
using Ikadn.Ikon.Types;

namespace Stareater.Galaxy
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

		public StartingConditions(IkonComposite ikstonData) :
			this(ikstonData[PopulationKey].To<long>(),
				ikstonData[ColoniesKey].To<int>(),
				ikstonData[InfrastructureKey].To<long>(),
				ikstonData[NameKey].To<string>())
		{ }

		public string Name
		{
			get
			{
				return Settings.Get.Language["StartingConditions"][nameKey].Text();
			}
		}

		public IkonComposite BuildSaveData()
		{
			IkonComposite lastGameData = new IkonComposite(ClassName);
			lastGameData.Add(ColoniesKey, new IkonNumeric(Colonies));
			lastGameData.Add(PopulationKey, new IkonNumeric(Population));
			lastGameData.Add(InfrastructureKey, new IkonNumeric(Infrastructure));
			
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
