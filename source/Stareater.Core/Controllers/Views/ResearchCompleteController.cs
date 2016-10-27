using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Views;
using Stareater.GameData;
using Stareater.Players;

namespace Stareater.Controllers.Views
{
	public class ResearchCompleteController
	{
		private readonly Player player;
		private readonly ResearchProgress topicProgress;
		private readonly GameController gameController;
		private readonly MainGame game;
		
		private readonly List<string> priorities = new List<string>();
		
		internal ResearchCompleteController(Player player, ResearchTopic topic, GameController gameController, MainGame game)
		{
			this.game = game;
			this.gameController = gameController;
			this.player = player;
			this.topicProgress = this.game.States.ResearchAdvances.Of(player).First(x => x.Topic == topic);
			
			this.priorities.AddRange(this.topicProgress.Topic.Unlocks[this.topicProgress.NextLevel]);
		}

		public void Done()
		{
			gameController.BreakthroughReviewed(this);
		}
		
		public ResearchTopicInfo TopicInfo
		{
			get
			{
				return new ResearchTopicInfo(topicProgress, this.game.Statics.DevelopmentTopics);
			}
		}

		public IEnumerable<DevelopmentTopicInfo> UnlockPriorities 
		{
			get
			{
				return this.priorities.Select(devTopic => new DevelopmentTopicInfo(
					new DevelopmentProgress(this.player, this.game.Statics.DevelopmentTopics.First(x => x.IdCode == devTopic), 0, 0)
				));
			}
		}
		
		public void SetPriority(DevelopmentTopicInfo unlock, int priority)
		{
			if (priority < 0 || priority >= this.priorities.Count || !this.priorities.Contains(unlock.IdCode))
				return;
			
			this.priorities.Remove(unlock.IdCode);
			if (priority < this.priorities.Count)
				this.priorities.Insert(priority, unlock.IdCode);
			else
				this.priorities.Add(unlock.IdCode);
		}
	}
}
