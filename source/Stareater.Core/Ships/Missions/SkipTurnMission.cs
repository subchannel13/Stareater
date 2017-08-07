using System;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.GameData;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;

namespace Stareater.Ships.Missions
{
	class SkipTurnMission : AMission
	{
		#region implemented abstract members of AMission
		public override void Accept(IMissionVisitor visitor)
		{
			visitor.Visit(this);
		}

		public override AMission Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap)
		{
			return new SkipTurnMission();
		}

		public override IkadnBaseObject Save(ObjectIndexer indexer)
		{
			return new IkonComposite(MissionTag);
		}

		public override IkadnBaseObject Save(SaveSession session)
		{
			return new IkonComposite(MissionTag);
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
		
		#region Saving keys
		public const string MissionTag = "Skip";
 		#endregion
	}
}
