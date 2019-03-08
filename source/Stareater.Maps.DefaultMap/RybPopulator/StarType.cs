using System.Drawing;

namespace Stareater.Maps.DefaultMap.RybPopulator
{
	class StarType
	{
		public Color Hue { get; private set; }
		public double MinScale { get; private set; }
		public double Maxscale { get; private set; }
		public string[] Traits { get; private set; }

		public StarType(Color hue, double minScale, double maxscale, string[] traits)
		{
			this.Hue = hue;
			this.MinScale = minScale;
			this.Maxscale = maxscale;
			this.Traits = traits;
		}
	}
}
