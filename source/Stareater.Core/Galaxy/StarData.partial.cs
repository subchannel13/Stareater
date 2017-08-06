using System;
using System.Linq;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.Localization.StarNames;
using Stareater.Utils.StateEngine;

namespace Stareater.Galaxy
{
	public partial class StarData
	{
		public const int MaxPlanets = 8;

		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			var other = obj as StarData;
			return other != null && object.Equals(this.Position, other.Position);
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
		
		public static IStarName loadName(IkadnBaseObject rawData)
		{
			return rawData.Tag.Equals(ConstellationStarName.SaveTag) ? 
				ConstellationStarName.Load(rawData.To<IkonComposite>()) : 
				ProperStarName.Load(rawData.To<IkonComposite>());
		}

		public static IStarName loadName(IkadnBaseObject rawData, LoadSession session)
		{
			return rawData.Tag.Equals(ConstellationStarName.SaveTag) ?
				ConstellationStarName.Load(rawData.To<IkonComposite>()) :
				ProperStarName.Load(rawData.To<IkonComposite>());
		}
	}
}
