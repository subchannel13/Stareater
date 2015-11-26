using System;
using System.Linq;

using NGenerics.DataStructures.Mathematical;
using Stareater.Controllers.Data;
using Stareater.Galaxy;
using Stareater.Utils;

namespace Stareater.Controllers.Views.Ships
{
	public class FleetInfo
	{
		public PlayerInfo Owner { get; private set; }
		public MissionsInfo Missions { get; private set; }
		public Vector2D VisualPosition { get; private set; }
		
		internal bool AtStar { get; private set; }
		internal Fleet FleetData { get; private set; }
		
		internal FleetInfo(Fleet fleet, bool atStar, IVisualPositioner visualPositioner)
		{
			this.AtStar = atStar;
			this.FleetData = fleet;

			this.Missions = MissionInfoFactory.Create(fleet);
			this.Owner = new PlayerInfo(fleet.Owner);
			
			this.VisualPosition = visualPositioner.FleetPosition(fleet.Position, this.Missions, atStar);
		}
		
		public bool IsMoving 
		{ 
			get { return this.Missions.Waypoints.Length > 0; }
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			var other = obj as FleetInfo;
			return other != null && object.Equals(this.FleetData, other.FleetData);
		}
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				if (this.FleetData != null)
					hashCode += 1000000007 * this.FleetData.GetHashCode();
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
