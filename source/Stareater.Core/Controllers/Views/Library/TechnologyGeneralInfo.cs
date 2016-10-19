using System;
using Stareater.GameData;
using Stareater.Localization;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views.Library
{
	public class TechnologyGeneralInfo
	{
		private const string LangContext = TechnologyTopic.LangContext;
		
		private readonly DevelopmentTopic Data;
		
		internal TechnologyGeneralInfo(DevelopmentTopic data)
		{
			this.Data = data;
		}
		
		public string Name(int level)
		{
			if (level < 0 || level > this.Data.MaxLevel)
				throw new ArgumentOutOfRangeException("level");
					
			return LocalizationManifest.Get.CurrentLanguage[LangContext][this.Data.NameCode].Text(new Var(DevelopmentTopic.LevelKey, level).Get);
		}
		
		public string Description(int level)
		{ 
			if (level < 0 || level > this.Data.MaxLevel)
				throw new ArgumentOutOfRangeException("level");
			
			return LocalizationManifest.Get.CurrentLanguage[LangContext][this.Data.DescriptionCode].Text(new Var(DevelopmentTopic.LevelKey, level).Get);
		}
		
		public string ImagePath 
		{
			get
			{
				return this.Data.ImagePath;
			}
		}
		
		public int MaxLevel 
		{ 
			get 
			{
				return this.Data.MaxLevel;
			}
		}
	}
}
