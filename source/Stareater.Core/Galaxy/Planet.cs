using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Utils;

namespace Stareater.Galaxy
{
	public class Planet
	{
		public double AtmosphereDensityMin = -5;
		public double AtmosphereDensityMax = 10;
		public double AtmosphereQualityMin = -10;
		public double AtmosphereQualityMax = 0;
		public double GravityMin = -5;
		public double GravityMax = 10;
		public double RadiationMin = 0;
		public double RadiationMax = 10;
		public double TemperatureMin = -5;
		public double TemperatureMax = 10;
		
		public double OptimalCondition = 0;
		
		public double Size { get; private set; }
		public double MineralsSurface { get; private set; }
		public double MineralsDeep { get; private set; }

		public StarData Star { get; private set; }
		public int Position { get; private set; }

		public Planet(StarData star, int position, int size, double atmosphereDensityFactor, double atmosphereQuality, double mineralsSurface, double mineralsDeep)
		{
			this.Star = star;
			this.Position = position;
			
			this.Size = size;
			this.AtmosphereDensity = Methods.Clamp(calcAtmosphereDensity(atmosphereDensityFactor), AtmosphereDensityMin, AtmosphereDensityMax);
			this.AtmosphereQuality = Methods.Clamp(atmosphereQuality, AtmosphereQualityMin, AtmosphereQualityMax);
			this.GravityDeviation = Methods.Clamp(calcGravity(), GravityMin, GravityMax);
			this.RadiationDeviation = Methods.Clamp(calcRadiation(), RadiationMin, RadiationMax);
			this.TemperatureDeviation = Methods.Clamp(calcTemperature(), TemperatureMin, TemperatureMax);
		}
		
		public double AtmosphereDensity { get; private set; }
		public double AtmosphereQuality { get; private set; }
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
	}
}
