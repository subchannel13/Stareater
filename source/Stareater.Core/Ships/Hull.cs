using System;
using Stareater.GameData;

namespace Stareater.Ships
{
	class Hull
	{
		public HullType TypeInfo { get; private set; }
		public int Level { get; private set; }
		
		private int imageIndex;
		
		public Hull(HullType type, int level, int imageIndex)
		{
			this.TypeInfo = type;
			this.Level = level;
			
			this.imageIndex = imageIndex;
		}
		
		public string ImagePath 
		{ 
			get
			{
				return TypeInfo.ImagePaths[imageIndex];
			}
		}
	}
}
