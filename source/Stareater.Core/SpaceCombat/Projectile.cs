using NGenerics.DataStructures.Mathematical;
using Stareater.GameData.Ships;
using Stareater.GameLogic;
using Stareater.Players;

namespace Stareater.SpaceCombat
{
	class Projectile
	{
		public Player Owner { get; private set; }
		public AbilityStats Stats { get; private set; }
		public Combatant Target { get; private set; }

		public Vector2D Position;
		public long Count { get; private set; }
		public double MovementPoints = 1;

		public Projectile(Player owner, AbilityStats stats, Combatant target, Vector2D position, long count)
		{
			this.Owner = owner;
			this.Stats = stats;
			this.Target = target;
			this.Position = position;
			this.Count = count;
		}
	}
}
