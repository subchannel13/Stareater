using NGenerics.DataStructures.Mathematical;

namespace Stareater.SpaceCombat
{
	class Projectile
	{
		public Combatant Target { get; private set; }

		public Vector2D Position;
		public double MovementPoints = 1;

		public Projectile(Combatant target, Vector2D position)
		{
			this.Target = target;
			this.Position = position;
		}
	}
}
