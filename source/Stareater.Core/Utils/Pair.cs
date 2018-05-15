using Stareater.Utils.StateEngine;
using System.Collections.Generic;

namespace Stareater.Utils
{
	//TODO(v0.7) add info class for Wormhole and make this private
	public class Pair<T>
	{
		[StateProperty]
		public T First { get; private set; }

		[StateProperty]
		public T Second { get; private set; }

		public Pair(T first, T second)
		{
			if (first.GetHashCode() < second.GetHashCode())
			{
				this.First = first;
				this.Second = second;
			}
			else
			{
				this.First = second;
				this.Second = first;
			}
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
			var hashCode = 43270662;
			hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(this.First);
			hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(this.Second);
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
