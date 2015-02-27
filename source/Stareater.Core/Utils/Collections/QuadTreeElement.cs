using System;
using NGenerics.DataStructures.Mathematical;

namespace Stareater.Utils.Collections
{
	class QuadTreeElement<T>
	{
		public T Data { get; private set; }
		public Vector2D TopRight { get; private set; }
		public Vector2D BottomLeft { get; private set; }
	
		public QuadTreeElement(T data, Vector2D topRight, Vector2D bottomLeft)
		{
			this.Data = data;
			this.TopRight = topRight;
			this.BottomLeft = bottomLeft;
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			QuadTreeElement<T> other = obj as QuadTreeElement<T>;
			if (other == null)
				return false;
			return object.Equals(this.Data, other.Data);
		}
		
		public override int GetHashCode()
		{
			return Data.GetHashCode();
		}
		
		public static bool operator ==(QuadTreeElement<T> lhs, QuadTreeElement<T> rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(QuadTreeElement<T> lhs, QuadTreeElement<T> rhs)
		{
			return !(lhs == rhs);
		}
		#endregion

	}
}
