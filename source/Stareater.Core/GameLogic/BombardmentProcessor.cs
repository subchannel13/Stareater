using System;
using System.Linq;
using Stareater.SpaceCombat;
using System.Collections.Generic;
using Stareater.Players;
using Stareater.Utils.Collections;
using Stareater.GameData.Ships;

namespace Stareater.GameLogic
{
	class BombardmentProcessor : ACombatProcessor
	{
		protected readonly BombardBattleGame game;

		public BombardmentProcessor(BombardBattleGame battleGame, MainGame mainGame) : base(mainGame)
		{
			this.game = battleGame;
			this.makePlayerOrder();
		}

		protected override ABattleGame battleGame => game;

		public bool IsOver 
		{
			get 
			{ 
				return !this.CanBombard;
			}
		}

		private void makePlayerOrder()
		{
			this.calculateInitiative();

			var colonies = this.battleGame.Planets.Where(x => x.Colony != null).ToList();
			var initiativeSum = new Dictionary<Player, double>();
			var fleetWeight = new Dictionary<Player, double>();
			foreach(var unit in this.game.Combatants)
			{
				if (colonies.All(planet => !this.mainGame.Processor.IsAtWar(unit.Owner, planet.Colony.Owner)) || !unitCanBombard(unit))
					continue;

				if (!initiativeSum.ContainsKey(unit.Owner))
				{
					initiativeSum[unit.Owner] = 0;
					fleetWeight[unit.Owner] = 0;
				}

				var hull = unit.Ships.Design.Hull;
				var weight = unit.Ships.Quantity * hull.TypeInfo.Size.Evaluate(new Var(AComponentType.LevelKey, hull.Level).Get); //TODO(v0.7) move size to design stats
				initiativeSum[unit.Owner] += unit.Initiative * weight;
				fleetWeight[unit.Owner] += weight;
			}

			this.game.PlayOrder.Clear();
			foreach (var player in initiativeSum.Keys.OrderBy(x => initiativeSum[x] / fleetWeight[x]))
				this.game.PlayOrder.Enqueue(player);
		}

		public void Bombard(CombatPlanet planet)
		{
			foreach(var unit in this.game.Combatants)
			{
				var unitStats = this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design];

				for (int i = 0; i < unit.AbilityCharges.Length; i++)
					this.attackPlanet(unitStats.Abilities[i], unit.AbilityCharges[i], planet);
			}
			
			this.nextRound();
		}
	}
}
