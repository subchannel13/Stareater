using System;

namespace Stareater.GameData.Ships
{
	abstract class AAbilityType
	{
		public string ImagePath { get; private set; }

		protected AAbilityType(string imagePath)
		{
			this.ImagePath = imagePath;
		}
		
		public abstract void Accept(IAbilityVisitor visitor);
	}
}
