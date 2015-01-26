using System;
using System.Collections.Generic;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.GameData;
using Stareater.Utils.Collections;

namespace Stareater.Ships.Missions
{
	abstract class AMission
	{
		public abstract MissionType Type { get; }
		
		public abstract AMission Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap);
		public abstract IkadnBaseObject Save(ObjectIndexer indexer);
	}
}
