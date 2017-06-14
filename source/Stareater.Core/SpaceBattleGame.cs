using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using Stareater.SpaceCombat;

namespace Stareater
{
	class SpaceBattleGame
	{
		public static readonly int BattlefieldRadius = 4;
		
		public Random Rng { get; private set; }
		
		public Vector2D Location { get; private set; }
		public int TurnLimit;
		
		public List<Combatant> Combatants { get; private set; }
		public List<Combatant> Retreated { get; private set; }
		public int Turn;
		public Queue<Combatant> PlayOrder { get; private set; }
		
		public CombatPlanet[] Planets { get; private set; }
		
		public SpaceBattleGame(Vector2D location, MainGame mainGame)
		{
			this.Combatants = new List<Combatant>();
			this.Planets = new CombatPlanet[mainGame.States.Planets.At[mainGame.States.Stars.At[location]].Count];
			this.PlayOrder = new Queue<Combatant>();
			this.Retreated = new List<Combatant>();
			this.Rng = new Random();
			this.Turn = 0;
			
			this.Location = location;
		}
	}
}
