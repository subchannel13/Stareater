using System;

namespace Stareater.Controllers.Views
{
	public class FoundGalaxyObject
	{
		public GalaxyObjectType Type { get; private set; }
		public int ResultIndex { get; private set; }
		public double Distance { get; private set; }
		
		public FoundGalaxyObject(GalaxyObjectType type, int resultIndex, double distance)
		{
			this.Type = type;
			this.ResultIndex = resultIndex;
			this.Distance = distance;
		}
	}
}
