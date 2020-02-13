using System;
using System.Linq;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Combat;
using Stareater.Utils.StateEngine;

namespace Stareater.Players.DefaultAI
{
	[StateTypeAttribute(saveTag: DefaultAIFactory.FactoryId)]
	class DefaultAIPlayer : IOffscreenPlayer, IBattleEventListener, IBombardEventListener
	{
		private readonly Random random = new Random();
		private PlayerController playerController;
		private SpaceBattleController battleController;
		private BombardmentController bombardController;
		
		public PlayerController Controller 
		{ 
			set { this.playerController = value; }
		}
		
		public void PlayTurn()
		{
			this.playerController.RunAutomation();

			foreach(var stellaris in this.playerController.Stellarises())
			{
				StarSystemController starSystem = this.playerController.OpenStarSystem(stellaris.HostStar);
				manage(starSystem.StellarisController());				
			}
			
			this.playerController.EndGalaxyPhase();
		}

		public IBattleEventListener StartBattle(SpaceBattleController controller)
		{
			this.battleController = controller;
			return this;
		}
		
		public IBombardEventListener StartBombardment(BombardmentController controller)
		{
			this.bombardController = controller;
			return this;
		}

		#region IBattleEventListener implementation
		void IBattleEventListener.OnStart()
		{
			//no operation
		}

		public void PlayUnit(CombatantInfo unitInfo)
		{
			this.battleController.UnitDone();
		}
		#endregion
		
		#region IBombardEventListener implementation

		public void BombardTurn()
		{
			this.bombardController.Leave();
		}

		#endregion

		private void manage(AConstructionSiteController controller)
		{
			var options = controller.ConstructableItems.ToArray();
			if (options.Length == 0)
				return;
			
			while(controller.ConstructionQueue.Any())
				controller.Dequeue(0);
			controller.Enqueue(options[random.Next(options.Length)]);
		}
	}
}
