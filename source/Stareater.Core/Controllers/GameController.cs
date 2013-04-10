using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Controllers.Data;

namespace Stareater.Controllers
{
	public class GameController
	{
		public GameState State { get; private set; }

		public GameController()
		{
			State = GameState.NoGame;
		}
	}
}
