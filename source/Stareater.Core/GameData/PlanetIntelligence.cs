using Stareater.Utils.StateEngine;

namespace Stareater.GameData
{
	class PlanetIntelligence 
	{
		public const int NeverVisited = -1;

		[StatePropertyAttribute]
		public int LastVisited { get; private set; }

		[StatePropertyAttribute]
		public bool Discovered { get; set; }

		public PlanetIntelligence() 
		{
			this.LastVisited = NeverVisited;
 		}

		//TODO(v0.9) check validity
		public bool Explored
		{
			get { return this.LastVisited != NeverVisited; }
		}

		//TODO(v0.9) check validity, if unnecessary then make properties with public set
		public void Visit(int turn)
		{
			this.LastVisited = turn;
		}
	}
}
