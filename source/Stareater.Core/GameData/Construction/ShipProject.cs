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
			get { return this.Type.IsVirtual; }
		}

		public IEnumerable<IConstructionEffect> Effects
		{
			get { yield return new ConstructionAddShip(this.Type); }
		}

		public void Accept(IConstructionProjectVisitor visitor)
		{
			visitor.Visit(this);
		}

		public bool Equals(IConstructionProject project)
		{
			var other = project as ShipProject;

			if (other == null)
				return false;

			return this.Type.Equals(other.Type);
		}
		
		public const string Tag = "Ship";
	}
}
