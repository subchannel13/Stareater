using System;
using System.Collections.Generic;
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
		public double[] AbilityAmmo;
		public double[] AbilityCharges;
		
		public double HitPoints;
		public double ShieldPoints;
		public HashSet<Player> CloakedFor = new HashSet<Player>();
		
		public Combatant(Vector2D position, Player owner, ShipGroup ships, DesignStats stats, double[] abilityAmmo, double[] abilityCharges)
		{
			this.Position = position;
			this.Owner = owner;
			this.Ships = ships;
			this.AbilityAmmo = abilityAmmo;
			this.AbilityCharges = abilityCharges;

			this.HitPoints = stats.HitPoints - ships.Damage;
			this.ShieldPoints = stats.ShieldPoints;
		}
	}
}
