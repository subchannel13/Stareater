using System;
using System.Collections.Generic;
using NGenerics.DataStructures.Mathematical;

namespace Stareater.Controllers.Views.Ships
{
	public class MoveMissionInfo : AFleetMission
	{
		public Vector2D[] Waypoints { get; private set; }
		
		internal MoveMissionInfo(Vector2D[] waypoints)
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
