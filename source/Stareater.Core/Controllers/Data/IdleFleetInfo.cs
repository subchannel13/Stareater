using System;
using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy;
using Stareater.Utils;

namespace Stareater.Controllers.Data
{
	public class IdleFleetInfo
	{
		public PlayerInfo Owner { get; private set; }
		public StarData Location { get; private set; }
		public Vector2D VisualPosition { get; private set; }
		
		internal IdleFleetInfo(IdleFleet fleet, Methods.VisualPositionFunc visualPositionFunc)
		{
			this.Location = fleet.Location;
			this.Owner = new PlayerInfo(fleet.Owner);
			
			this.VisualPosition = visualPositionFunc(fleet.Location.Position);
		}
	}
}
