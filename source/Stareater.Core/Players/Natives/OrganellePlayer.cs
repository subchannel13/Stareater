using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Ikadn.Ikon.Types;
using Stareater.Controllers.Views.Combat;

namespace Stareater.Players.Natives
{
	class OrganellePlayer : IOffscreenPlayer, IBattleEventListener
	{
		private PlayerController playerController;
		private SpaceBattleController battleController;

		public PlayerController Controller
		{
			set { this.playerController = value; }
		}

		public void PlayTurn()
		{
			//TODO(v0.6);
		}

		public void OnResearchComplete(ResearchCompleteController controller)
		{
			//no operation
		}

		public IBattleEventListener StartBattle(SpaceBattleController controller)
		{
			this.battleController = controller;
			return this;
		}

		public Ikadn.IkadnBaseObject Save()
		{
			return new IkonComposite(PlayerType.OrganelleControllerTag);
		}

		public void PlayUnit(CombatantInfo unitInfo)
		{
			//TODO(v0.6);
		}
	}
}
