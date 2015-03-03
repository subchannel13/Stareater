using System;
using Stareater.Galaxy;

namespace Stareater.Controllers.Views.Ships
{
	public class StationaryMissionInfo : AFleetMission
	{
		public StarData Location { get; private set; }
		
		internal StationaryMissionInfo(StarData location)
		{
			this.Location = location;
		}
		
		public override FleetMissionType Type {
			get {
				return FleetMissionType.Stationary;
			}
		}
	}
}
