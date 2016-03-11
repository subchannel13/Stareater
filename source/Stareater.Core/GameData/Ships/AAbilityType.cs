using System;

namespace Stareater.GameData.Ships
{
	abstract class AAbilityType
	{
		public string ImagePath { get; private set; }
		//TODO(later) energy cost
		
		protected AAbilityType(string imagePath)
		{
			this.ImagePath = imagePath;
		}
	}
}
