using System;
using Stareater.Galaxy;

namespace Stareater.Controllers.Data
{
	public class IdleFleetInfo
	{
		public PlayerInfo Owner { get; private set; }
		public StarData Location { get; private set; }
		
		internal IdleFleetInfo(IdleFleet fleet)
		{
			this.Location = fleet.Location;
			this.Owner = new PlayerInfo(fleet.Owner);
		}
	}
}
