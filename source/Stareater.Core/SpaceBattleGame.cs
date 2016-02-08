using System;
using System.Collections.Generic;
using NGenerics.DataStructures.Mathematical;
using Stareater.GameLogic;

namespace Stareater
{
	class SpaceBattleGame
	{
		public Vector2D Position { get; private set; }
		public IEnumerable<FleetMovement> Fleets { get; private set; }
		
		public SpaceBattleGame(Vector2D position, IEnumerable<FleetMovement> fleets)
		{
			this.Position = position;
			this.Fleets = fleets;
		}
	}
}
