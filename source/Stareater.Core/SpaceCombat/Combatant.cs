using System;
using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.SpaceCombat
{
	class Combatant
	{
		public Player Owner { get; private set; }
		public ShipGroup Ships { get; private set; }
		
		public Vector2D Position;
		public double Initiative;
		public double MovementPoints = 1;
		public double[] AbilityCharges;
		
		public Combatant(Vector2D position, Player owner, ShipGroup ships, double[] abilityCharges)
		{
			this.Position = position;
			this.Owner = owner;
			this.Ships = ships;
			this.AbilityCharges = abilityCharges;
		}
	}
}
