 
using System;
using System.Linq;

namespace Stareater.Galaxy 
{
	public partial class Planet 
	{
		public StarData Star;
		public int Position;
		public PlanetType Type;
		public double Size;

		public Planet(StarData star, int position, PlanetType type, double size) 
		{
			this.Star = star;
			this.Position = position;
			this.Type = type;
			this.Size = size;
		}
		
		internal Planet Copy(GalaxyRemap galaxyRemap)
		{
			return new Planet(galaxyRemap.Stars[this.Star], this.Position, this.Type, this.Size);
		}
	}
}
