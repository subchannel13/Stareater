using NGenerics.DataStructures.Mathematical;
using Stareater.GameLogic.Combat;
using Stareater.Players;

namespace Stareater.SpaceCombat
{
	class Projectile
	{
		public Player Owner { get; private set; }
		public long Count { get; set; }
		public AbilityStats Stats { get; private set; }
		public Combatant Target { get; private set; }

		public Vector2D Position;
		public double MovementPoints = 1;

		public Projectile(Player owner, long count, AbilityStats stats, Combatant target, Vector2D position)
		{
			this.Owner = owner;
			this.Count = count;
			this.Stats = stats;
			this.Target = target;
			this.Position = position;
		}
	}
}
