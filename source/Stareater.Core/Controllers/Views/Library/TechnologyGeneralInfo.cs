using System;
using NGenerics.Util;
using Stareater.AppData;
using Stareater.GameData;
using Stareater.Utils;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views.Library
{
	public class TechnologyGeneralInfo
	{
		internal const string LangContext = "Technologies";
		
		private readonly Technology Data;
		
		internal TechnologyGeneralInfo(Technology data)
		{
			this.Data = data;
		}
		
		public string Name(int level)
		{
			if (level < 0 || level > this.Data.MaxLevel)
				throw new ArgumentOutOfRangeException("level");
					
			return Settings.Get.Language[LangContext][this.Data.NameCode].Text(new Var(Technology.LevelKey, level).Get);
		}
		
		public string Description(int level)
		{ 
			if (level < 0 || level > this.Data.MaxLevel)
				throw new ArgumentOutOfRangeException("level");
			
			return Settings.Get.Language[LangContext][this.Data.DescriptionCode].Text(new Var(Technology.LevelKey, level).Get);
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
