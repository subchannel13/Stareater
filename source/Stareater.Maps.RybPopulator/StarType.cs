using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Stareater.Maps.RybPopulator
{
	class StarType
	{
		private Color color;
		private double minRadiation;
		private double maxRadiation;

		public StarType(Color color, double minRadiation, double maxRadiation)
		{
			this.color = color;
			this.minRadiation = minRadiation;
			this.maxRadiation = maxRadiation;
		}
	}
}
