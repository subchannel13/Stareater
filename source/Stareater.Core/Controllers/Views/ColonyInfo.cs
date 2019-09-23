using Stareater.Galaxy;
using Stareater.GameLogic;

namespace Stareater.Controllers.Views
{
	public class ColonyInfo
	{
		public PlayerInfo Owner { get; private set; }
		
		internal Colony Data { get; private set; }
		private ColonyProcessor processor;

		internal ColonyInfo(Colony colony, ColonyProcessor processor)
		{
			this.Data = colony;
			this.processor = processor;
			this.Owner = new PlayerInfo(colony.Owner);
		}
		
		public PlanetInfo Location
		{
			get { return new PlanetInfo(this.Data.Location.Planet); }
		}

		public double Population => this.Data.Population;
		public double PopulationMax => this.processor.MaxPopulation;
	}
}
