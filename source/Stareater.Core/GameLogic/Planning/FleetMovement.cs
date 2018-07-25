using Stareater.Galaxy;
using Stareater.Utils;

namespace Stareater.GameLogic.Planning
{
	class FleetMovement
	{
		public Fleet OriginalFleet { get; private set; }
		public Fleet LocalFleet { get; private set; }
		public double ArrivalTime { get; private set; }
		public double DepartureTime { get; private set; }
		public Vector2D MovementDirection { get; private set; }
		
		public FleetMovement(Fleet originalFleet, Fleet localFleet, double arrivalTime, double departureTime, Vector2D movementDirection)
		{
			this.OriginalFleet = originalFleet;
			this.LocalFleet = localFleet;
			this.ArrivalTime = arrivalTime;
			this.DepartureTime = departureTime;
			this.MovementDirection = (!movementDirection.IsZero) ? movementDirection.Unit : movementDirection;
		}
	}
}
