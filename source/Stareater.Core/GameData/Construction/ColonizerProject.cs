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

		private ColonizerProject()
		{ }

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

		public bool Equals(IConstructionProject project)
		{
			var other = project as ColonizerProject;

			if (other == null)
				return false;

			return this.Colonizer.Equals(other.Colonizer) && this.Plan.Equals(other.Plan);
		}

		public const string Tag = "Colonizer";
	}
}
