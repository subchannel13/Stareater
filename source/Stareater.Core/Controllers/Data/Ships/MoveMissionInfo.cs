using System;
using System.Collections.Generic;
using NGenerics.DataStructures.Mathematical;

namespace Stareater.Controllers.Data.Ships
{
	public class MoveMissionInfo : AFleetMission
	{
		public IEnumerable<Vector2D> Waypoints { get; private set; }
		
		internal MoveMissionInfo(IEnumerable<Vector2D> waypoints)
		{
			this.Waypoints = waypoints;
		}
		
		public override FleetMissionType Type {
			get {
				return FleetMissionType.Move;
			}
		}
	}
}
