using System;
using System.Collections.Generic;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.GameData;
using Stareater.Utils.Collections;

namespace Stareater.Ships.Missions
{
	abstract class AMission
	{
		public abstract void Accept(IMissionVisitor visitor);
		
		public abstract AMission Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap);
		public abstract IkadnBaseObject Save(ObjectIndexer indexer);
		
		#region Equals and GetHashCode implementation
		public abstract override bool Equals(object obj);
		
		public abstract override int GetHashCode();
		
		public static bool operator ==(AMission lhs, AMission rhs) {
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(AMission lhs, AMission rhs) {
			return !(lhs == rhs);
		}
		#endregion
	}
}
