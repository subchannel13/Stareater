using Stareater.GameData.Ships;
using Stareater.Utils.StateEngine;

namespace Stareater.Ships
{
	[StateTypeAttribute(saveTag: "Component")]
    class Component<T> where T : AComponentType
	{
		[StatePropertyAttribute]
		public T TypeInfo { get; private set; }
		[StatePropertyAttribute]
		public int Level { get; private set; }
		[StatePropertyAttribute]
		public int Quantity { get; private set; }
		
		public Component(T typeInfo, int level, int quantity = 1)
		{
			this.TypeInfo = typeInfo;
			this.Level = level;
			this.Quantity = quantity;
		}

		private Component()
		{ }
	}
}
