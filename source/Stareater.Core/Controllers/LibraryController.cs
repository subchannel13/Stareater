using System;

namespace Stareater.Controllers
{
	public class LibraryController
	{
		private readonly GameController gameController;
		
		internal LibraryController(GameController gameController)
		{
			this.gameController = gameController;
		}
	}
}
