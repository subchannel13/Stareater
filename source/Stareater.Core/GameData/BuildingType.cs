using System;
using Stareater.AppData.Expressions;

namespace Stareater.GameData
{
	public class BuildingType
	{
		public string LanguageCode { get; private set; }
		public string ImagePath { get; private set; }
		
		public Formula HitPoints { get; private set; }
		
		public BuildingType(string languageCode, string imagePath, Formula hitPoints)
		{
			this.LanguageCode = languageCode;
			this.ImagePath = imagePath;
			
			this.HitPoints = hitPoints;
		}
	}
}
