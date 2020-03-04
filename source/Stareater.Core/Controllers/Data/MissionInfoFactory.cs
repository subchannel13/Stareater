using System;
using System.Collections.Generic;
using Stareater.Controllers.Views.Ships;
using Stareater.Galaxy;
using Stareater.Ships.Missions;

namespace Stareater.Controllers.Data
{
	class MissionInfoFactory : IMissionVisitor
	{
		private readonly List<WaypointInfo> waypoints = new List<WaypointInfo>();
		
		private MissionInfoFactory()
		{ }
		
		public static MissionsInfo Create(Fleet fleet)
		{
			if (fleet.Missions.Count == 0)
				return new MissionsInfo(new WaypointInfo[0]);
			
			var factory = new MissionInfoFactory();
			foreach(var mission in fleet.Missions)
				mission.Accept(factory);
			
			return new MissionsInfo(factory.waypoints.ToArray());
		}

		#region IMissionVisitor implementation

		public void Visit(MoveMission mission)
		{
			//TODO(v0.8) hide waypoints for foreing fleets
			waypoints.Add(new WaypointInfo(mission.Start, mission.Destination, mission.UsedWormhole));
		}

		public void Visit(DisembarkMission mission)
		{
			//No operation
		}

		public void Visit(LoadMission mission)
		{
			//No operation
		}

		public void Visit(SkipTurnMission mission)
		{
			//TODO(later) not crucial but some implementation should be provided
		}
		
		#endregion
	}
}
