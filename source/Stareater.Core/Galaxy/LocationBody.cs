using System;

namespace Stareater.Galaxy
{
	struct LocationBody
	{
		public StarData Star;
		public Planet Planet;
		
		public LocationBody(StarData star, Planet planet)
		{
			this.Star = star;
			this.Planet = planet;
		}
		
		public LocationBody(StarData star) : this(star, null)
		{ }
	}
}
