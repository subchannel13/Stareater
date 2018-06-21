using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using System.Collections.Generic;
using System.Linq;
using Stareater.Galaxy.BodyTraits;

namespace Stareater.Galaxy
{
	class Planet 
	{
		[StateProperty]
		public StarData Star { get; private set; }

		[StateProperty]
		public int Position { get; private set; }

		[StateProperty]
		public PlanetType Type { get; private set; }

		[StateProperty]
		public double Size { get; private set; }

		[StateProperty]
		public PendableSet<ITrait> Traits { get; private set; }

		public Planet(StarData star, int position, PlanetType type, double size, List<TraitType> traits) 
		{
			this.Star = star;
			this.Position = position;
			this.Type = type;
			this.Size = size;
			this.Traits = new PendableSet<ITrait>(traits.Select(x => x.Make()));
		} 
		
		private Planet() 
		{ }

		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			var other = obj as Planet;
			if (other == null)
				return false;
			return object.Equals(this.Star, other.Star) && this.Position == other.Position;
		}

		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked
			{
				if (Star != null)
					hashCode += 1000000007 * Star.GetHashCode();
				hashCode += 1000000009 * Position.GetHashCode();
			}
			return hashCode;
		}

		public static bool operator ==(Planet lhs, Planet rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(Planet lhs, Planet rhs)
		{
			return !(lhs == rhs);
		}
		#endregion
	}
}
