using System.Drawing;

namespace Stareater.Maps.DefaultMap.RybPopulator
{
	class StarType
	{
		public Color Hue { get; private set; }
		public double MinRadiation { get; private set; }
		public double MaxRadiation { get; private set; }

		public StarType(Color color, double minRadiation, double maxRadiation)
		{
			this.Hue = color;
			this.MinRadiation = minRadiation;
			this.MaxRadiation = maxRadiation;
		}
	}
}
