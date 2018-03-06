using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy;
using Stareater.SpaceCombat;
using Stareater.Utils;
using Stareater.Players;
using Stareater.GameLogic.Combat;
using Stareater.GameLogic.Planning;

namespace Stareater.GameLogic
{
	class SpaceBattleProcessor : ACombatProcessor
	{
		protected readonly SpaceBattleGame game;

		public SpaceBattleProcessor(SpaceBattleGame battleGame, MainGame mainGame) : base(mainGame)
		{
			this.game = battleGame;
		}

		protected override ABattleGame battleGame 
		{
			get { return this.game; }
		}

		#region Initialization
		public void Initialize(IEnumerable<FleetMovement> fleets, double startTime)
		{
			this.initUnits(fleets);
			this.initBodies();
			this.makeUnitOrder();
			
			//TODO(v0.7) move to state constructor
			this.game.TurnLimit = (int)Math.Ceiling(50 * (1 - startTime));
		}

		private void initBodies()
		{
			double maxPlanets = this.mainGame.States.Planets.Max(x => x.Position) - 1;
			var star = mainGame.States.Stars.At[game.Location];
			var planets = this.mainGame.States.Planets.At[star];
			var colonies = this.mainGame.States.Colonies.AtStar[star];
			
			var rings = new Dictionary<int, List<Vector2D>>();
			for(int i = 0; i <= SpaceBattleGame.BattlefieldRadius; i++)
				rings[i] = new List<Vector2D>();
			for(int y = -SpaceBattleGame.BattlefieldRadius; y <= SpaceBattleGame.BattlefieldRadius; y++)
				for(int x = -SpaceBattleGame.BattlefieldRadius; x <= SpaceBattleGame.BattlefieldRadius; x++)
				{
					int distance = (int)Methods.HexDistance(new Vector2D(x, y));
					if (distance <= SpaceBattleGame.BattlefieldRadius)
						rings[distance].Add(new Vector2D(x, y));
				}
			    
			var unoccupied = new Dictionary<int, List<Vector2D>>();
			for(int i = 0; i <= SpaceBattleGame.BattlefieldRadius; i++)
				unoccupied[i] = new List<Vector2D>(rings[i]);
			
			for(int i = 0; i < planets.Count; i++)
			{
				var ring = (int)Math.Floor(Methods.Lerp(
					(planets[i].Position - 1) / maxPlanets, 
					1, 
					SpaceBattleGame.BattlefieldRadius + 0.9999
				));
				Vector2D position;
				
				if (unoccupied[ring].Count > 0)
				{
					int slot = this.game.Rng.Next(unoccupied[ring].Count);
					position = unoccupied[ring][slot];
					
					unoccupied[ring].RemoveAt(slot);
				}
				else
					position = rings[ring][this.game.Rng.Next(rings[ring].Count)];
				
				this.game.Planets[i] = new CombatPlanet(
					colonies.FirstOrDefault(x => x.Location.Planet == planets[i]),
					planets[i],
					position,
					this.mainGame.Statics.ColonyFormulas.PopulationHitPoints.Evaluate(null) //TODO(later) pass relevant variables
				);
			}
		}
		
		private void initUnits(IEnumerable<FleetMovement> fleets)
		{
			foreach(var fleet in fleets)
			{
				Vector2D position;
				if (fleet.MovementDirection.Magnitude() > 0)
					position = -fleet.MovementDirection * (SpaceBattleGame.BattlefieldRadius + 0.25);
				else
				{
					var angle = game.Rng.NextDouble() * 2 * Math.PI;
					position = new Vector2D(Math.Cos(angle), Math.Sin(angle));
				}
				position = correctPosition(position);
				
				foreach(var shipGroup in fleet.LocalFleet.Ships)
				{
					var designStats = mainGame.Derivates.Of(shipGroup.Design.Owner).DesignStats[shipGroup.Design];
					var ammo = designStats.Abilities.Select(x => double.IsInfinity(x.Ammo) ? double.PositiveInfinity : x.Ammo * x.Quantity * (double)shipGroup.Quantity).ToArray();
					var abilities = designStats.Abilities.Select(x => x.Quantity * (double)shipGroup.Quantity).ToArray();
					var simiralCombatant = this.game.Combatants.FirstOrDefault(x => x.Position == position && x.Ships.Design == shipGroup.Design);
					
					if (simiralCombatant == null)
						this.game.Combatants.Add(new Combatant(position, fleet.OriginalFleet.Owner, shipGroup, designStats, ammo, abilities));
					else
					{
						simiralCombatant.Ships.Quantity += shipGroup.Quantity;
						for(int i = 0; i < abilities.Length; i++)
							simiralCombatant.AbilityCharges[i] += abilities[i];
					}
				}
			}
			
			var players = this.game.Combatants.Select(x => x.Owner).Distinct();
			foreach(var unit in this.game.Combatants)
				this.rollCloaking(unit, this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design], players);
		}
		
