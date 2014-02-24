using System;
using Stareater.GameData;

namespace Stareater.Ships
{
	class Hull
	{
		private HullType type;
		private int level;
		
		public Hull(HullType type, int level)
		{
			this.type = type;
			this.level = level;
		}
	}
}
