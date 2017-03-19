using System;
using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.GameLogic;

namespace Stareater.GameData
{
	class Constructable
	{
		public string LanguageCode { get; private set; }
		public bool LiteralText { get; private set; }
		public string ImagePath { get; private set; }
		
		public string IdCode { get; private set; }
		public IEnumerable<Prerequisite> Prerequisites { get; private set; }
		public SiteType ConstructableAt { get; private set; }
		public bool IsVirtual { get; private set; }
		public string StockpileGroup { get; private set; }
		
		public Formula Condition { get; private set; }
		public Formula Cost { get; private set; }
		public Formula TurnLimit { get; private set; }
		
		public IEnumerable<IConstructionEffect> Effects { get; private set; }
		
		public Constructable(string languageCode, bool literalText, string imagePath, string idCode, 
		                     IEnumerable<Prerequisite> prerequisites, SiteType constructableAt, bool isVirtual, string stockpileGroup,
		                     Formula condition, Formula cost, Formula turnLimit, IEnumerable<IConstructionEffect> effects)
		{
			this.LanguageCode = languageCode;
			this.LiteralText = literalText;
			this.ImagePath = imagePath;
			this.IdCode = idCode;
			this.Prerequisites = prerequisites;
			this.ConstructableAt = constructableAt;
			this.IsVirtual = isVirtual;
			this.StockpileGroup = stockpileGroup;
			this.Condition = condition;
			this.Cost = cost;
			this.TurnLimit = turnLimit;
			this.Effects = effects;
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			var other = obj as Constructable;
			
			return (other != null) && this.IdCode == other.IdCode;
		}

		public override int GetHashCode()
		{
			return IdCode.GetHashCode();
		}

		public static bool operator ==(Constructable lhs, Constructable rhs) {
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(Constructable lhs, Constructable rhs) {
			return !(lhs == rhs);
		}

		#endregion
		
		public const string ShipStockpile = "ship";
	}
}
