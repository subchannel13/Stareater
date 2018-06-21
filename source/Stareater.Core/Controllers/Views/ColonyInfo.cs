using Stareater.Galaxy;

namespace Stareater.Controllers.Views
{
	public class ColonyInfo
	{
		public PlayerInfo Owner { get; private set; }
		public double Population { get; private set; }
		
		internal Colony Data { get; private set; }
		
		internal ColonyInfo(Colony colony)
		{
			this.Data = colony;
			
			this.Owner = new PlayerInfo(colony.Owner);
			this.Population = colony.Population;
		}
		
		public PlanetInfo Location
		{
			get { return new PlanetInfo(this.Data.Location.Planet); }
		}
	}
}
