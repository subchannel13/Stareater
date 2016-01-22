using System;
using System.Collections.Generic;
using NGenerics.DataStructures.Mathematical;

namespace Stareater.GameLogic
{
	class Conflict
	{
		public Vector2D Position { get; private set; }
		public IEnumerable<FleetMovement> Fleets { get; private set; }
		
		public Conflict(Vector2D position, IEnumerable<FleetMovement> fleets)
		{
			this.Position = position;
			this.Fleets = fleets;
		}
	}
}
