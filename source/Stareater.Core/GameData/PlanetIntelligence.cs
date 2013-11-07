using System;

namespace Stareater.GameData
{
	public class PlanetIntelligence
	{
		public const int NeverVisited = -1;
		public const double Unexplored = 0;
		public const double FullyExplored = 1;
		
		public double Explored { get; private set; }
		public int LastVisited { get; private set; }
		
		public PlanetIntelligence()
		{
			this.Explored = Unexplored;
			this.LastVisited = NeverVisited;
		}
		
		public double Explore(double fraction)
		{
			this.Explored += fraction;
			
			if (this.Explored > FullyExplored) {
				double extra = this.Explored - FullyExplored;
				this.Explored = FullyExplored;
				
				return extra;
			}
			
			return 0;
		}
		
		public void Visit(int turn)
		{
			this.LastVisited = turn;
		}

		public PlanetIntelligence Copy()
		{
			PlanetIntelligence copy = new PlanetIntelligence();

			copy.Explored = this.Explored;
			copy.LastVisited = this.LastVisited;

			return copy;
		}
	}
}
