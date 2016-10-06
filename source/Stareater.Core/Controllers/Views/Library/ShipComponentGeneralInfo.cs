using System;
using Stareater.GameData.Ships;
using Stareater.Localization;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views.Library
{
	public class ShipComponentGeneralInfo
	{
		private AComponentType Data;
		private string langContext;
		private string imagePath;
		
		internal ShipComponentGeneralInfo(AComponentType data, string langContext, string imagePath)
		{
			this.Data = data;
			this.langContext = langContext;
			this.imagePath = imagePath;
		}
		
		public string Name(int level)
		{
			if (level < 0 || level > this.Data.MaxLevel)
				throw new ArgumentOutOfRangeException("level");
					
			return LocalizationManifest.Get.CurrentLanguage[langContext][this.Data.NameCode].Text(new Var(AComponentType.LevelKey, level).Get);
		}
		
		public string Description(int level)
		{ 
			if (level < 0 || level > this.Data.MaxLevel)
				throw new ArgumentOutOfRangeException("level");
			
			return LocalizationManifest.Get.CurrentLanguage[langContext][this.Data.DescCode].Text(new Var(AComponentType.LevelKey, level).Get);
		}
		
		public string ImagePath 
		{
			get
			{
				return imagePath;
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
