using System;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.GameData;
using Stareater.Utils.Collections;

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
			data.Add(ImageKey, new IkonInteger(this.imageIndex));
			
			return data;
		}
		
		public static Hull Load(IkonComposite rawData, ObjectDeindexer deindexer)
		{
			return new Hull(
				deindexer.Get<HullType>(rawData[TypeKey].To<string>()),
				rawData[LevelKey].To<int>(),
				rawData[ImageKey].To<int>()
			);
		}
		
		#region Saving keys
		private const string HullTag = "Hull"; 
		private const string TypeKey = "type";
		private const string LevelKey = "level";
		private const string ImageKey = "image";
 		#endregion
	}
}
