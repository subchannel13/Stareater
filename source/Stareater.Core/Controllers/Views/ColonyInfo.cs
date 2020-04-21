using Stareater.Galaxy;
using Stareater.GameLogic;
using System;

namespace Stareater.Controllers.Views
{
	public class ColonyInfo
	{
		public PlayerInfo Owner { get; private set; }
		
		internal Colony Data { get; private set; }
		private readonly ColonyProcessor processor;

		internal ColonyInfo(Colony colony, ColonyProcessor processor)
		{
			this.Data = colony;
			this.processor = processor;
			this.Owner = new PlayerInfo(colony.Owner);
		}
		
		public PlanetInfo Location
		{
			get { return new PlanetInfo(this.Data.Location.Planet, this.PopulationMax); }
		}

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
