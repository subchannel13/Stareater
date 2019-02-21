using System;
using System.Collections.Generic;
using System.Linq;

namespace Stareater.AppData.Expressions
{
	class MinFunction : IExpressionNode
	{
		private readonly IExpressionNode[] sequence;

		public MinFunction(IExpressionNode[] sequence)
		{
			this.sequence = sequence;
		}

		public IExpressionNode Simplified()
		{
			if (sequence.Length == 1)
				return sequence.First();

			var constants = sequence.Where(x => x.IsConstant).ToArray();
			var varing = sequence.Where(x => !x.IsConstant).ToList();

			if (varing.Count == 0)
				return new Constant(constants.Min(x => x.Evaluate(null)));
			if (constants.Length > 1) {
				varing.Add(new Constant(constants.Min(x => x.Evaluate(null))));
				return new MinFunction(varing.ToArray());
			}
				
			return this;
		}

		public bool IsConstant
		{
			get { return sequence.All(element => element.IsConstant); }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return sequence.Min(x => x.Evaluate(variables));
		}
		
		public IEnumerable<string> Variables 
		{ 
			get
			{
				foreach(var node in sequence)
					foreach(var variable in node.Variables)
						yield return variable;
			}
		}
	}

	class MaxFunction : IExpressionNode
	{
		private readonly IExpressionNode[] sequence;

		public MaxFunction(IExpressionNode[] sequence)
		{
			this.sequence = sequence;
		}

		public IExpressionNode Simplified()
		{
			if (sequence.Length == 1)
				return sequence.First();

			var constants = sequence.Where(x => x.IsConstant).ToArray();
			var varing = sequence.Where(x => !x.IsConstant).ToList();

			if (varing.Count == 0)
				return new Constant(constants.Max(x => x.Evaluate(null)));
			if (constants.Length > 1) {
				varing.Add(new Constant(constants.Max(x => x.Evaluate(null))));
				return new MinFunction(varing.ToArray());
			}
				
			return this;
		}

		public bool IsConstant
		{
			get { return sequence.All(element => element.IsConstant); }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return sequence.Max(x => x.Evaluate(variables));
		}
		
		public IEnumerable<string> Variables 
		{ 
			get
			{
				foreach(var node in sequence)
					foreach(var variable in node.Variables)
						yield return variable;
			}
		}
	}

	class FloorFunction : IExpressionNode
	{
		private readonly IExpressionNode argument;

		public FloorFunction(IExpressionNode argument)
		{
			this.argument = argument;
		}

		public IExpressionNode Simplified()
		{
			if (argument.IsConstant)
				return new Constant(this.Evaluate(null));

			return this;
		}

		public bool IsConstant
		{
			get { return argument.IsConstant; }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return Math.Floor(argument.Evaluate(variables));
		}
		
		public IEnumerable<string> Variables
		{ 
			get
			{
				foreach(var variable in argument.Variables)
					yield return variable;
			}
		}
	}

	class CeilFunction : IExpressionNode
	{
		private readonly IExpressionNode argument;

		public CeilFunction(IExpressionNode argument)
		{
			this.argument = argument;
		}

		public IExpressionNode Simplified()
		{
			if (argument.IsConstant)
				return new Constant(this.Evaluate(null));

			return this;
		}

		public bool IsConstant
		{
			get { return argument.IsConstant; }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return Math.Ceiling(argument.Evaluate(variables));
		}
		
		public IEnumerable<string> Variables
		{ 
			get
			{
				foreach(var variable in argument.Variables)
					yield return variable;
			}
		}
	}

	class TruncFunction : IExpressionNode
	{
		private readonly IExpressionNode argument;

		public TruncFunction(IExpressionNode argument)
		{
			this.argument = argument;
		}

		public IExpressionNode Simplified()
		{
			if (argument.IsConstant)
				return new Constant(this.Evaluate(null));

			return this;
		}

		public bool IsConstant
		{
			get { return argument.IsConstant; }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return Math.Truncate(argument.Evaluate(variables));
		}
		
		public IEnumerable<string> Variables
		{ 
			get
			{
				foreach(var variable in argument.Variables)
					yield return variable;
			}
		}
	}

	class AbsFunction : IExpressionNode
	{
		private readonly IExpressionNode argument;

		public AbsFunction(IExpressionNode argument)
		{
			this.argument = argument;
		}

		public IExpressionNode Simplified()
		{
			if (argument.IsConstant)
				return new Constant(this.Evaluate(null));

			return this;
		}

		public bool IsConstant
		{
			get { return argument.IsConstant; }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return Math.Abs(argument.Evaluate(variables));
		}
		
		public IEnumerable<string> Variables
		{ 
			get
			{
				foreach(var variable in argument.Variables)
					yield return variable;
			}
		}
	}

	class RoundFunction : IExpressionNode
	{
		private readonly IExpressionNode argument;

