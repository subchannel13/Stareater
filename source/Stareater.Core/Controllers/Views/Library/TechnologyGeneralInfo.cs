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
		private readonly string nameCode;
		private readonly string descriptionCode;
		private readonly string imagePath;
		
		internal TechnologyGeneralInfo(DevelopmentTopic data)
		{
			this.descriptionCode = data.DescriptionCode;
			this.imagePath = data.ImagePath;
			this.maxLevel = data.MaxLevel;
			this.nameCode = data.NameCode;
		}

		internal TechnologyGeneralInfo(ResearchTopic data)
		{
			this.descriptionCode = data.DescriptionCode;
			this.imagePath = data.ImagePath;
			this.maxLevel = data.MaxLevel;
			this.nameCode = data.NameCode;
		}
		
		public string Name(int level)
		{
			if (level < 0 || level > this.maxLevel)
				throw new ArgumentOutOfRangeException("level");
					
			return LocalizationManifest.Get.CurrentLanguage[LangContext][this.nameCode].Text(new Var(DevelopmentTopic.LevelKey, level).Get);
		}
		
		public string Description(int level)
		{
			if (level < 0 || level > this.maxLevel)
				throw new ArgumentOutOfRangeException("level");
			
			return LocalizationManifest.Get.CurrentLanguage[LangContext][this.descriptionCode].Text(new Var(DevelopmentTopic.LevelKey, level).Get);
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
