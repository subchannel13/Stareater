using System;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;

namespace Stareater.Ships.Missions
{
	[StateType(saveTag: MissionTag)]
	class ColonizationMission : AMission
	{
		[StateProperty]
		public Planet Target { get; private set; }
		
		public ColonizationMission(Planet target)
		{
			this.Target = target;
		}

		private ColonizationMission()
		{ }

		#region implemented abstract members of AMission
		public override void Accept(IMissionVisitor visitor)
		{
			visitor.Visit(this);
		}

		public override bool Equals(object obj)
		{
			var other = obj as ColonizationMission;
			return other != null && this.Target == other.Target;
		}
		public override int GetHashCode()
		{
			return this.Target.GetHashCode();
		}
		#endregion
		
		public const string MissionTag = "Colonize";
	}
}
