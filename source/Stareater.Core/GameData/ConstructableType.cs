using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.Utils.StateEngine;
using Stareater.GameData.Construction;

namespace Stareater.GameData
{
	[StateTypeAttribute(true)]
	class ConstructableType : IIdentifiable
	{
		public string LanguageCode { get; private set; }
		public string ImagePath { get; private set; }
		
		public string IdCode { get; private set; }
		public IEnumerable<Prerequisite> Prerequisites { get; private set; }
		public SiteType ConstructableAt { get; private set; }
		public string StockpileGroup { get; private set; }
		
		public Formula Condition { get; private set; }
		public Formula Cost { get; private set; }
		public Formula TurnLimit { get; private set; }
		
		public IEnumerable<IConstructionEffect> Effects { get; private set; }
		
		public ConstructableType(string languageCode, string imagePath, string idCode, 
		                     IEnumerable<Prerequisite> prerequisites, SiteType constructableAt, string stockpileGroup,
		                     Formula condition, Formula cost, Formula turnLimit, IEnumerable<IConstructionEffect> effects)
		{
			this.LanguageCode = languageCode;
			this.ImagePath = imagePath;
			this.IdCode = idCode;
			this.Prerequisites = prerequisites;
			this.ConstructableAt = constructableAt;
			this.StockpileGroup = stockpileGroup;
			this.Condition = condition;
			this.Cost = cost;
			this.TurnLimit = turnLimit;
			this.Effects = effects;
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			var other = obj as ConstructableType;
			
			return (other != null) && this.IdCode == other.IdCode;
		}

		public override int GetHashCode()
		{
			return IdCode.GetHashCode();
		}

		public static bool operator ==(ConstructableType lhs, ConstructableType rhs) {
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(ConstructableType lhs, ConstructableType rhs) {
			return !(lhs == rhs);
		}

		#endregion
		
		public const string ShipStockpile = "ship";
	}
}
