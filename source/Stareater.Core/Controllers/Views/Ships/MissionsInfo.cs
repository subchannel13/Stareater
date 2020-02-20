using System;
using System.Collections.ObjectModel;

namespace Stareater.Controllers.Views.Ships
{
	public class MissionsInfo
	{
		public ReadOnlyCollection<WaypointInfo> Waypoints { get; private set; }
		
		internal MissionsInfo(WaypointInfo[] waypoints)
		{
			this.Waypoints = Array.AsReadOnly(waypoints);
		}
	}
}
