using System;
using System.Linq;
using Ikadn.Ikon.Types;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Combat;

namespace Stareater.Players.DefaultAI
{
	class DefaultAIPlayer : IOffscreenPlayer, IBattleEventListener
	{
		private readonly Random random = new Random();
		private PlayerController playerController;
		private SpaceBattleController battleController;
		
		public void PlayTurn(PlayerController controller)
		{
			//TODO(v0.5) change interface to register controller first and play turn later
			this.playerController = controller;
			
			foreach(var stellaris in controller.Stellarises())
			{
				StarSystemController starSystem = controller.OpenStarSystem(stellaris.HostStar);
				manage(starSystem.StellarisController());
				
				foreach(var planet in starSystem.Planets)
					if (starSystem.BodyType(planet.Position) == BodyType.OwnColony)
						manage(starSystem.ColonyController(planet.Position));
			}
			
			controller.EndGalaxyPhase();
		}

		public void PlayBattle(SpaceBattleController controller)
		{
			this.battleController = controller;
			this.battleController.Register(this.playerController, this);
		}

		#region IBattleEventListener implementation
		public void PlayUnit(CombatantInfo unitInfo)
		{
			//TODO(v0.5) skip turn
		}
		#endregion
		
		public Ikadn.IkadnBaseObject Save()
		{
			var data = new IkonComposite(PlayerType.AiControllerTag);
			data.Add(PlayerType.FactoryIdKey, new IkonText(DefaultAIFactory.FactoryId));
			
			return data;
		}

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
