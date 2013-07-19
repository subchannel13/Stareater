using System;
using NGenerics.DataStructures.Mathematical;

namespace Stareater.Galaxy.Builders
{
	public class StarSystem
	{
		public StarData Star { get; private set; }
		public Planet[] Planets { get; private set; }
		
		public StarSystem(StarData star, Planet[] planets)
		{
			this.Star = star;
			this.Planets = planets;
		}
	}
}
