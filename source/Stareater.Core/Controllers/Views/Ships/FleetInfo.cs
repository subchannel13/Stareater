using System;
using System.Collections.Generic;
using System.Linq;

using NGenerics.DataStructures.Mathematical;
using Stareater.Controllers.Data;
using Stareater.Galaxy;
using Stareater.Ships.Missions;
using Stareater.Utils;

namespace Stareater.Controllers.Views.Ships
{
	public class FleetInfo
	{
		public PlayerInfo Owner { get; private set; }
		public AMissionInfo Mission { get; private set; }
		public Vector2D VisualPosition { get; private set; }
		
		internal bool AtStar { get; private set; }
		internal Fleet FleetData { get; private set; }
		
		internal FleetInfo(Fleet fleet, bool atStar, IVisualPositioner visualPositioner)
		{
			this.AtStar = atStar;
			this.FleetData = fleet;

			this.Mission = MissionInfoFactory.Create(fleet.Mission, fleet);
			this.Owner = new PlayerInfo(fleet.Owner);
			
			this.VisualPosition = visualPositioner.FleetPosition(fleet.Position, this.Mission, atStar);
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			FleetInfo other = obj as FleetInfo;
			if (other == null)
				return false;
			return object.Equals(this.FleetData, other.FleetData);
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
