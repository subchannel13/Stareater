using System;
using System.Collections.Generic;
using Stareater.Controllers.Data;

namespace Stareater.Controllers
{
	public class SavesController
	{
		GameController gameController;
		
		public SavesController(GameController gameController)
		{
			this.gameController = gameController;
		}
		
		public bool CanSave
		{
			get { return true; }
		}
		
		public IEnumerable<SavedGameData> Games
		{
			get
			{
				yield break;
			}
		}
	}
}
