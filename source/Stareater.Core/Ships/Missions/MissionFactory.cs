using Ikadn;
using System.Collections.Generic;
using Stareater.Utils.StateEngine;

namespace Stareater.Ships.Missions
{
	static class MissionFactory
	{
		public static AMission Load(IkadnBaseObject rawData, LoadSession session)
		{
			if (rawData.Tag.Equals(MoveMission.MissionTag))
				return session.Load<MoveMission>(rawData);
			else if (rawData.Tag.Equals(ColonizationMission.MissionTag))
				return session.Load<ColonizationMission>(rawData);
			else if (rawData.Tag.Equals(SkipTurnMission.MissionTag))
				return new SkipTurnMission();
			else if (rawData.Tag.Equals(LoadMission.MissionTag))
				return new LoadMission();
			else
				throw new KeyNotFoundException("Unknown order type: " + rawData.Tag);
		}
	}
}
