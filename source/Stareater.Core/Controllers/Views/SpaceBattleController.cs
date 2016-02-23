using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Views.Combat;
using Stareater.Galaxy;

namespace Stareater.Controllers.Views
{
	public class SpaceBattleController
	{
		private readonly SpaceBattleGame battleGame;
		private readonly MainGame mainGame;
		
		internal SpaceBattleController(SpaceBattleGame battleGame, MainGame mainGame)
		{
			this.battleGame = battleGame;
			this.mainGame = mainGame;
			this.Star = mainGame.States.Stars.At(battleGame.Location);
		}
	
		public static readonly int BattlefieldRadius = SpaceBattleGame.BattlefieldRadius;
		
		public StarData Star { get; private set; }
		
		public IEnumerable<CombatantInfo> Units
		{
			get { return this.battleGame.Combatants.Select(x => new CombatantInfo(x, mainGame.Derivates.Of(x.Owner).DesignStats[x.Ships.Design])); }
		}
	}
}
