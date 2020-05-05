using Stareater.AppData.Expressions;
using Stareater.Ships;
using Stareater.Utils.StateEngine;
using System.Collections.Generic;
using System;

namespace Stareater.GameData.Construction
{
	[StateTypeAttribute(saveTag: Tag)]
	class ShipProject : IConstructionProject
	{
		[StatePropertyAttribute]
		public Design Type { get; set; }

		[StatePropertyAttribute]
		public bool IsVirtual { get; private set; }

		[StatePropertyAttribute]
		private double cost { get; set; }

		public ShipProject(Design design, double cost, bool isVirtual)
		{
			this.Type = design;
			this.cost = cost;
			this.IsVirtual = isVirtual;
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

		public Formula TurnLimit
		{
			get { return new Formula(double.PositiveInfinity); }
		}

		public IEnumerable<IConstructionEffect> Effects
		{
			get { yield return new ConstructionAddShip(this.Type); }
		}

		public Formula Cost => new Formula(this.cost);

		public void Accept(IConstructionProjectVisitor visitor)
		{
			visitor.Visit(this);
		}

		public bool Equals(IConstructionProject project)
		{
			return !(project is ShipProject other) ? false : this.Type.Equals(other.Type);
		}

		public const string Tag = "Ship";
	}
}
