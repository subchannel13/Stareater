using System;
using Stareater.GameData;
using Stareater.Players;

namespace Stareater.Controllers.Views
{
	public class ResearchCompleteController
	{
		private readonly MainGame game;
		private readonly Player player;
		private readonly ResearchTopic topic;
		
		internal ResearchCompleteController(MainGame game, Player player, ResearchTopic topic)
		{
			this.game = game;
			this.player = player;
			this.topic = topic;
		}
	}
}
