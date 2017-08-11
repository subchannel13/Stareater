using System;
using System.Collections.Generic;
using System.Linq;

using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.GameData;
using Stareater.Players;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;

namespace Stareater.Galaxy
{
	[StateBaseType("Load")]
	abstract partial class AConstructionSite
	{
		public abstract SiteType Type { get; }

		public static AConstructionSite Load(IkadnBaseObject rawData, LoadSession session)
		{
			if (rawData.Tag.Equals("Colony")) //TODO(v0.7) use constant from Colony class
				return session.Load<Colony>(rawData);
			else
				return session.Load<StellarisAdmin>(rawData);
		}

		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			var other = obj as AConstructionSite;
				if (other == null)
					return false;
						return this.Location == other.Location && object.Equals(this.Owner, other.Owner);
		}

		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				hashCode += 1000000007 * Location.GetHashCode();
				if (Owner != null)
					hashCode += 100000009 * Owner.GetHashCode();
			}
			return hashCode;
		}

		public static bool operator ==(AConstructionSite lhs, AConstructionSite rhs) {
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(AConstructionSite lhs, AConstructionSite rhs) {
			return !(lhs == rhs);
		}
		#endregion
	}
}
