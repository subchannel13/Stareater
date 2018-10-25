using Stareater.Utils.StateEngine;
using System;

namespace Stareater.Ships.Missions
{
	[StateType(saveTag: MissionTag)]
	class DisembarkMission : AMission
	{
		public override void Accept(IMissionVisitor visitor)
		{
			visitor.Visit(this);
		}

		public override bool Equals(object obj)
		{
			return obj is DisembarkMission;
		}

		public override int GetHashCode()
		{
			return 3;
		}

		public const string MissionTag = "Disembark";
	}
}