		private Vector2D correctPosition(Vector2D position)
		{
			var snapped = snapPosition(position);
			
			if (Math.Abs(snapped.Magnitude()) < 1e-3 && position.Magnitude() > 0)
				return Methods.HexNeighbours(new Vector2D(0, 0)).
					Aggregate((a, b) => (a - position).Magnitude() > (b - position).Magnitude() ? a : b);
			
			var corrected = snapped;
			while(Methods.HexDistance(corrected) > SpaceBattleGame.BattlefieldRadius)
			{
				var distance = Methods.HexDistance(corrected);
				corrected = Methods.HexNeighbours(corrected).
					Where(x => Methods.HexDistance(x) < distance).
					Aggregate((a, b) => (a - snapped).Magnitude() > (b - snapped).Magnitude() ? a : b);
			}
							
			return corrected;
		}
		
		private Vector2D snapPosition(Vector2D position)
		{
			var x = Math.Round(position.X, MidpointRounding.AwayFromZero);
			var yOffset = Math.Abs((int)x) % 2 == 0 ? 0 : -0.5;
			
			return new Vector2D(
				x,
				Math.Round(position.Y + yOffset, MidpointRounding.AwayFromZero)
			);
		}
		#endregion

		#region Battle lifecycle
		public bool IsOver 
		{
			get 
			{ 
				if (this.game.Turn >= this.game.TurnLimit)
					return true;
				
				//TODO(later) check planetary defenses
				var players = this.game.Combatants.Select(x => x.Owner).Distinct().ToList();
				
				return players.All(party1 => players.All(party2 => party1 == party2 || !this.mainGame.Processor.IsAtWar(party1, party2)));
			}
		}

		protected override void nextRound()
		{
			base.nextRound();
			this.moveProjectiles();
			this.makeUnitOrder();
		}

		private void moveProjectiles()
		{
			foreach (var missile in this.game.Projectiles)
			{
				while (missile.Target != null && missile.Count > 0)
					if (missile.Target.Ships.Quantity <= 0) //TODO(v0.7) check for retreat
					{
						//TODO(v0.7) check war declarations
						missile.Target = this.game.Combatants.
								Where(x => x.Position == missile.Position && x.Ships.Quantity > 0 && x.Owner != missile.Owner).
								OrderBy(x => x.Ships.Quantity).
								FirstOrDefault();
					}
					else if (missile.Position == missile.Target.Position)
					{
						missile.Count -= this.doProjectileAttack(missile.Owner, missile.Stats, missile.Count, missile.Target);
					}
					else if (missile.MovementPoints > 0)
					{
						missile.Position = Methods.FindBest(
							Methods.HexNeighbours(missile.Position),
							hex => -Methods.HexDistance(missile.Target.Position, hex)
						);
						missile.MovementPoints -= 1 / missile.Stats.Speed;
					}
					else
						break;

				missile.MovementPoints = Math.Min(missile.MovementPoints + 1, 1);
				if (missile.Count <= 0)
					this.game.Projectiles.PendRemove(missile);
			}
			this.game.Projectiles.ApplyPending();
        }

        private void makeUnitOrder()
		{
			this.calculateInitiative();

			var units = new List<Combatant>(this.battleGame.Combatants);
			units.Sort((a, b) => -a.Initiative.CompareTo(b.Initiative));

			foreach (var unit in units)
				this.game.PlayOrder.Enqueue(unit);
		}
		#endregion

		#region Unit actions
		public void MoveTo(Vector2D destination)
		{
			if (!this.ValidMoves(this.game.PlayOrder.Peek()).Contains(destination))
				return;
			
			var unit = this.game.PlayOrder.Peek();
			
			if (Methods.HexDistance(destination) <= SpaceBattleGame.BattlefieldRadius)
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
				this.nextRound();
		}
		
