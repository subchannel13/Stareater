using System;
using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy;

namespace Stareater.SpaceCombat
{
	class CombatPlanet
	{
		public Colony Colony { get; private set; }
		public Vector2D Position { get; private set; }
		
		public CombatPlanet(Colony colony, Vector2D position)
		{
			this.Colony = colony;
			this.Position = position;
		}
	}
}
