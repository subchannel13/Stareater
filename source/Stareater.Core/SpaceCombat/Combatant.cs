using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.Players;
using Stareater.GameLogic.Combat;
using Stareater.Utils;

namespace Stareater.SpaceCombat
{
	class Combatant
	{
		public Fleet OriginalFleet { get; private set; }
		public ShipGroup Ships { get; private set; }
		
		public Vector2D Position;
		public double Initiative;
		public double MovementPoints = 1;
		public double[] AbilityAmmo;
		public double[] AbilityCharges;
		
		public double TopArmor;
		public double TopShields;
		public double RestArmor;
		public double RestShields;
		public HashSet<Player> CloakedFor = new HashSet<Player>();
		
		public Combatant(Vector2D position, Fleet originalFleet, ShipGroup ships, DesignStats stats, double[] abilityAmmo, double[] abilityCharges)
		{
			this.Position = position;
			this.OriginalFleet = originalFleet;
			this.Ships = ships;
			this.AbilityAmmo = abilityAmmo;
			this.AbilityCharges = abilityCharges;

			this.TopArmor = stats.HitPoints - ships.Damage / ships.Quantity;
			this.TopShields = stats.ShieldPoints;
			this.RestArmor = stats.HitPoints * ships.Quantity - ships.Damage - this.TopArmor;
			this.RestShields = stats.ShieldPoints * (ships.Quantity - 1);
		}

		public Player Owner => this.OriginalFleet.Owner;
	}
}
