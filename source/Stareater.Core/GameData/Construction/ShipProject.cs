using Stareater.AppData.Expressions;
using Stareater.Ships;
using Stareater.Utils.StateEngine;
using System.Collections.Generic;
using System;

namespace Stareater.GameData.Construction
{
	[StateType(saveTag: Tag)]
	class ShipProject : IConstructionProject
	{
		[StateProperty]
		public Design Type { get; set; }

		public ShipProject(Design design)
		{
			this.Type = design;
		}

		private ShipProject()
		{ }

		public string StockpileGroup
		{
			get { return ConstructableType.ShipStockpile; }
		}

		public Formula Condition
		{
			get { return new Formula(true); }
		}

		public Formula Cost
		{
			get { return new Formula(this.Type.Cost); }
		}

		public Formula TurnLimit
		{
			get { return new Formula(double.PositiveInfinity); }
		}

		public bool IsVirtual
		{
			get { return false; }
		}

		public IEnumerable<IConstructionEffect> Effects
		{
			get { yield return new ConstructionAddShip(this.Type); }
		}

		public void Accept(IConstructionProjectVisitor visitor)
		{
			visitor.Visit(this);
		}

		#region Equals
		public override bool Equals(object obj)
		{
			var other = obj as ShipProject;

			if (other == null)
				return false;

			return this.Type.Equals(other.Type);
		}

		public override int GetHashCode()
		{
			return this.Type.GetHashCode();
		}
		
		public static bool operator ==(ShipProject lhs, ShipProject rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(ShipProject lhs, ShipProject rhs)
		{
			return !(lhs == rhs);
		}
		#endregion

		public const string Tag = "Ship";
	}
}
