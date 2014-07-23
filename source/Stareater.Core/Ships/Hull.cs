using System;
using Ikadn;
using Ikadn.Ikon.Types;
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
		
		public IkadnBaseObject Save()
		{
			var data = new IkonComposite(HullTag);
			
			data.Add(TypeKey, new IkonText(this.TypeInfo.IdCode));
			data.Add(LevelKey, new IkonInteger(this.Level));
			
			return data;
		}
		
		#region Saving keys
		private const string HullTag = "Hull"; 
		private const string TypeKey = "type";
		private const string LevelKey = "level";
 		#endregion
	}
}
