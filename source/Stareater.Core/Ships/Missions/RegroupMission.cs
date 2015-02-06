using System;
using Stareater.GameData;
using Stareater.Utils.Collections;

namespace Stareater.Ships.Missions
{
	class RegroupMission : AMission
	{
		public RegroupMission()
		{
		}
		
		public override MissionType Type {
			get {
				return MissionType.Regroup;
			}
		}
		
		public override AMission Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap)
		{
			//TODO(v0.5)
			throw new NotImplementedException();
		}
		
		public override Ikadn.IkadnBaseObject Save(ObjectIndexer indexer)
		{
			//TODO(v0.5)
			throw new NotImplementedException();
		}
	}
}
