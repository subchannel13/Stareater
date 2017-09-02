using Stareater.Utils.StateEngine;

namespace Stareater.Galaxy
{
	//TODO(v0.7) try to remove the need for this class
	class LocationBody
	{
		[StateProperty]
		public StarData Star { get; private set; }

		[StateProperty]
		public Planet Planet { get; private set; }

		public LocationBody(StarData star, Planet planet)
		{
			this.Star = star;
			this.Planet = planet;
		}
		
		public LocationBody(StarData star) : this(star, null)
		{ }

		private LocationBody()
		{ }

		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			return (obj is LocationBody) && Equals((LocationBody)obj);
		}

		public bool Equals(LocationBody other)
		{
			return object.Equals(this.Star, other.Star) && object.Equals(this.Planet, other.Planet);
		}

		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				if (Star != null)
					hashCode += 1000000007 * Star.GetHashCode();
				if (Planet != null)
					hashCode += 1000000009 * Planet.GetHashCode();
			}
			return hashCode;
		}

		public static bool operator ==(LocationBody lhs, LocationBody rhs) {
			return lhs.Equals(rhs);
		}

		public static bool operator !=(LocationBody lhs, LocationBody rhs) {
			return !(lhs == rhs);
		}
		#endregion
	}
}
