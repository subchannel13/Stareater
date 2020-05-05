using Stareater.GameData.Ships;
using Stareater.Utils.StateEngine;
using System;

namespace Stareater.Ships
{
	[StateTypeAttribute(saveTag: "Component")]
    class Component<T> : IEquatable<Component<T>> where T : AComponentType
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

		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			return obj is Component<T> other ? this.Equals(other) : false;
		}

		public bool Equals(Component<T> other)
		{
			return this.TypeInfo.Equals(other.TypeInfo) &&
				this.Level == other.Level &&
				this.Quantity == other.Quantity;
		}

		public override int GetHashCode()
		{
			return this.TypeInfo.GetHashCode() * 101 + this.Level;
		}

		public static bool operator ==(Component<T> lhs, Component<T> rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (lhs is null || rhs is null)
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(Component<T> lhs, Component<T> rhs)
		{
			return !(lhs == rhs);
		}
		#endregion
	}
}
