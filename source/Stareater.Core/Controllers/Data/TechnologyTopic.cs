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
		
		public double Cost { get; private set; }
		public double InvestedPoints { get; private set; }
		public double Investment { get; private set; }
		public int Level { get; private set; }
		public int NextLevel { get; private set; }
		
		internal TechnologyTopic(TechnologyProgress tech)
		{
			this.technology = tech.Topic;
			this.textVars = new Var("lvl0", tech.NextLevel).Get;
				
			this.Cost = tech.Topic.Cost.Evaluate(textVars);
			this.InvestedPoints = tech.InvestedPoints;
			this.Investment = 0; //TODO: Get real investment points
			this.Level = tech.Level;
			this.NextLevel = tech.NextLevel;
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
		
		public string ImagePath 
		{
			get
			{
				return technology.ImagePath;
			}
		}
		
		public long MaxLevel 
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
