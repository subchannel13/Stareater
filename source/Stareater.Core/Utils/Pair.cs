using Stareater.Utils.StateEngine;
using System.Collections.Generic;

namespace Stareater.Utils
{
	//TODO(v0.8) add info class for Wormhole and make this private
	[StateTypeAttribute(saveTag:"Pair")]
	public class Pair<T>
	{
		[StatePropertyAttribute]
		public T First { get; private set; }

		[StatePropertyAttribute]
		public T Second { get; private set; }

		public Pair(T first, T second)
		{
			this.First = first;
			this.Second = second;
		}

		private Pair()
		{ }

		public bool Any(T item)
		{
			var comparer = EqualityComparer<T>.Default;
			return comparer.Equals(this.First, item) || comparer.Equals(this.Second, item);
		}

		public override bool Equals(object obj)
		{
			var other = obj as Pair<T>;
			var comparer = EqualityComparer<T>.Default;

			return 
				other != null && 
				(
					comparer.Equals(this.First, other.First) &&
					comparer.Equals(this.Second, other.Second)
				||
					comparer.Equals(this.First, other.Second) &&
					comparer.Equals(this.Second, other.First)
				);
		}

		public override int GetHashCode()
		{
			var firstCode = EqualityComparer<T>.Default.GetHashCode(this.First);
			var secondCode = EqualityComparer<T>.Default.GetHashCode(this.Second);
			if (firstCode > secondCode)
			{
				var temp = firstCode;
				firstCode = secondCode;
				secondCode = temp;
			}

			var hashCode = 43270662;
			hashCode = hashCode * -1521134295 + firstCode;
			hashCode = hashCode * -1521134295 + secondCode;
			return hashCode;
		}

		public static bool operator ==(Pair<T> lhs, Pair<T> rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (lhs is null || rhs is null)
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(Pair<T> lhs, Pair<T> rhs)
		{
			return !(lhs == rhs);
		}
	}
}
