using System.Collections.Generic;

namespace Stareater.Utils
{
	public class Edge<T>
	{
		public Vertex<T> FirstEnd { get; private set; }
		public Vertex<T> SecondEnd { get; private set; }
		public double Weight { get; private set; }

		public Edge(Vertex<T> firstEnd, Vertex<T> secondEnd, double weight)
		{
			this.FirstEnd = firstEnd;
			this.SecondEnd = secondEnd;
			this.Weight = weight;
		}

		public override bool Equals(object obj)
		{
			var other = obj as Edge<T>;
			var comparer = EqualityComparer<Vertex<T>>.Default;

			return
				other != null &&
				(
					comparer.Equals(this.FirstEnd, other.FirstEnd) &&
					comparer.Equals(this.SecondEnd, other.SecondEnd)
				||
					comparer.Equals(this.FirstEnd, other.SecondEnd) &&
					comparer.Equals(this.SecondEnd, other.FirstEnd)
				);
		}

		public override int GetHashCode()
		{
			var comparer = EqualityComparer<Vertex<T>>.Default;
			var firstCode = comparer.GetHashCode(this.FirstEnd);
			var secondCode = comparer.GetHashCode(this.SecondEnd);
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

		public static bool operator ==(Edge<T> lhs, Edge<T> rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (lhs is null || rhs is null)
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(Edge<T> lhs, Edge<T> rhs)
		{
			return !(lhs == rhs);
		}
	}
}
