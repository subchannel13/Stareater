using System;

namespace Stareater.Controllers
{
	public class AudienceController
	{
		private readonly GameController gameController;
		
		internal AudienceController(GameController gameController)
		{
			this.gameController = gameController;
		}
		
		public void Done()
		{
			gameController.AudienceConcluded(this);
		}
	}
}
