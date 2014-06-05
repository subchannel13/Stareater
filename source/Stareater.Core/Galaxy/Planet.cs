 
using System;
using System.Linq;
using Stareater.GameData;

namespace Stareater.Galaxy 
{
	public partial class Planet 
	{
		public StarData Star { get; private set; }
		public int Position { get; private set; }
		public PlanetType Type { get; private set; }
		public double Size { get; private set; }
		public double MineralsSurface { get; private set; }
		public double MineralsDeep { get; private set; }

		public Planet(StarData star, int position, PlanetType type, double size, double mineralsSurface, double mineralsDeep) 
		{
			this.Star = star;
			this.Position = position;
			this.Type = type;
			this.Size = size;
			this.MineralsSurface = mineralsSurface;
			this.MineralsDeep = mineralsDeep;
 
		} 


		internal Planet Copy(GalaxyRemap galaxyRemap) 
		{
			return new Planet(galaxyRemap.Stars[this.Star], this.Position, this.Type, this.Size, this.MineralsSurface, this.MineralsDeep);
 
		} 
 
	}
}
