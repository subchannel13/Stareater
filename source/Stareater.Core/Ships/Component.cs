using System;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.GameData.Ships;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;

namespace Stareater.Ships
{
	[StateType(saveMethod: "Save", loadMethod: "Load")]
    class Component<T> where T : AComponentType
	{
		[StateProperty]
		public T TypeInfo { get; private set; }
		[StateProperty]
		public int Level { get; private set; }
		[StateProperty]
		public int Quantity { get; private set; }
		
		public Component(T typeInfo, int level, int quantity = 1)
		{
			this.TypeInfo = typeInfo;
			this.Level = level;
			this.Quantity = quantity;
		}

		private Component()
		{ }

		public IkadnBaseObject Save(SaveSession session)
		{
			return Save();
		}

		public IkadnBaseObject Save()
		{
			var data = new IkonArray();
			
			data.Add(new IkonText(this.TypeInfo.IdCode));
			data.Add(new IkonInteger(this.Level));
			
			if (this.TypeInfo.CanHaveMultiple)
				data.Add(new IkonInteger(this.Quantity));
			
			return data;
		}

		public static Component<T> Load(IkonArray rawData, LoadSession session)
		{
			return new Component<T>(
				session.Deindexer.Get<T>(rawData[TypeIndex].To<string>()),
				rawData[LevelIndex].To<int>(),
				QuantityIndex < rawData.Count ? rawData[QuantityIndex].To<int>() : 1
			);
		}

		public static Component<T> Load(IkonArray rawData, ObjectDeindexer deindexer)
		{
			return new Component<T>(
				deindexer.Get<T>(rawData[TypeIndex].To<string>()),
				rawData[LevelIndex].To<int>(),
				QuantityIndex < rawData.Count ? rawData[QuantityIndex].To<int>() : 1
			);
		}
		
		#region Saving keys
		private const int TypeIndex = 0;
		private const int LevelIndex = 1;
		private const int QuantityIndex = 2;
 		#endregion
	}
}
