using System;

namespace Stareater.GameData
{
	partial class PlanetIntelligence
	{
		public const int NeverVisited = -1;
		public const double Unexplored = 0;
		public const double FullyExplored = 1;
				
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
	}
}
