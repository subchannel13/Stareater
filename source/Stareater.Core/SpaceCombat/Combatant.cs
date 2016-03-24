using System;
using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy;
using Stareater.GameLogic;
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
		
		public double HitPoints;
		public double ShieldPoints;
		
		public Combatant(Vector2D position, Player owner, ShipGroup ships, DesignStats stats, double[] abilityCharges)
		{
			this.Position = position;
			this.Owner = owner;
			this.Ships = ships;
			this.AbilityCharges = abilityCharges;

			this.HitPoints = stats.HitPoints;
			this.ShieldPoints = stats.ShieldPoints;
		}
	}
}
