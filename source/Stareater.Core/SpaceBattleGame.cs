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
		public int Turn;
		public Queue<Combatant> PlayOrder { get; private set; }
		
		public SpaceBattleGame(Vector2D location, IEnumerable<FleetMovement> fleets, double startTime)
		{
			this.Combatants = new List<Combatant>();
			this.PlayOrder = new Queue<Combatant>();
			this.Rng = new Random();
			this.Processor = new SpaceBattleProcessor(this);
			this.Turn = 0;
			
			this.Location = location;
			this.Processor.Initialize(fleets, startTime);
		}
	}
}
