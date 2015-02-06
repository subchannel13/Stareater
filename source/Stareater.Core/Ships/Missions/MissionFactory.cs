using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ikadn;
using Stareater.Utils.Collections;

namespace Stareater.Ships.Missions
{
	static class MissionFactory
	{
		public static AMission Load(IkadnBaseObject rawData, ObjectDeindexer deindexer)
		{
			AMission mission = null;
			
			if (rawData.Tag.Equals(StationaryMission.MissionTag))
				mission = StationaryMission.Load(rawData, deindexer);
			else if (rawData.Tag.Equals(MoveMission.MissionTag))
				mission = MoveMission.Load(rawData, deindexer);
			//TODO(v0.5) regroup mission
			else
				throw new KeyNotFoundException("Unknown order type: " + rawData.Tag);
			
			return mission;
		}
	}
}
