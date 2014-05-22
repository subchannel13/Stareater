using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using NGenerics.DataStructures.Mathematical;
using Stareater.Localization.StarNames;

namespace Stareater.Galaxy
{
	public partial class StarData
	{
		public const int MaxPlanets = 8;

		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			StarData other = obj as StarData;
			if (other == null)
				return false;
			return object.Equals(this.Position, other.Position);
		}
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				if (Position != null)
					hashCode += 1000000007 * Position.GetHashCode();
			}
			return hashCode;
		}
		
		public static bool operator ==(StarData lhs, StarData rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(StarData lhs, StarData rhs)
		{
			return !(lhs == rhs);
		}
		#endregion
	}
}
