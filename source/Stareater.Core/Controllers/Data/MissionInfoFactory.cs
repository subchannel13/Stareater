using System;
using System.Collections.Generic;
using Stareater.Controllers.Views.Ships;
using Stareater.Galaxy;
using Stareater.Ships.Missions;

namespace Stareater.Controllers.Data
{
	class MissionInfoFactory : IMissionVisitor
	{
		private Fleet fleet; 
		private AMissionInfo result = null;
		
		private MissionInfoFactory(Fleet fleet)
		{
			this.fleet = fleet;
		}
		
		public static AMissionInfo Create(LinkedList<AMission> mission, Fleet fleet)
		{
			if (mission.Count == 0)
				return new StationaryMissionInfo();
			
			var factory = new MissionInfoFactory(fleet);
			mission.First.Value.Accept(factory);
			return factory.result;
		}

		#region IMissionVisitor implementation

		public void Visit(MoveMission mission)
		{
			result = new MoveMissionInfo(mission.Waypoints);
		}

		#endregion
	}
}
