using Stareater.Controllers.Views;
using System.Collections.Generic;
using System.Linq;

namespace Stareater.Controllers
{
	public class ResultsController
	{
		private MainGame game;

		internal ResultsController(MainGame game)
		{
			this.game = game;
		}

		public IEnumerable<PlayerProgressInfo> Scores
		{
			get
			{
				return this.game.MainPlayers.Select(x => new PlayerProgressInfo(x));
			}
		}
	}
}
