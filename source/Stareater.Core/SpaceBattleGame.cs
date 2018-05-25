using NGenerics.DataStructures.Mathematical;
using Stareater.SpaceCombat;
using Stareater.Utils.Collections;
using System;
using System.Collections.Generic;

namespace Stareater
{
	class SpaceBattleGame : ABattleGame
	{
		public static readonly int BattlefieldRadius = 4;

		public Queue<Combatant> PlayOrder { get; private set; }
		public PendableSet<Projectile> Projectiles { get; private set; }

		public SpaceBattleGame(Vector2D location, int turnLimit, MainGame mainGame) : 
			base(
				new Random(), location, turnLimit, new List<Combatant>(),
				new CombatPlanet[mainGame.States.Planets.At[mainGame.States.Stars.At[location]].Count],
				new List<Combatant>(), 0
			)
		{
			this.PlayOrder = new Queue<Combatant>();
			this.Projectiles = new PendableSet<Projectile>();
		}
	}
}
