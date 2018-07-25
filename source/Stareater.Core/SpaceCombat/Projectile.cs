using Stareater.GameLogic.Combat;
using Stareater.Players;
using Stareater.Utils;

namespace Stareater.SpaceCombat
{
	class Projectile
	{
		public Player Owner { get; private set; }
		public long Count { get; set; }
		public AbilityStats Stats { get; private set; }
		public Combatant Target { get; set; }

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
