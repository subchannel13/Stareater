using System.Collections.Generic;
using System.Drawing;
using Stareater.Galaxy.BodyTraits;
using Stareater.Localization.StarNames;
using Stareater.Utils;

namespace Stareater.Galaxy.Builders
{
	public class StarSystemBuilder
	{
		internal StarData Star { get; private set; }
		internal List<Planet> Planets { get; private set; }
		
		public StarSystemBuilder(Color color, float imageSizeScale, IStarName name, Vector2D position, List<TraitType> traits)
		{
			this.Star = new StarData(color, imageSizeScale, name, position, traits);
			this.Planets = new List<Planet>();
		}

		public void AddPlanet(int position, PlanetType type, double size, List<TraitType> traits)
		{
			this.Planets.Add(new Planet(this.Star, position, type, size, traits));
		}
	}
}
