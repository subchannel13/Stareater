using System;
using Ikadn;
using Stareater.GameData;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;

namespace Stareater.Ships.Missions
{
	[StateType(saveMethod: "Save", loaderClass: typeof(MissionFactory), loadMethod: "Load")]
	abstract class AMission
	{
		public abstract void Accept(IMissionVisitor visitor);

		public abstract AMission Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap);
		public abstract IkadnBaseObject Save(ObjectIndexer indexer);
		public abstract IkadnBaseObject Save(SaveSession session);

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
