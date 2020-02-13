using System;
using Stareater.GameData;
using Stareater.Localization;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views.Library
{
	public class TechnologyGeneralInfo
	{
		private const string LangContext = DevelopmentTopicInfo.LangContext;

		private readonly int maxLevel;
		private readonly string languageCode;
		private readonly string imagePath;
		
		internal TechnologyGeneralInfo(DevelopmentTopic data)
		{
			this.imagePath = data.ImagePath;
			this.maxLevel = data.MaxLevel;
			this.languageCode = data.LanguageCode;
		}

		internal TechnologyGeneralInfo(ResearchTopic data)
		{
			this.imagePath = data.ImagePath;
			this.maxLevel = data.MaxLevel;
			this.languageCode = data.LanguageCode;
		}
		
		public string Name(int level)
		{
			if (level < 0 || level > this.maxLevel)
				throw new ArgumentOutOfRangeException(nameof(level));
					
			return LocalizationManifest.Get.CurrentLanguage[LangContext].Name(this.languageCode).Text(new Var(DevelopmentTopic.LevelKey, level).Get);
		}
		
		public string Description(int level)
		{
			if (level < 0 || level > this.maxLevel)
				throw new ArgumentOutOfRangeException(nameof(level));
			
			return LocalizationManifest.Get.CurrentLanguage[LangContext].Description(this.languageCode).Text(new Var(DevelopmentTopic.LevelKey, level).Get);
		}
		
		public string ImagePath 
		{
			get
			{
				return this.imagePath;
			}
		}
		
		public int MaxLevel 
		{ 
			get 
			{
				return this.maxLevel;
			}
		}
	}
}
