using System;

namespace Stareater.Controllers.Views.Ships
{
	public class MissionsInfo
	{
		public WaypointInfo[] Waypoints { get; private set; }
		
		internal MissionsInfo(WaypointInfo[] waypoints)
		{
			this.Waypoints = waypoints;
		}
	}
}
