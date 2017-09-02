using Stareater.AppData.Expressions;
using Stareater.Utils.StateEngine;
using System.Collections.Generic;

namespace Stareater.GameData.Construction
{
	[StateType(saveTag: Tag)]
	class StaticProject : IConstructionProject
	{
		[StateProperty]
		public ConstructableType Type { get; private set; }

		public StaticProject(ConstructableType type)
		{
			this.Type = type;
		}

		private StaticProject()
		{ }

		public string StockpileGroup
		{
			get
			{
				return this.Type.StockpileGroup;
			}
		}

		public Formula Condition
		{
			get
			{
				return this.Type.Condition;
			}
		}

		public Formula Cost
		{
			get
			{
				return this.Type.Cost;
			}
		}

		public Formula TurnLimit
		{
			get
			{
				return this.Type.TurnLimit;
			}
		}

		public bool IsVirtual
		{
			get { return false; }
		}

		public IEnumerable<IConstructionEffect> Effects
		{
			get { return this.Type.Effects; }
		}

		public void Accept(IConstructionProjectVisitor visitor)
		{
			visitor.Visit(this);
		}

		public bool Equals(IConstructionProject project)
		{
			var other = project as StaticProject;

			if (other == null)
				return false;

			return this.Type.IdCode == other.Type.IdCode;
		}

		public const string Tag = "Static";
	}
}
