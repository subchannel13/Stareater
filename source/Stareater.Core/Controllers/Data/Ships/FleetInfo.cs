using System;
using System.Collections.Generic;
using System.Linq;

using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy;
using Stareater.Ships.Missions;
using Stareater.Utils;

namespace Stareater.Controllers.Data.Ships
{
	public class FleetInfo
	{
		public PlayerInfo Owner { get; private set; }
		public AFleetMission Mission { get; private set; }
		public Vector2D VisualPosition { get; private set; }
		
		internal Fleet Fleet { get; private set; }
		
		internal FleetInfo(Fleet fleet, AMission newMission, AMission oldMission, Game game, IVisualPositioner visualPositioner)
		{
			this.Fleet = fleet;

			this.Mission = makeMissionInfo(newMission);
			this.Owner = new PlayerInfo(fleet.Owner);
			
			var oldMissionInfo = oldMission != null ? makeMissionInfo(oldMission) : null;
			this.VisualPosition = visualPositioner.FleetPosition(fleet.Position, this.Mission, oldMissionInfo);
		}
		
		private static AFleetMission makeMissionInfo(AMission mission)
		{
			switch(mission.Type) {
				case MissionType.Move:
					return new MoveMissionInfo((mission as MoveMission).Waypoints);
				case MissionType.Stationary:
					return new StationaryMissionInfo((mission as StationaryMission).Location);
				default:
					throw new ArgumentOutOfRangeException("mission");
			}
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			FleetInfo other = obj as FleetInfo;
			if (other == null)
				return false;
			return object.Equals(this.Fleet, other.Fleet);
		}
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				if (Fleet != null)
					hashCode += 1000000007 * Fleet.GetHashCode();
			}
			return hashCode;
		}
		
		public static bool operator ==(FleetInfo lhs, FleetInfo rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(FleetInfo lhs, FleetInfo rhs)
		{
			return !(lhs == rhs);
		}
		#endregion
	}
}
