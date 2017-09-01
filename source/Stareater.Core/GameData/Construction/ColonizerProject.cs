using Stareater.Utils.StateEngine;
using System;
using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.Ships;
using Stareater.GameData.Databases.Tables;

namespace Stareater.GameData.Construction
{
	[StateType(saveTag: Tag)]
	class ColonizerProject : IConstructionProject
	{
		[StateProperty]
		public Design Colonizer { get; set; }

		[StateProperty]
		public ColonizationPlan Plan { get; set; }

		public ColonizerProject(Design desing, ColonizationPlan plan)
		{
			this.Colonizer = desing;
			this.Plan = plan;
		}

		public Formula Condition
		{
			get { return new Formula(true); }
		}

		public Formula Cost
		{
			get { return new Formula(this.Colonizer.Cost); }
		}

		public IEnumerable<IConstructionEffect> Effects
		{
			get
			{
				yield return new ConstructionAddColonizer(this.Colonizer, this.Plan.Destination);
            }
		}

		public bool IsVirtual
		{
			get { return true; }
		}

		public string StockpileGroup
		{
			get { return ConstructableType.ShipStockpile; }
		}

		public Formula TurnLimit
		{
			get { return new Formula(double.PositiveInfinity); }
		}

		public void Accept(IConstructionProjectVisitor visitor)
		{
			visitor.Visit(this);
		}

		#region Equals
		public override bool Equals(object obj)
		{
			var other = obj as ColonizerProject;

			if (other == null)
				return false;

			return this.Colonizer.Equals(other.Colonizer) && this.Plan.Equals(other.Plan);
		}

		public override int GetHashCode()
		{
			return this.Plan.GetHashCode();
		}

		public static bool operator ==(ColonizerProject lhs, ColonizerProject rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(ColonizerProject lhs, ColonizerProject rhs)
		{
			return !(lhs == rhs);
		}
		#endregion

		public const string Tag = "Colonizer";
	}
}
