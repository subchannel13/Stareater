using System;
using System.Collections.Generic;
using Stareater.AppData;
using Stareater.GameData;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Data
{
	public class TechnologyTopic
	{
		private const string LangContext = "Technologies";
		private const string NameSuffix = "_NAME";
		private const string DescriptionSuffix = "_DESC";
		
		private Technology technology;
		private IDictionary<string, double> textVars;
		
		public string ImagePath { get; private set; }
		public double Cost { get; private set; }
		
		public TechnologyTopic(TechnologyProgress tech)
		{
			this.technology = tech.Topic;
			this.textVars = new Var("lvl", tech.NextLevel).Get;
				
			this.Cost = tech.Topic.Cost.Evaluate(new Var("lvl0", tech.NextLevel).Get);
		}
		
		public string Name 
		{
			get 
			{
				return Settings.Get.Language[LangContext][technology.IdCode + NameSuffix].Text(textVars);
			}
		}
		
		public string Description 
		{ 
			get 
			{
				return Settings.Get.Language[LangContext][technology.IdCode + DescriptionSuffix].Text(textVars);
			}
		}
		
		public long MaxLevel 
		{ 
			get 
			{
				return technology.MaxLevel;
			}
		}
	}
}
