using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Utils;

namespace Stareater.Galaxy
{
	public class Planet
	{
		public const double AtmosphereDensityMin = -5;
		public const double AtmosphereDensityMax = 10;
		public const double GravityMin = -5;
		public const double GravityMax = 10;
		public const double RadiationMin = 0;
		public const double RadiationMax = 10;
		public const double TemperatureMin = -5;
		public const double TemperatureMax = 10;

		public const double OptimalCondition = 0;
		
		public PlanetType Type { get; private set; }
		public double Size { get; private set; }
		public double MineralsSurface { get; private set; }
		public double MineralsDeep { get; private set; }

		public StarData Star { get; private set; }
		public int Position { get; private set; }

		public Planet(StarData star, int position, PlanetType type, int size, int gravity, double atmosphereDensityFactor, double mineralsSurface, double mineralsDeep)
		{
			this.Star = star;
			this.Position = position;
			this.Type = type;
			
			this.Size = size;
			this.AtmosphereDensity = Methods.Clamp(calcAtmosphereDensity(atmosphereDensityFactor), AtmosphereDensityMin, AtmosphereDensityMax);
			this.GravityDeviation = Methods.Clamp(calcGravity(), GravityMin, GravityMax);
			this.RadiationDeviation = Methods.Clamp(calcRadiation(), RadiationMin, RadiationMax);
			this.TemperatureDeviation = Methods.Clamp(calcTemperature(), TemperatureMin, TemperatureMax);
		}

		internal Planet(StarData star, int position, PlanetType type, double size, 
			double atmosphereDensity, double gravityDeviation, double radiationDeviation, double temperatureDeviation,
			double mineralsSurface, double mineralsDeep)
		{
			this.Star = star;
			this.Position = position;
			this.Type = type;

			this.Size = size;
			this.AtmosphereDensity = atmosphereDensity;
			this.GravityDeviation = gravityDeviation;
			this.RadiationDeviation = radiationDeviation;
			this.TemperatureDeviation = temperatureDeviation;

			this.MineralsDeep = mineralsDeep;
			this.MineralsSurface = mineralsSurface;
		}

		public double AtmosphereDensity { get; private set; }
		public double GravityDeviation { get; private set; }
		public double RadiationDeviation { get; private set; }
		public double TemperatureDeviation { get; private set; }
	
		private double calcAtmosphereDensity(double factor)
		{
			//TODO: specify how is atmosphere density calculated
			return Math.Round(Methods.Lerp(factor, AtmosphereDensityMin, AtmosphereDensityMax), MidpointRounding.AwayFromZero);
		}
		
		private double calcGravity()
		{
			//TODO: specify how is gravity calculated
			return Math.Round(Size / 100, MidpointRounding.AwayFromZero) - 10;
		}
		
		private double calcRadiation()
		{
			//TODO: specify how is radiation calculated
			return Star.Radiation - Position - (AtmosphereDensity + AtmosphereDensityMin);
		}
		
		private double calcTemperature()
		{
			//TODO: specify how is temperature calculated
			double radiation = Math.Max(Star.Radiation - Position, 0);
			double absorbed = Math.Min(radiation, AtmosphereDensity + AtmosphereDensityMin);
			
			return Math.Round(absorbed + (AtmosphereDensity > OptimalCondition ? AtmosphereDensity / 2 : 0));
		}
		
		//TODO: expose stuff like radiation absorption and green house effect to view

		internal Planet Copy(StarData starRemap)
		{
			return new Planet(starRemap, Position, Type, Size,
				AtmosphereDensity, GravityDeviation, RadiationDeviation, TemperatureDeviation,
				MineralsSurface, MineralsDeep);
		}
	}
}
