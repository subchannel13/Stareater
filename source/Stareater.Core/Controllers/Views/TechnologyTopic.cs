using System;
using System.Collections.Generic;
using Stareater.GameData;
using Stareater.GameLogic;
using Stareater.Localization;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views
{
	public class TechnologyTopic
	{
		internal const string LangContext = "Technologies";
		
		private readonly Technology technology;
		private IDictionary<string, double> textVars;
		
		public double Cost { get; private set; }
		public double InvestedPoints { get; private set; }
		public double Investment { get; private set; }
		public int Level { get; private set; }
		public int NextLevel { get; private set; }
		
		internal TechnologyTopic(TechnologyProgress tech)
		{
			this.technology = tech.Topic;
			this.textVars = new Var(Technology.LevelKey, tech.NextLevel).Get;
				
			this.Cost = tech.Topic.Cost.Evaluate(textVars);
			this.InvestedPoints = tech.InvestedPoints;
			this.Investment = 0;
			this.Level = tech.Level;
			this.NextLevel = tech.NextLevel;
		}
		
		internal TechnologyTopic(TechnologyProgress tech, ScienceResult investmentResult)
		{
			this.technology = tech.Topic;
			this.textVars = new Var(Technology.LevelKey, tech.NextLevel).Get;
				
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
				return LocalizationManifest.Get.CurrentLanguage[LangContext][technology.NameCode].Text(textVars);
			}
		}
		
		public string Description 
		{ 
			get 
			{
				return LocalizationManifest.Get.CurrentLanguage[LangContext][technology.DescriptionCode].Text(textVars);
			}
		}
		
		public string ImagePath 
		{
			get
			{
				return technology.ImagePath;
			}
		}
		
		public int MaxLevel 
		{ 
			get 
			{
				return technology.MaxLevel;
			}
		}
		
		public string IdCode 
		{
			get 
			{
				return technology.IdCode;
			}
		}
	}
}
