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
		
		public Vector2D Location { get; private set; }
		public IEnumerable<Combatant> Combatants { get; private set; }
		public Random Rng { get; private set; }
		
		public SpaceBattleGame(Vector2D location, IEnumerable<FleetMovement> fleets)
		{
			this.Rng = new Random();
			
			this.Location = location;
			this.Combatants = fleetsToFigures(fleets).ToArray();
		}
		
		private IEnumerable<Combatant> fleetsToFigures(IEnumerable<FleetMovement> fleets)
		{
			foreach(var fleet in fleets)
			{
				Vector2D position;
				if (fleet.MovementDirection.Magnitude() > 0)
					position = -fleet.MovementDirection * BattlefieldRadius;
				else
				{
					var angle = this.Rng.NextDouble() * 2 * Math.PI;
					position = new Vector2D(Math.Cos(angle), Math.Sin(angle));
				}
				position = snapPosition(correctPosition(position));
				
				yield return new Combatant((int)position.X, (int)position.Y, fleet.OriginalFleet.Owner);
			}
		}

		private Vector2D correctPosition(Vector2D position)
		{
			var snapped = snapPosition(position);
			double yHeight = (BattlefieldRadius * 2 - Math.Abs(snapped.X));
			
			if (Math.Abs(snapped.Magnitude()) < 1e-3 && position.Magnitude() > 0)
				return correctPosition(position + position * 0.5 / position.Magnitude());
			else if (Math.Abs(snapped.X) > BattlefieldRadius || snapped.Y < -Math.Ceiling(yHeight / 2.0) || snapped.Y > Math.Floor(yHeight / 2.0))
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
	}
}
