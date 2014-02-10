using System;

namespace Stareater.GameData
{
	public class BuildingType
	{
		public string NameCode { get; private set; }
		public string ImagePath { get; private set; }
		
		public BuildingType(string nameCode, string imagePath)
		{
			this.NameCode = nameCode;
			this.ImagePath = imagePath;
		}
	}
}
