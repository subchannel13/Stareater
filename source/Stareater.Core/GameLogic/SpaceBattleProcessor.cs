using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using Stareater.SpaceCombat;
using Stareater.Utils;

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
				{
					var designStats = mainGame.Derivates.Of(shipGroup.Design.Owner).DesignStats[shipGroup.Design];
					var abilities = designStats.Abilities.Select(x => x.Quantity * (double)shipGroup.Quantity);
					this.game.Combatants.Add(new Combatant(position, fleet.OriginalFleet.Owner, shipGroup, designStats, abilities.ToArray()));
				}
			}
		}
		
		private Vector2D correctPosition(Vector2D position)
		{
			var snapped = snapPosition(position);
			double yHeight = (SpaceBattleGame.BattlefieldRadius * 2 - Math.Abs(snapped.X));
			
			if (Math.Abs(snapped.Magnitude()) < 1e-3 && position.Magnitude() > 0)
				return correctPosition(position + position * 0.5 / position.Magnitude());
			else if (!Methods.InsideHexGrid(snapped, SpaceBattleGame.BattlefieldRadius))
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

		#region Unit actions
		public void MoveTo(Vector2D destination)
		{
			if (!this.ValidMoves(this.game.PlayOrder.Peek()).Contains(destination))
				return;
			
			var unit = this.game.PlayOrder.Peek();
			
			if (Methods.InsideHexGrid(destination, SpaceBattleGame.BattlefieldRadius))
			{
				unit.Position = destination;
				unit.MovementPoints -= 1 / mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design].CombatSpeed;
			}
			else
			{
				this.game.Retreated.Add(unit);
				this.game.Combatants.Remove(unit);
				this.UnitDone();
			}
		}
		
		public void UnitDone()
		{
			this.game.PlayOrder.Dequeue();
			
			if (this.game.PlayOrder.Count == 0)
			{
				this.game.Turn++;
				foreach(var unit in this.game.Combatants)
				{
					unit.MovementPoints += 1;
					unit.MovementPoints = Math.Min(unit.MovementPoints, 1);
				}
				//TODO(v0.5) other new turn logic like removing destroyed units
				this.MakeUnitOrder();
			}
		}

		public void UseAbility(int index, double quantity, Combatant target)
		{
			var unit = this.game.PlayOrder.Peek();
			var abilityStats = this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design].Abilities[index];
			var chargesLeft = quantity;
			
			if (!Methods.InsideHexGrid(target.Position - unit.Position, abilityStats.Range))
				return;
			
			if (abilityStats.IsInstantDamage)
				chargesLeft = this.applyDamage(abilityStats, quantity, target);
			
			//TODO(v0.5) reduce spent charges
			throw new NotImplementedException();
		}
		#endregion
		
		public IEnumerable<Vector2D> ValidMoves(Combatant unit)
		{
			if (unit.MovementPoints <= 0)
				yield break;
			
			var yOffset = (int)Math.Abs(unit.Position.X) % 2 == 0 ? 0 : 1;
				
			yield return unit.Position + new Vector2D(0, 1);
			yield return unit.Position + new Vector2D(1, 0 + yOffset);
			yield return unit.Position + new Vector2D(1, -1 + yOffset);
			yield return unit.Position + new Vector2D(0, -1);
			yield return unit.Position + new Vector2D(-1, -1 + yOffset);
			yield return unit.Position + new Vector2D(-1, 0 + yOffset);
		}
		
		private double applyDamage(AbilityStats abilityStats, double quantity, Combatant target)
		{
			var targetStats = this.mainGame.Derivates.Of(target.Owner).DesignStats[target.Ships.Design];
			
			//TODO(v0.5) do all calculations
			return quantity;
		}
	}
}
