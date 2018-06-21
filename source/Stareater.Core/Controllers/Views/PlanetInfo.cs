using System.Collections.Generic;
using Stareater.Galaxy;

namespace Stareater.Controllers.Views
{
	public class PlanetInfo
	{
		internal Planet Data { get; private set; }
		
		internal PlanetInfo(Planet data)
		{
			this.Data = data;
		}

		public PlanetType Type
		{
			get { return this.Data.Type; }
		}

		public int Position
		{
			get { return this.Data.Position; }
		}

		public StarInfo HostStar
		{
			get { return new StarInfo(this.Data.Star); }
		}

		public override bool Equals(object obj)
		{
			var other = obj as PlanetInfo;
			return other != null &&
				   EqualityComparer<Planet>.Default.Equals(this.Data, other.Data);
		}

		public override int GetHashCode()
		{
			return this.Data.GetHashCode();
		}

		public static bool operator ==(PlanetInfo info1, PlanetInfo info2)
		{
			return EqualityComparer<PlanetInfo>.Default.Equals(info1, info2);
		}

		public static bool operator !=(PlanetInfo info1, PlanetInfo info2)
		{
			return !(info1 == info2);
		}
	}
}
