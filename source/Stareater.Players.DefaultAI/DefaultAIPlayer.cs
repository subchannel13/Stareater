using System;
using System.Linq;
using Ikadn.Ikon.Types;
using Stareater.Controllers;
using Stareater.Controllers.Views;

namespace Stareater.Players.DefaultAI
{
	class DefaultAIPlayer : IOffscreenPlayer
	{
		private readonly Random random = new Random();
		
		public void PlayTurn(PlayerController controller)
		{
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

		public void PlayBattle()
		{
			throw new NotImplementedException();
		}
		
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
