using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using Stareater.GameLogic;
using Stareater.SpaceCombat;

namespace Stareater
{
	class SpaceBattleGame
	{
		public static readonly int BattlefieldRadius = 4;
		
		public Random Rng { get; private set; }
		public SpaceBattleProcessor Processor = null;
		
		public Vector2D Location { get; private set; }
		public int TurnLimit;
		
		public List<Combatant> Combatants { get; private set; }
		public List<Combatant> Retreated { get; private set; }
		public int Turn;
		public Queue<Combatant> PlayOrder { get; private set; }
		
		public Vector2D[] PlanetPositions { get; private set; }
		
		public SpaceBattleGame(Vector2D location, IEnumerable<FleetMovement> fleets, double startTime, MainGame mainGame)
		{
			this.Combatants = new List<Combatant>();
			this.PlanetPositions = new Vector2D[mainGame.States.Planets.At(mainGame.States.Stars.At(location)).Count];
			this.PlayOrder = new Queue<Combatant>();
			this.Retreated = new List<Combatant>();
			this.Rng = new Random();
			this.Processor = new SpaceBattleProcessor(this, mainGame);
			this.Turn = 0;
			
			this.Location = location;
			this.Processor.Initialize(fleets, startTime);
		}
	}
}
