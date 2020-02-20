using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Ikadn.Ikon.Types;
using Stareater.Localization;

namespace Stareater.Galaxy
{
	public class StartingConditions
	{
		public const int MaxColonies = 8; //TODO(v0.8) move to map assets

		public int Colonies { get; private set; }
		public long Population { get; private set; }
		public ReadOnlyCollection<StartingBuilding> Buildings { get; private set; }

		private readonly string nameKey;

		public StartingConditions(long population, int colonies, IEnumerable<StartingBuilding> buildings, string nameKey)
		{
			this.Colonies = colonies;
			this.Population = population;
			this.Buildings = Array.AsReadOnly(buildings.OrderBy(x => x.Id).ToArray());
			this.nameKey = nameKey;
		}

		public string Name
		{
			get
			{
				return LocalizationManifest.Get.CurrentLanguage["StartingConditions"][nameKey].Text();
			}
		}

		public IkonComposite BuildSaveData()
		{
			var lastGameData = new IkonComposite("StartinConditions")
			{
				{ ColoniesKey, new IkonInteger(this.Colonies) },
				{ PopulationKey, new IkonInteger(this.Population) },
				{ InfrastructureKey, new IkonArray(this.Buildings.Select(x => new IkonArray {
					new IkonText(x.Id),
					new IkonInteger(x.Amount)
				})) },
				{ NameKey, new IkonText(this.nameKey) }
			};

			return lastGameData;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Any exception is considered format fault and falls back to default value")]
		internal static StartingConditions Load(IkonComposite ikstonData)
		{
			var requiredKeys = new[] { PopulationKey, ColoniesKey, InfrastructureKey, NameKey };
			if (!requiredKeys.All(ikstonData.Keys.Contains))
				return null;

			try
			{
				var population = ikstonData[PopulationKey].To<long>();
				var colonies = ikstonData[ColoniesKey].To<int>();
				var infrastructureData = ikstonData[InfrastructureKey].To<IkonArray[]>();
				var nameKey = ikstonData[NameKey].To<string>();

				if (population < 0 || colonies < 0)
					return null;

				var infrastructure = new List<StartingBuilding>();
				foreach(var item in infrastructureData)
				{
					var amount = item[1].To<long>();
					if (amount < 0)
						return null;

					infrastructure.Add(new StartingBuilding(item[0].To<string>(), amount));
				}

				return new StartingConditions(population, colonies, infrastructure, nameKey);
			}
			catch
			{
				return null;
			}
		}

		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			var other = obj as StartingConditions;
			if (other == null)
				return false;

			return this.Colonies == other.Colonies &&
				this.Population == other.Population &&
				this.Buildings.Select(x => x.Id).SequenceEqual(other.Buildings.Select(x => x.Id)) &&
				this.Buildings.Select(x => x.Amount).SequenceEqual(other.Buildings.Select(x => x.Amount));
		}

		public override int GetHashCode()
		{
			return Colonies.GetHashCode() + this.Population.GetHashCode() * 31;
		}
		
		public static bool operator ==(StartingConditions lhs, StartingConditions rhs) {
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (lhs is null || rhs is null)
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(StartingConditions lhs, StartingConditions rhs) {
			return !(lhs == rhs);
		}
		#endregion

		#region Attribute keys
		const string ColoniesKey = "colonies";
		const string PopulationKey = "population";
		const string InfrastructureKey = "buildings";
		const string NameKey = "nameKey";
		#endregion
	}
}
