using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ikadn;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;

namespace Stareater.Ships.Missions
{
	static class MissionFactory
	{
		public static AMission Load(IkadnBaseObject rawData, ObjectDeindexer deindexer)
		{
			AMission mission = null;
			
			if (rawData.Tag.Equals(MoveMission.MissionTag))
				mission = MoveMission.Load(rawData, deindexer);
			else
				throw new KeyNotFoundException("Unknown order type: " + rawData.Tag);
			
			return mission;
		}

		public static AMission Load(IkadnBaseObject rawData, LoadSession session)
		{
			AMission mission = null;

			if (rawData.Tag.Equals(MoveMission.MissionTag))
				mission = MoveMission.Load(rawData, session);
			else if (rawData.Tag.Equals(ColonizationMission.MissionTag))
				mission = ColonizationMission.Load(rawData, session);
			else
				throw new KeyNotFoundException("Unknown order type: " + rawData.Tag);

			return mission;
		}
	}
}
