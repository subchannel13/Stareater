using System;
using Stareater.Galaxy;

namespace Stareater.Controllers.Views.Ships
{
	public class StationaryMissionInfo : AMissionInfo
	{
		public override FleetMissionType Type {
			get {
				return FleetMissionType.Stationary;
			}
		}
	}
}
