using System;
using System.Linq;

namespace Stareater.Galaxy
{
	public partial class Planet
	{
		//TODO(v0.5): Make trait list readonly to view, consider making whole class private
		
		/*public const double AtmosphereDensityMin = -5;
		public const double AtmosphereDensityMax = 10;
		public const double GravityMin = -5;
		public const double GravityMax = 10;
		public const double RadiationMin = 0;
		public const double RadiationMax = 10;
		public const double TemperatureMin = -5;
		public const double TemperatureMax = 10;

		public const double OptimalCondition = 0;*/
		
		/*public Planet(StarData star, int position, PlanetType type, int size, int gravity, double atmosphereDensityFactor, double mineralsSurface, double mineralsDeep)
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
			/*this.AtmosphereDensity = atmosphereDensity;
			this.GravityDeviation = gravityDeviation;
			this.RadiationDeviation = radiationDeviation;
			this.TemperatureDeviation = temperatureDeviation;

			this.MineralsDeep = mineralsDeep;
			this.MineralsSurface = mineralsSurface;
		}*/

		/*public double AtmosphereDensity { get; private set; }
		public double GravityDeviation { get; private set; }
		public double RadiationDeviation { get; private set; }
		public double TemperatureDeviation { get; private set; }
	
		private double calcAtmosphereDensity(double factor)
		{
			//TODO(v0.5): specify how is atmosphere density calculated
			return Math.Round(Methods.Lerp(factor, AtmosphereDensityMin, AtmosphereDensityMax), MidpointRounding.AwayFromZero);
		}
		
		private double calcGravity()
		{
			//TODO(v0.5): specify how is gravity calculated
			return Math.Round(Size / 100, MidpointRounding.AwayFromZero) - 10;
		}
		
		private double calcRadiation()
		{
			//TODO(v0.5): specify how is radiation calculated
			return Star.Radiation - Position - (AtmosphereDensity + AtmosphereDensityMin);
		}
		
		private double calcTemperature()
		{
			//TODO(v0.5): specify how is temperature calculated
			double radiation = Math.Max(Star.Radiation - Position, 0);
			double absorbed = Math.Min(radiation, AtmosphereDensity + AtmosphereDensityMin);
			
			return Math.Round(absorbed + (AtmosphereDensity > OptimalCondition ? AtmosphereDensity / 2 : 0));
		}
		
		//TODO(v0.5): expose stuff like radiation absorption and green house effect to view
		*/

		/*internal Planet Copy(StarData starRemap)
		{
			return new Planet(starRemap, Position, Type, Size,
				AtmosphereDensity, GravityDeviation, RadiationDeviation, TemperatureDeviation,
				MineralsSurface, MineralsDeep);
		}*/
		
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
			unchecked {
				if (Star != null)
					hashCode += 1000000007 * Star.GetHashCode();
				hashCode += 1000000009 * Position.GetHashCode();
			}
			return hashCode;
		}

		public static bool operator ==(Planet lhs, Planet rhs) {
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(Planet lhs, Planet rhs) {
			return !(lhs == rhs);
		}
		#endregion
	}
}