		public RoundFunction(IExpressionNode argument)
		{
			this.argument = argument;
		}

		public IExpressionNode Simplified()
		{
			if (argument.IsConstant)
				return new Constant(this.Evaluate(null));

			return this;
		}

		public bool IsConstant
		{
			get { return argument.IsConstant; }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return Math.Round(argument.Evaluate(variables), MidpointRounding.AwayFromZero);
		}
		
		public IEnumerable<string> Variables
		{ 
			get
			{
				foreach(var variable in argument.Variables)
					yield return variable;
			}
		}
	}

	class SgnFunction : IExpressionNode
	{
		private readonly IExpressionNode argument;

		public SgnFunction(IExpressionNode argument)
		{
			this.argument = argument;
		}

		public IExpressionNode Simplified()
		{
			if (argument.IsConstant)
				return new Constant(this.Evaluate(null));

			return this;
		}

		public bool IsConstant
		{
			get { return argument.IsConstant; }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return Math.Sign(argument.Evaluate(variables));
		}
		
		public IEnumerable<string> Variables
		{ 
			get
			{
				foreach(var variable in argument.Variables)
					yield return variable;
			}
		}
	}

	class LimitFunction : IExpressionNode
	{
		private readonly IExpressionNode argument;
		private readonly IExpressionNode minNode;
		private readonly IExpressionNode maxNode;

		public LimitFunction(IExpressionNode argument, IExpressionNode min, IExpressionNode max)
		{
			this.argument = argument;
			this.minNode = min;
			this.maxNode = max;
		}

		public IExpressionNode Simplified()
		{
			if (this.IsConstant)
				return new Constant(this.Evaluate(null));
			
			return this;
		}

		public bool IsConstant
		{
			get
			{
				return argument.IsConstant &&
					minNode.IsConstant &&
					maxNode.IsConstant;
			}
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			double x = argument.Evaluate(variables);
			double min = minNode.Evaluate(variables);
			double max = maxNode.Evaluate(variables);

			if (x < min)
				return min;

			return (x > max) ? max : x;
		}
		
		public IEnumerable<string> Variables
		{ 
			get
			{
				foreach(var variable in argument.Variables)
					yield return variable;
				
				foreach(var variable in minNode.Variables)
					yield return variable;
				
				foreach(var variable in maxNode.Variables)
					yield return variable;
			}
		}
	}

	class IfThenElseFunction : IExpressionNode
	{
		private readonly IExpressionNode condition;
		private readonly IExpressionNode trueNode;
		private readonly IExpressionNode falseNode;

		public IfThenElseFunction(IExpressionNode condition, IExpressionNode trueNode, IExpressionNode falseNode)
		{
			this.condition = condition;
			this.trueNode = trueNode;
			this.falseNode = falseNode;
		}

		public IExpressionNode Simplified()
		{
			if (this.IsConstant)
				return new Constant(this.Evaluate(null));

			if (condition.IsConstant)
				return (condition.Evaluate(null) >= 0) ? trueNode : falseNode;
			
			return this;
		}

		public bool IsConstant
		{
			get
			{
				if (condition.IsConstant)
					return ((condition.Evaluate(null) >= 0) ? trueNode : falseNode).IsConstant;

				return false;
			}
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return ((condition.Evaluate(variables) >= 0) ? trueNode : falseNode).Evaluate(variables);
		}
		
		public IEnumerable<string> Variables
		{ 
			get
			{
				foreach(var variable in condition.Variables)
					yield return variable;
				
				foreach(var variable in trueNode.Variables)
					yield return variable;
				
				foreach(var variable in falseNode.Variables)
					yield return variable;
			}
		}
	}

	class RatioFunction : IExpressionNode
	{
		private readonly IExpressionNode argument;
		private readonly IExpressionNode baseValue;

		public RatioFunction(IExpressionNode argument, IExpressionNode baseValue)
		{
			this.argument = argument;
			this.baseValue = baseValue;
		}

		public IExpressionNode Simplified()
		{
			if (this.IsConstant)
				return new Constant(this.Evaluate(null));

			if (this.baseValue.IsConstant && this.baseValue.Evaluate(null) == 0)
				return this.argument;

			if (this.argument.IsConstant && this.argument.Evaluate(null) == 0)
				return this.baseValue;

			return this;
		}

		public bool IsConstant
		{
			get
			{
				return this.baseValue.IsConstant && this.argument.IsConstant;
			}
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			var b = this.baseValue.Evaluate(variables);
			var x = this.argument.Evaluate(variables);
			return x / (x + b);
		}

		public IEnumerable<string> Variables
		{
			get
			{
				foreach (var variable in this.argument.Variables)
					yield return variable;

				foreach (var variable in this.baseValue.Variables)
					yield return variable;
			}
		}
	}
}
