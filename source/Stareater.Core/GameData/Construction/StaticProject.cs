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
			this.Accept(visitor);
		}

		#region Equals
		public override bool Equals(object obj)
		{
			var other = obj as StaticProject;

			if (other == null)
				return false;

			return this.Type.IdCode == other.Type.IdCode;
		}

		public override int GetHashCode()
		{
			return this.Type.IdCode.GetHashCode();
		}

		public static bool operator ==(StaticProject lhs, StaticProject rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(StaticProject lhs, StaticProject rhs)
		{
			return !(lhs == rhs);
		}
		#endregion

		public const string Tag = "Static";
	}
}
