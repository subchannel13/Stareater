using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.AppData.Expressions
{
	class MinFunction : IExpressionNode
	{
		ICollection<IExpressionNode> sequence;

		public MinFunction(ICollection<IExpressionNode> sequence)
		{
			this.sequence = sequence;
		}

		public IExpressionNode Simplified()
		{
			if (sequence.Count == 0)
				return sequence.First();

			var constants = sequence.Where(x => x.isConstant).ToArray();
			var varing = sequence.Where(x => !x.isConstant).ToList();

			if (varing.Count == 0)
				return new Constant(constants.Min(x => x.Evaluate(null)));
			if (constants.Length > 1) {
				varing.Add(new Constant(constants.Min(x => x.Evaluate(null))));
				return new MinFunction(varing);
			}
				
			return this;
		}

		public bool isConstant
		{
			get { return sequence.All(element => element.isConstant); }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return sequence.Min(x => x.Evaluate(variables));
		}
	}

	class MaxFunction : IExpressionNode
	{
		ICollection<IExpressionNode> sequence;

		public MaxFunction(ICollection<IExpressionNode> sequence)
		{
			this.sequence = sequence;
		}

		public IExpressionNode Simplified()
		{
			if (sequence.Count == 0)
				return sequence.First();

			var constants = sequence.Where(x => x.isConstant).ToArray();
			var varing = sequence.Where(x => !x.isConstant).ToList();

			if (varing.Count == 0)
				return new Constant(constants.Max(x => x.Evaluate(null)));
			if (constants.Length > 1) {
				varing.Add(new Constant(constants.Max(x => x.Evaluate(null))));
				return new MinFunction(varing);
			}
				
			return this;
		}

		public bool isConstant
		{
			get { return sequence.All(element => element.isConstant); }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return sequence.Max(x => x.Evaluate(variables));
		}
	}

	class FloorFunction : IExpressionNode
	{
		IExpressionNode argument;

		public FloorFunction(IExpressionNode argument)
		{
			this.argument = argument;
		}

		public IExpressionNode Simplified()
		{
			if (argument.isConstant)
				return new Constant(this.Evaluate(null));

			return this;
		}

		public bool isConstant
		{
			get { return argument.isConstant; }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return Math.Floor(argument.Evaluate(variables));
		}
	}

	class CeilFunction : IExpressionNode
	{
		IExpressionNode argument;

		public CeilFunction(IExpressionNode argument)
		{
			this.argument = argument;
		}

		public IExpressionNode Simplified()
		{
			if (argument.isConstant)
				return new Constant(this.Evaluate(null));

			return this;
		}

		public bool isConstant
		{
			get { return argument.isConstant; }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return Math.Ceiling(argument.Evaluate(variables));
		}
	}

	class TruncFunction : IExpressionNode
	{
		IExpressionNode argument;

		public TruncFunction(IExpressionNode argument)
		{
			this.argument = argument;
		}

		public IExpressionNode Simplified()
		{
			if (argument.isConstant)
				return new Constant(this.Evaluate(null));

			return this;
		}

		public bool isConstant
		{
			get { return argument.isConstant; }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return Math.Truncate(argument.Evaluate(variables));
		}
	}

	class AbsFunction : IExpressionNode
	{
		IExpressionNode argument;

		public AbsFunction(IExpressionNode argument)
		{
			this.argument = argument;
		}

		public IExpressionNode Simplified()
		{
			if (argument.isConstant)
				return new Constant(this.Evaluate(null));

			return this;
		}

		public bool isConstant
		{
			get { return argument.isConstant; }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return Math.Abs(argument.Evaluate(variables));
		}
	}

	class RoundFunction : IExpressionNode
	{
		IExpressionNode argument;

		public RoundFunction(IExpressionNode argument)
		{
			this.argument = argument;
		}

		public IExpressionNode Simplified()
		{
			if (argument.isConstant)
				return new Constant(this.Evaluate(null));

			return this;
		}

		public bool isConstant
		{
			get { return argument.isConstant; }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return Math.Round(argument.Evaluate(variables), MidpointRounding.AwayFromZero);
		}
	}

	class SgnFunction : IExpressionNode
	{
		IExpressionNode argument;

		public SgnFunction(IExpressionNode argument)
		{
			this.argument = argument;
		}

		public IExpressionNode Simplified()
		{
			if (argument.isConstant)
				return new Constant(this.Evaluate(null));

			return this;
		}

		public bool isConstant
		{
			get { return argument.isConstant; }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return Math.Sign(argument.Evaluate(variables));
		}
	}

	class LimitFunction : IExpressionNode
	{
		IExpressionNode argument;
		IExpressionNode minNode;
		IExpressionNode maxNode;

		public LimitFunction(IExpressionNode argument, IExpressionNode min, IExpressionNode max)
		{
			this.argument = argument;
			this.minNode = min;
			this.maxNode = max;
		}

		public IExpressionNode Simplified()
		{
			if (this.isConstant)
				return new Constant(this.Evaluate(null));
			
			return this;
		}

		public bool isConstant
		{
			get
			{
				return argument.isConstant &&
					minNode.isConstant &&
					maxNode.isConstant;
			}
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			double x = argument.Evaluate(variables);
			double min = minNode.Evaluate(variables);
			double max = maxNode.Evaluate(variables);

			return (x < min) ? min : (x > max) ? max : x;
		}
	}

	class IfThenElseFunction : IExpressionNode
	{
		IExpressionNode condition;
		IExpressionNode trueNode;
		IExpressionNode falseNode;

		public IfThenElseFunction(IExpressionNode condition, IExpressionNode trueNode, IExpressionNode falseNode)
		{
			this.condition = condition;
			this.trueNode = trueNode;
			this.falseNode = falseNode;
		}

		public IExpressionNode Simplified()
		{
			if (this.isConstant)
				return new Constant(this.Evaluate(null));

			if (condition.isConstant)
				return (condition.Evaluate(null) >= 0) ? trueNode : falseNode;
			
			return this;
		}

		public bool isConstant
		{
			get
			{
				if (condition.isConstant)
					return ((condition.Evaluate(null) >= 0) ? trueNode : falseNode).isConstant;

				return false;
			}
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return ((condition.Evaluate(variables) >= 0) ? trueNode : falseNode).Evaluate(variables);
		}
	}
}
