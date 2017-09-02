using Stareater.Utils.StateEngine;

namespace Stareater.GameData
{
	partial class PlanetIntelligence 
	{
		public const int NeverVisited = -1;

		[StateProperty]
		public int LastVisited { get; private set; }

		public PlanetIntelligence() 
		{
			this.LastVisited = NeverVisited;
 		}

		public bool Explored
		{
			get { return this.LastVisited != NeverVisited; }
		}

		public void Visit(int turn)
		{
			this.LastVisited = turn;
		}
	}
}
