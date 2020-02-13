using Ikadn.Ikon.Types;
using Stareater.Utils.StateEngine;
using System.Collections.Generic;
using Stareater.Players;
using Stareater.GameData;

namespace Stareater.Galaxy
{
	[StateBaseTypeAttribute("Load", typeof(AConstructionSite))]
	abstract class AConstructionSite 
	{
		[StatePropertyAttribute]
		public LocationBody Location { get; private set; }

		[StatePropertyAttribute]
		public Player Owner { get; private set; }

		[StatePropertyAttribute]
		public Dictionary<string, double> Buildings { get; private set; }

		[StatePropertyAttribute]
		public Dictionary<string, double> Stockpile { get; private set; }

		protected AConstructionSite(LocationBody location, Player owner) 
		{
			this.Location = location;
			this.Owner = owner;
			this.Buildings = new Dictionary<string, double>();
			this.Stockpile = new Dictionary<string, double>();
 
			#if DEBUG
			this.id = nextId();
			#endif
		} 
		
		protected AConstructionSite() 
		{ }

		public abstract SiteType Type { get; }

		public static AConstructionSite Load(IkonBaseObject rawData, LoadSession session)
		{
			if (rawData.Tag.Equals(Colony.Tag))
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
			unchecked
			{
				hashCode += 1000000007 * Location.GetHashCode();
				if (Owner != null)
					hashCode += 100000009 * Owner.GetHashCode();
			}
			return hashCode;
		}

		public static bool operator ==(AConstructionSite lhs, AConstructionSite rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(AConstructionSite lhs, AConstructionSite rhs)
		{
			return !(lhs == rhs);
		}
		#endregion

		#region object ID
		#if DEBUG
		private long id;

		public override string ToString()
		{
			return "AConstructionSite " + id;
		}

		private static long LastId = 0;
		private static readonly object LockObj = new object();

		private static long nextId()
		{
			lock (LockObj) {
				LastId++;
				return LastId;
			}
		}
		#endif
		#endregion
	}
}
