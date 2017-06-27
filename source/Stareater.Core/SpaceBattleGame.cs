using NGenerics.DataStructures.Mathematical;
using Stareater.SpaceCombat;
using System;
using System.Collections.Generic;

namespace Stareater
{
	class SpaceBattleGame : ABattleGame
	{
		public static readonly int BattlefieldRadius = 4;

		public Queue<Combatant> PlayOrder { get; private set; }

		public SpaceBattleGame(Vector2D location, MainGame mainGame) : 
			base(
				new Random(), location, 0, new List<Combatant>(),
				new CombatPlanet[mainGame.States.Planets.At[mainGame.States.Stars.At[location]].Count],
				new List<Combatant>(), 0
			)
		{
			this.PlayOrder = new Queue<Combatant>();
		}
	}
}
