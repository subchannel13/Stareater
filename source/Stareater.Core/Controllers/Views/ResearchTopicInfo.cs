using System;
using System.Collections.Generic;
using Stareater.GameData;
using Stareater.GameLogic.Planning;
using Stareater.Localization;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views
{
	public class ResearchTopicInfo : IEquatable<ResearchTopicInfo>
	{
		internal const string LangContext = "Technologies";
		
		internal ResearchTopic Topic { get; private set; }
		private readonly IDictionary<string, double> textVars;
		
		public double Cost { get; private set; }
		public double InvestedPoints { get; private set; }
		public double Investment { get; private set; }
		public int Level { get; private set; }
		public int NextLevel { get; private set; }
		
		internal ResearchTopicInfo(ResearchProgress tech)
		{
			this.Topic = tech.Topic;
			this.textVars = new Var(DevelopmentTopic.LevelKey, tech.NextLevel).Get;
				
			this.Cost = tech.Topic.Cost.Evaluate(textVars);
			this.InvestedPoints = tech.InvestedPoints;
			this.Investment = 0;
			this.Level = tech.Level;
			this.NextLevel = tech.NextLevel;
		}

		internal ResearchTopicInfo(ResearchProgress tech, ResearchResult investmentResult)
		{
			this.Topic = tech.Topic;
			this.textVars = new Var(DevelopmentTopic.LevelKey, tech.NextLevel).Get;
				
			this.Cost = tech.Topic.Cost.Evaluate(textVars);
			this.InvestedPoints = tech.InvestedPoints;
			this.Investment = investmentResult.InvestedPoints;
			this.Level = tech.Level;
			this.NextLevel = investmentResult.CompletedCount > 1 ? tech.Level + (int)investmentResult.CompletedCount : tech.NextLevel;
		}
		
		public string Name 
		{
			get 
			{
				return LocalizationManifest.Get.CurrentLanguage[LangContext].Name(Topic.LanguageCode).Text(textVars);
			}
		}
		
		public string Description 
		{ 
			get 
			{
				return LocalizationManifest.Get.CurrentLanguage[LangContext].Description(Topic.LanguageCode).Text(textVars);
			}
		}
		
		public string ImagePath 
		{
			get
			{
				return Topic.ImagePath;
			}
		}
		
		public int MaxLevel 
		{ 
			get 
			{
				return Topic.MaxLevel;
			}
		}
		
		public string IdCode 
		{
			get 
			{
				return Topic.IdCode;
			}
		}

		public bool Equals(ResearchTopicInfo other)
		{
			if (other is null)
				return false;

			if (Object.ReferenceEquals(this, other))
				return true;

			if (this.GetType() != other.GetType())
				return false;

			return this.Topic.IdCode == other.Topic.IdCode;
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as ResearchTopicInfo);
		}

		public override int GetHashCode()
		{
			return this.Topic.IdCode.GetHashCode();
		}

		public static bool operator ==(ResearchTopicInfo info1, ResearchTopicInfo info2)
		{
			if (info1 is null)
				return info2 is null;
			return info1.Equals(info2);
		}

		public static bool operator !=(ResearchTopicInfo info1, ResearchTopicInfo info2)
		{
			return !(info1 == info2);
		}
	}
}
