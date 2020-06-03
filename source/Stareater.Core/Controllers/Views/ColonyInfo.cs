using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.GameLogic;
using System;

namespace Stareater.Controllers.Views
{
	public class ColonyInfo
	{
		public PlayerInfo Owner { get; private set; }
		
		internal Colony Data { get; private set; }
		private readonly ColonyProcessor processor;
		private readonly PlanetIntelligence intel;

		internal ColonyInfo(Colony colony, ColonyProcessor processor, PlanetIntelligence intel)
		{
			this.Data = colony;
			this.processor = processor;
			this.intel = intel;
			this.Owner = new PlayerInfo(colony.Owner);
		}

		public PlanetInfo Location => new PlanetInfo(this.Data.Location.Planet, this.PopulationMax, this.intel);

		public double Population => this.Data.Population;
		public double PopulationMax => this.processor.MaxPopulation;

		public double ExtraStats(string statName)
		{
			if (statName == null)
				throw new ArgumentNullException(nameof(statName));
			
			var key = statName.ToUpperInvariant();
			if (!this.processor.ExtraStats.ContainsKey(key))
				throw new ArgumentOutOfRangeException(nameof(statName));

			return this.processor.ExtraStats[key];
		}
	}
}
