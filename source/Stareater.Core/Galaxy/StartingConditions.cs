using System;
using System.Linq;
using Stareater.AppData;
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
			this.nameKey = nameKey; //TODO(v0.5) deduce the name from stats
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
			var lastGameData = new IkonComposite(ClassName);
			lastGameData.Add(ColoniesKey, new IkonInteger(Colonies));
			lastGameData.Add(PopulationKey, new IkonInteger(Population));
			lastGameData.Add(InfrastructureKey, new IkonInteger(Infrastructure));
			lastGameData.Add(NameKey, new IkonText(nameKey));
			
			return lastGameData;
		}

		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			var other = obj as StartingConditions;
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
		
		public static bool operator ==(StartingConditions lhs, StartingConditions rhs) {
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(StartingConditions lhs, StartingConditions rhs) {
			return !(lhs == rhs);
		}
		#endregion

		#region Attribute keys
		const string ClassName = "StartinConditions";
		const string ColoniesKey = "colonies";
		const string PopulationKey = "population";
		const string InfrastructureKey = "infrastructure";
		const string NameKey = "nameKey";
		#endregion
	}
}
