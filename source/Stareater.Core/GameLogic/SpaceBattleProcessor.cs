using System;
using System.Collections.Generic;
using NGenerics.DataStructures.Mathematical;
using Stareater.SpaceCombat;

namespace Stareater.GameLogic
{
	class SpaceBattleProcessor
	{
		private readonly SpaceBattleGame game;
		
		public SpaceBattleProcessor(SpaceBattleGame battleGame)
		{
			this.game = battleGame;
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
					this.game.Combatants.Add(new Combatant((int)position.X, (int)position.Y, fleet.OriginalFleet.Owner, shipGroup));
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
			var units = new List<Combatant>(this.game.Combatants);
			
			while(units.Count > 0)
			{
				int i = this.game.Rng.Next(units.Count);
				this.game.PlayOrder.Enqueue(units[i]);
				units.RemoveAt(i);
			}
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
	}
}
