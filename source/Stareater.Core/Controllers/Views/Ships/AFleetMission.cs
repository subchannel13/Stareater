using System;
using Stareater.Ships.Missions;

namespace Stareater.Controllers.Views.Ships
{
	public abstract class AFleetMission
	{
		public abstract FleetMissionType Type { get; }
	}
}
