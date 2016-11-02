using System;
using Stareater.AppData.Expressions;

namespace Stareater.GameData
{

	class DevelopmentTopic
	{
		public const string LevelKey = "lvl";
		public const string PriorityKey = "priority";
		
		public string NameCode { get; private set; }
		public string DescriptionCode{ get; private set; }
		
		public string ImagePath { get; private set; }
		public string IdCode { get; private set; }
		public Formula Cost { get; private set; }
		public int MaxLevel { get; private set; }
		
		public DevelopmentTopic(string nameCode, string descriptionCode, string imagePath, string code, Formula cost, int maxLevel)
		{
			this.NameCode = nameCode;
			this.DescriptionCode = descriptionCode;
			this.ImagePath = imagePath;
			this.IdCode = code;
			this.Cost = cost;
			this.MaxLevel = maxLevel;
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			var other = obj as DevelopmentTopic;
			return other != null && this.IdCode == other.IdCode;
		}
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				if (IdCode != null)
					hashCode += 1000000007 * IdCode.GetHashCode();
			}
			return hashCode;
		}
		
		public static bool operator ==(DevelopmentTopic lhs, DevelopmentTopic rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(DevelopmentTopic lhs, DevelopmentTopic rhs)
		{
			return !(lhs == rhs);
		}
		#endregion

	}
}
