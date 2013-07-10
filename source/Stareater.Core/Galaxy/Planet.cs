using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Galaxy
{
	class Planet
	{
		private double size;
		private double atmosphereDensity;
		private double atmosphereQuality;
		private double gravityDeviation;
		private double radiationDeviation;
		private double temperatureDeviation;
		private double mineralsSurface;
		private double mineralsDeep;

		public StarData Star { get; private set; }

		public Planet(StarData star)
		{
			this.Star = star;
		}
	}
}
