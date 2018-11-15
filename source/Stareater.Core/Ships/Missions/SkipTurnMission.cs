using Stareater.Utils.StateEngine;

namespace Stareater.Ships.Missions
{
	//TODO(v0.8) unused
	[StateType(saveTag: MissionTag)]
	class SkipTurnMission : AMission
	{
		#region implemented abstract members of AMission
		public override bool FullTurnAction
		{
			get { return true; }
		}

		public override void Accept(IMissionVisitor visitor)
		{
			visitor.Visit(this);
		}

		public override bool Equals(object obj)
		{
			return obj is SkipTurnMission;
		}

		public override int GetHashCode()
		{
			return 1;
		}
		#endregion
		
		public const string MissionTag = "Skip";
	}
}
