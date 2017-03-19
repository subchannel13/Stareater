using System;
using Stareater.AppData.Expressions;

namespace Stareater.GameData
{
	class ResearchTopic
	{
		public string LanguageCode { get; private set; }
		
		public string ImagePath { get; private set; }
		public string IdCode { get; private set; }
		public Formula Cost { get; private set; }
		
		public string[][] Unlocks { get; private set; }
		
		public ResearchTopic(string languageCode, string imagePath, string idCode, Formula cost, string[][] unlocks)
		{
			this.LanguageCode = languageCode;
			this.ImagePath = imagePath;
			this.IdCode = idCode;
			this.Cost = cost;
			this.Unlocks = unlocks;
		}
		
		public int MaxLevel { get { return this.Unlocks.Length - 1; } }
	}
}
