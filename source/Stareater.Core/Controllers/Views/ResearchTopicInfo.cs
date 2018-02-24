using System.Collections.Generic;
using System.Linq;
using Stareater.GameData;
using Stareater.Localization;
using Stareater.Utils.Collections;
using Stareater.GameLogic.Planning;

namespace Stareater.Controllers.Views
{
	public class ResearchTopicInfo
	{
		internal const string LangContext = "Technologies";
		
		private readonly ResearchTopic topic;
		private IDictionary<string, double> textVars;
		
		public double Cost { get; private set; }
		public double InvestedPoints { get; private set; }
		public double Investment { get; private set; }
		public int Level { get; private set; }
		public int NextLevel { get; private set; }
		public DevelopmentTopicInfo[] Unlocks { get; private set; }
		
		internal ResearchTopicInfo(ResearchProgress tech, IEnumerable<DevelopmentTopic> developmentTopics)
		{
			this.topic = tech.Topic;
			this.textVars = new Var(DevelopmentTopic.LevelKey, tech.NextLevel).Get;
				
			this.Cost = tech.Topic.Cost.Evaluate(textVars);
			this.InvestedPoints = tech.InvestedPoints;
			this.Investment = 0;
			this.Level = tech.Level;
			this.NextLevel = tech.NextLevel;
			this.Unlocks = tech.Topic.Unlocks[tech.NextLevel].Select(id => new DevelopmentTopicInfo(new DevelopmentProgress(
				developmentTopics.First(x => x.IdCode == id), tech.Owner)
			)).ToArray();
		}

		internal ResearchTopicInfo(ResearchProgress tech, ResearchResult investmentResult, IEnumerable<DevelopmentTopic> developmentTopics)
		{
			this.topic = tech.Topic;
			this.textVars = new Var(DevelopmentTopic.LevelKey, tech.NextLevel).Get;
				
			this.Cost = tech.Topic.Cost.Evaluate(textVars);
			this.InvestedPoints = tech.InvestedPoints;
			this.Investment = investmentResult.InvestedPoints;
			this.Level = tech.Level;
			this.NextLevel = investmentResult.CompletedCount > 1 ? tech.Level + (int)investmentResult.CompletedCount : tech.NextLevel;
			this.Unlocks = tech.Topic.Unlocks[tech.NextLevel].Select(id => new DevelopmentTopicInfo(new DevelopmentProgress(
				developmentTopics.First(x => x.IdCode == id), tech.Owner)
			)).ToArray();
		}
		
		public string Name 
		{
			get 
			{
				return LocalizationManifest.Get.CurrentLanguage[LangContext].Name(topic.LanguageCode).Text(textVars);
			}
		}
		
		public string Description 
		{ 
			get 
			{
				return LocalizationManifest.Get.CurrentLanguage[LangContext].Description(topic.LanguageCode).Text(textVars);
			}
		}
		
		public string ImagePath 
		{
			get
			{
				return topic.ImagePath;
			}
		}
		
		public int MaxLevel 
		{ 
			get 
			{
				return topic.MaxLevel;
			}
		}
		
		public string IdCode 
		{
			get 
			{
				return topic.IdCode;
			}
		}
	}
}
