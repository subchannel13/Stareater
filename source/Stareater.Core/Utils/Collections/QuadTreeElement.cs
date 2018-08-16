using Stareater.Utils.StateEngine;

namespace Stareater.Utils.Collections
{
	public class QuadTreeElement<T>
	{
		[StateProperty]
		public T Data { get; private set; }
		[StateProperty]
		public Vector2D TopRight { get; private set; }
		[StateProperty]
		public Vector2D BottomLeft { get; private set; }
	
		public QuadTreeElement(T data, Vector2D topRight, Vector2D bottomLeft)
		{
			this.Data = data;
			this.TopRight = topRight;
			this.BottomLeft = bottomLeft;
		}

		private QuadTreeElement()
		{ }
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			var other = obj as QuadTreeElement<T>;
			return other != null && object.Equals(this.Data, other.Data);
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
