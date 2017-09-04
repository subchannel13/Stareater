using Stareater.Utils.StateEngine;

namespace Stareater.Ships.Missions
{
	[StateBaseType("Load", typeof(MissionFactory))]
	abstract class AMission
	{
		public abstract void Accept(IMissionVisitor visitor);

		#region Equals and GetHashCode implementation
		public abstract override bool Equals(object obj);

		public abstract override int GetHashCode();

		public static bool operator ==(AMission lhs, AMission rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(AMission lhs, AMission rhs)
		{
			return !(lhs == rhs);
		}
		#endregion
	}
}
