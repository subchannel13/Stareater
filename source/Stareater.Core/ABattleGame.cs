using System;
using System.Collections.Generic;
using Stareater.SpaceCombat;
using Stareater.Utils;

namespace Stareater
{
	abstract class ABattleGame
	{
		//TODO(later) redesign how RNG works in the game
		public Random Rng { get; private set; }
		
		public Vector2D Location { get; private set; }
		public int TurnLimit;
		
		public List<Combatant> Combatants { get; private set; }
		public CombatPlanet[] Planets { get; private set; }
		public List<Combatant> Retreated { get; private set; }
		public int Turn;

		protected ABattleGame(Random rng, Vector2D location, int turnLimit, List<Combatant> combatants, CombatPlanet[] planets, List<Combatant> retreated, int turn)
		{
			Rng = rng;
			Location = location;
			TurnLimit = turnLimit;
			Combatants = combatants;
			Planets = planets;
			Retreated = retreated;
			Turn = turn;
		}
	}
}
