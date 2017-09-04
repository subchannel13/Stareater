using System;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.GameData;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;

namespace Stareater.Ships.Missions
{
	[StateType(saveTag: MissionTag)]
	class SkipTurnMission : AMission
	{
		#region implemented abstract members of AMission
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
