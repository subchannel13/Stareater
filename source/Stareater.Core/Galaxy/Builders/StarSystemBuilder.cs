using System;
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
		
		public StarSystemBuilder(Color color, float imageSizeScale, IStarName name, Vector2D position, List<StarTraitType> traits)
		{
			this.Star = new StarData(color, imageSizeScale, name, position, traits);
			this.Planets = new List<Planet>();
		}

		public StarSystemBuilder(StarSystemBuilder baseSystem)
		{
			if (baseSystem == null)
				throw new ArgumentNullException(nameof(baseSystem));

			this.Star = baseSystem.Star; //TODO(v0.8) deep copy if not null
			this.Planets = new List<Planet>(baseSystem.Planets);
		}

		/// <summary>
		/// Create dummy star system for evaluation purposes
		/// </summary>
		/// <param name="traits">Host star traits</param>
		public StarSystemBuilder(List<StarTraitType> starTraits) : this (Color.White, 1, null, new Vector2D(), starTraits)
		{ }

		public void AddPlanet(int position, PlanetType type, double size, IEnumerable<PlanetTraitType> traits)
		{
			this.Planets.Add(new Planet(this.Star, position, type, size, traits));
		}
	}
}
