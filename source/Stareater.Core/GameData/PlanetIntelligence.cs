 
using System;

namespace Stareater.GameData 
{
	partial class PlanetIntelligence 
	{
		public double Explored { get; private set; }
		public int LastVisited { get; private set; }

		public PlanetIntelligence() 
		{
			this.Explored = Unexplored;
			this.LastVisited = NeverVisited;
 
		} 

		internal PlanetIntelligence(PlanetIntelligence original) : this() 
		{
			this.Explored = original.Explored;
			this.LastVisited = original.LastVisited;
 
		}

		internal PlanetIntelligence Copy()
		{
			return new PlanetIntelligence(this);
		}
 
	}
}
