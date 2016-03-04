using System;
using System.Collections.Generic;
using NGenerics.DataStructures.Mathematical;
using Stareater.SpaceCombat;

namespace Stareater.GameLogic
{
	class SpaceBattleProcessor
	{
		private readonly SpaceBattleGame game;
		private readonly MainGame mainGame;
		
		public SpaceBattleProcessor(SpaceBattleGame battleGame, MainGame mainGame)
		{
			this.game = battleGame;
			this.mainGame = mainGame;
		}
		
		#region Initialization
		public void Initialize(IEnumerable<FleetMovement> fleets, double startTime)
		{
			this.initUnits(fleets);
			
			this.game.TurnLimit = (int)Math.Ceiling(50 * (1 - startTime));
		}

		private void initUnits(IEnumerable<FleetMovement> fleets)
		{
			foreach(var fleet in fleets)
			{
				Vector2D position;
				if (fleet.MovementDirection.Magnitude() > 0)
					position = -fleet.MovementDirection * SpaceBattleGame.BattlefieldRadius;
				else
				{
					var angle = game.Rng.NextDouble() * 2 * Math.PI;
					position = new Vector2D(Math.Cos(angle), Math.Sin(angle));
				}
				position = snapPosition(correctPosition(position));
				
				foreach(var shipGroup in fleet.LocalFleet.Ships)
					this.game.Combatants.Add(new Combatant(position, fleet.OriginalFleet.Owner, shipGroup));
			}
		}
		
		private Vector2D correctPosition(Vector2D position)
		{
			var snapped = snapPosition(position);
			double yHeight = (SpaceBattleGame.BattlefieldRadius * 2 - Math.Abs(snapped.X));
			
			if (Math.Abs(snapped.Magnitude()) < 1e-3 && position.Magnitude() > 0)
				return correctPosition(position + position * 0.5 / position.Magnitude());
			else if (
				Math.Abs(snapped.X) > SpaceBattleGame.BattlefieldRadius ||
				snapped.Y < -Math.Ceiling(yHeight / 2.0) || 
			    snapped.Y > Math.Floor(yHeight / 2.0))
				return correctPosition(position - position * 0.5 / position.Magnitude());
				
			return position;
		}
		
		private Vector2D snapPosition(Vector2D position)
		{
			return new Vector2D(
				Math.Round(position.X, MidpointRounding.AwayFromZero),
				Math.Round(position.Y - 0.5, MidpointRounding.AwayFromZero)
			);
		}
		#endregion

		public bool IsOver 
		{
			get { return this.game.Turn >= this.game.TurnLimit; }
		}
		
		public void MakeUnitOrder()
		{
			foreach(var unit in this.game.Combatants)
			{
				var stats = this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design];
				unit.Initiative = this.game.Rng.NextDouble() + stats.CombatSpeed;
			}
			
			var units = new List<Combatant>(this.game.Combatants);
			units.Sort((a, b) => -a.Initiative.CompareTo(b.Initiative));
			
			foreach(var unit in units)
				this.game.PlayOrder.Enqueue(unit);
		}

		public void UnitDone()
		{
			this.game.PlayOrder.Dequeue();
			
			if (this.game.PlayOrder.Count == 0)
			{
				this.game.Turn++;
				//TODO(v0.5) other new turn logic like removing destroyed units
				this.MakeUnitOrder();
			}
		}

		public IEnumerable<Vector2D> ValidMoves 
		{
			get
			{
				var unit = this.game.PlayOrder.Peek();
				
				if (unit.MovementPoints <= 0)
					yield break;
					
				yield return unit.Position + new Vector2D(0, 1);
				yield return unit.Position + new Vector2D(1, 0);
				yield return unit.Position + new Vector2D(1, -1);
				yield return unit.Position + new Vector2D(0, -1);
				yield return unit.Position + new Vector2D(-1, -1);
				yield return unit.Position + new Vector2D(-1, 0);
			}
		}
	}
}