		//TODO(later) check target validity
		public void UseAbility(int index, double quantity, Combatant target)
		{
			var unit = this.game.PlayOrder.Peek();
			var abilityStats = this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design].Abilities[index];
			var chargesLeft = quantity;
			var spent = 0.0;
			
			if (!abilityStats.TargetShips || Methods.HexDistance(target.Position, unit.Position) > abilityStats.Range)
				return;

			if (abilityStats.IsInstantDamage)
				spent = this.doDirectAttack(unit.Owner, unit.Position, abilityStats, quantity, target);
			else if (abilityStats.IsProjectile)
			{
				this.doLaunchProjectile(unit, abilityStats, quantity, target);
				spent = quantity;
			}
			
			spent = Math.Min(spent, quantity);
			unit.AbilityCharges[index] -= spent;
			if (!double.IsInfinity(unit.AbilityAmmo[index]))
				unit.AbilityAmmo[index] -= spent;
		}

		public void UseAbility(int index, double quantity, CombatPlanet planet)
		{
			var unit = this.game.PlayOrder.Peek();
			var abilityStats = this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design].Abilities[index];
			var chargesLeft = quantity;

			if (!abilityStats.TargetColony || Methods.HexDistance(planet.Position, unit.Position) > abilityStats.Range)
				return;

			var spent = this.attackPlanet(abilityStats, quantity, planet);

			unit.AbilityCharges[index] -= spent;
			if (!double.IsInfinity(unit.AbilityAmmo[index]))
				unit.AbilityAmmo[index] -= spent;
		}

		public void UseAbility(int index, double quantity, StarData star)
		{
			var unit = this.game.PlayOrder.Peek();
			var abilityStats = this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design].Abilities[index];
			var chargesLeft = quantity;
			
			if (!abilityStats.TargetStar || Methods.HexDistance(unit.Position) > abilityStats.Range)
				return;
			
			if (abilityStats.IsInstantDamage)
			{
				if (star.Traits.All(x => x.Type.IdCode != abilityStats.AppliesTrait.IdCode))
					star.Traits.Add(abilityStats.AppliesTrait.Make());
				
				chargesLeft -= quantity;
			}
			
			unit.AbilityCharges[index] = chargesLeft;
		}
		#endregion

		public IEnumerable<Vector2D> ValidMoves(Combatant unit)
		{
			return unit.MovementPoints <= 0 ? 
				new Vector2D[0] : 
				Methods.HexNeighbours(unit.Position);
		}
		
		#region Damage dealing
		private double chanceToHit(double attackAccuracy, double attackerDetection, DesignStats targetStats, bool targetCloaked)
		{
			var targetStealth = targetStats.Jamming + (targetCloaked ? this.mainGame.Statics.ShipFormulas.NaturalCloakBonus : 0);

			return (targetStealth > attackerDetection ? Math.Pow(sigmoidBase, targetStealth - attackerDetection) : 1) *
				 Probability(attackAccuracy - targetStats.Evasion);
		}

		private static bool doTopDamage(double firePower, double shieldEfficiency, double armorEfficiency, Combatant target, DesignStats targetStats)
		{
			if (target.TopShields > 0)
			{
				double shieldFire = firePower - Reduce(firePower, targetStats.ShieldThickness, shieldEfficiency);
				double shieldDamage = Reduce(shieldFire, targetStats.ShieldReduction, 1);
				firePower -= shieldFire - shieldDamage; //damage reduction difference

				shieldDamage = Math.Min(shieldDamage, target.TopShields);
				target.TopShields -= shieldDamage;
				firePower -= shieldDamage;
			}

			double armorDamage = Reduce(firePower, targetStats.ArmorReduction, armorEfficiency);
			target.TopArmor -= armorDamage;

			if (target.TopArmor <= 0)
			{
				target.Ships.Quantity--;
				var safeCount = Math.Max(target.Ships.Quantity, 1);

				target.TopArmor = target.RestArmor / safeCount;
				target.TopShields = target.RestShields / safeCount;
				target.RestArmor -= target.TopArmor;
				target.RestShields -= target.TopShields;

				return true;
			}

			return false;
		}

		private static bool doRestDamage(double maxTargets, double firePower, double shieldEfficiency, double armorEfficiency, Combatant target, DesignStats targetStats)
		{
			var targetRestCount = Math.Max(target.Ships.Quantity - 1, 0);
			var splashTargets = Methods.Clamp(maxTargets, 0, targetRestCount);

			if (target.RestShields > 0)
			{
				double shieldFire = firePower - Reduce(firePower, targetStats.ShieldThickness, shieldEfficiency);
				double shieldDamage = Reduce(shieldFire, targetStats.ShieldReduction, 1);
				firePower -= shieldFire - shieldDamage; //damage reduction difference

				shieldDamage = Math.Min(shieldDamage * splashTargets, target.RestShields);
				target.RestShields -= shieldDamage;
				firePower -= shieldDamage;
			}

			double armorDamage = Reduce(firePower, targetStats.ArmorReduction, armorEfficiency);
			target.RestArmor -= armorDamage * splashTargets;

			if (target.RestArmor <= 0)
			{
				target.Ships.Quantity = Math.Min(target.Ships.Quantity, 1);

				target.RestArmor = 0;
				target.RestShields = 0;

				return true;
			}

			return false;
		}

		private static void updateStackTop(Combatant stack)
		{
			if (stack.TopArmor > 0 || stack.Ships.Quantity <= 0)
				return;

			stack.Ships.Quantity--;
			var safeCount = Math.Max(stack.Ships.Quantity, 1);

			stack.TopArmor = stack.RestArmor / safeCount;
			stack.TopShields = stack.RestShields / safeCount;
			stack.RestArmor -= stack.TopArmor;
			stack.RestShields -= stack.TopShields;
		}
		
		private double doDirectAttack(Player attackSide, Vector2D attackFrom, AbilityStats abilityStats, double quantity, Combatant target)
		{
			var targetStats = this.mainGame.Derivates.Of(target.Owner).DesignStats[target.Ships.Design];
			var hitChance = chanceToHit(
				abilityStats.Accuracy + Methods.HexDistance(attackFrom, target.Position) * abilityStats.AccuracyRangePenalty,
				sensorStrength(target.Position, attackSide),
				targetStats, target.CloakedFor.Contains(attackSide));
			var spent = 0.0;

			while (quantity > spent && target.Ships.Quantity > 0)
			{
				spent++;

				if (hitChance > this.game.Rng.NextDouble())
				{
					doTopDamage(abilityStats.FirePower, abilityStats.ShieldEfficiency, abilityStats.ArmorEfficiency, target, targetStats);
					updateStackTop(target);
				}
			}
			
			//TODO(later) do different calculation for multiple ships below top of the stack
			return spent;
		}

		private long doProjectileAttack(Player attackSide, AbilityStats abilityStats, double quantity, Combatant target)
		{
			var targetStats = this.mainGame.Derivates.Of(target.Owner).DesignStats[target.Ships.Design];
			var hitChance = chanceToHit(
				abilityStats.Accuracy, sensorStrength(target.Position, attackSide),
				targetStats, target.CloakedFor.Contains(attackSide));
			long spent = 0;

			while (quantity > spent && target.Ships.Quantity > 0)
			{
				spent++;
				bool hit = hitChance > this.game.Rng.NextDouble();

				if (hit)
					doTopDamage(abilityStats.FirePower, abilityStats.ShieldEfficiency, abilityStats.ArmorEfficiency, target, targetStats);
				else if (abilityStats.SplashMaxTargets > 0)
					doTopDamage(abilityStats.SplashFirePower, abilityStats.SplashShieldEfficiency, abilityStats.SplashArmorEfficiency, target, targetStats);

				doRestDamage(
					abilityStats.SplashMaxTargets - 1,
					abilityStats.SplashFirePower, abilityStats.SplashShieldEfficiency, abilityStats.SplashArmorEfficiency, 
					target, targetStats);

				//TODO(v0.7) spill leftover spalsh damage to other stacks

				updateStackTop(target);
			}

			return spent;
		}

		private void doLaunchProjectile(Combatant attacker, AbilityStats abilityStats, double quantity, Combatant target)
		{
			this.game.Projectiles.Add(new Projectile(
				attacker.Owner,
				(long)quantity, 
				abilityStats, 
				target, 
				attacker.Position
			));
		}
		#endregion
	}
}
