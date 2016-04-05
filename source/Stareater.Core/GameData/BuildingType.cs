using System;
using Stareater.AppData.Expressions;

namespace Stareater.GameData
{
	public class BuildingType
	{
		public string NameCode { get; private set; }
		public string ImagePath { get; private set; }
		
		public Formula HitPoints { get; private set; }
		
		public BuildingType(string nameCode, string imagePath, Formula hitPoints)
		{
			this.NameCode = nameCode;
			this.ImagePath = imagePath;
			
			this.HitPoints = hitPoints;
		}
	}
}
