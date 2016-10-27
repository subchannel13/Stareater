using System;
using Stareater.GameData;
using Stareater.Players;

namespace Stareater.Controllers.Views
{
	public class ResearchCompleteController
	{
		private readonly Player player;
		private readonly ResearchTopic topic;
		private readonly GameController gameController;
		private readonly MainGame game;
		
		internal ResearchCompleteController(Player player, ResearchTopic topic, GameController gameController, MainGame game)
		{
			this.game = game;
			this.gameController = gameController;
			this.player = player;
			this.topic = topic;
		}

		public void Done()
		{
			gameController.BreakthroughReviewed(this);
		}
	}
}
