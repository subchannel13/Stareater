using Stareater.Utils.StateEngine;

namespace Stareater.GameData
{
	class PlanetIntelligence 
	{
		public const int NeverVisited = -1;

		[StatePropertyAttribute]
		public int LastVisited { get; set; }

		[StatePropertyAttribute]
		public bool Discovered { get; set; }

		public PlanetIntelligence() 
		{
			this.LastVisited = NeverVisited;
 		}
	}
}
