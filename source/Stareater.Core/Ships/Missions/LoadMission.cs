using Stareater.Utils.StateEngine;

namespace Stareater.Ships.Missions
{
	[StateType(saveTag: MissionTag)]
	class LoadMission : AMission
	{
		public override void Accept(IMissionVisitor visitor)
		{
			visitor.Visit(this);
		}

		public override bool Equals(object obj)
		{
			return obj is LoadMission;
		}

		public override int GetHashCode()
		{
			return 1;
		}

		public const string MissionTag = "Load";
	}
}
