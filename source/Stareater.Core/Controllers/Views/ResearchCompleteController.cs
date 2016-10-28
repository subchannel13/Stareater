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
		private readonly GameController gameController;
		private readonly MainGame game;
		private readonly List<string> priorities = new List<string>();
		
		internal Player Owner { get; private set; }
		internal ResearchProgress TopicProgress { get; private set; }
		
		internal ResearchCompleteController(Player owner, ResearchTopic topic, GameController gameController, MainGame game)
		{
			this.game = game;
			this.gameController = gameController;
			this.Owner = owner;
			this.TopicProgress = this.game.States.ResearchAdvances.Of(owner).First(x => x.Topic == topic);
			
			this.priorities.AddRange(this.TopicProgress.Topic.Unlocks[this.TopicProgress.NextLevel]);
		}

		public void Done()
		{
			gameController.BreakthroughReviewed(this);
		}
		
		public ResearchTopicInfo TopicInfo
		{
			get
			{
				return new ResearchTopicInfo(TopicProgress, this.game.Statics.DevelopmentTopics);
			}
		}

		public IEnumerable<DevelopmentTopicInfo> UnlockPriorities 
		{
			get
			{
				for(int i = 0; i < this.priorities.Count; i++)
					yield return new DevelopmentTopicInfo(
						new DevelopmentProgress(this.Owner, this.game.Statics.DevelopmentTopics.First(x => x.IdCode == this.priorities[i]), 0, 0, i)
					);
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
		
		internal IList<string> SelectedPriorities
		{
			get { return this.priorities; }
		}
	}
}
