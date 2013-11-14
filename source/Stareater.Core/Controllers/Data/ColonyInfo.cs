using System;
using Stareater.Galaxy;

namespace Stareater.Controllers.Data
{
	public class ColonyInfo
	{
		public PlayerInfo Owner { get; private set; }
		
		public double Population { get; private set; }
		
		internal ColonyInfo(Colony colony)
		{
			this.Owner = new PlayerInfo(colony.Owner);
			
			this.Population = colony.Population;
		}
	}
}
