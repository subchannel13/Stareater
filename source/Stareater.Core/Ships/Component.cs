using System;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.GameData.Ships;
using Stareater.Utils.Collections;

namespace Stareater.Ships
{
	class Component<T> where T : AComponentType
	{
		public T TypeInfo { get; private set; }
		public int Level { get; private set; }
		
		public Component(T typeInfo, int level)
		{
			this.TypeInfo = typeInfo;
			this.Level = level;
		}
		
		public IkadnBaseObject Save()
		{
			var data = new IkonArray();
			
			data.Add(new IkonText(this.TypeInfo.IdCode));
			data.Add(new IkonInteger(this.Level));
			
			return data;
		}
		
		public static Component<T> Load(IkonArray rawData, ObjectDeindexer deindexer)
		{
			return new Component<T>(
				deindexer.Get<T>(rawData[TypeIndex].To<string>()),
				rawData[LevelIndex].To<int>()
			);
		}
		
		#region Saving keys
		private const int TypeIndex = 0;
		private const int LevelIndex = 1;
 		#endregion
	}
}
