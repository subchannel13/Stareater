using System;
using System.Collections.Generic;
using System.Linq;

namespace Stareater.AppData.Expressions
{
	class Product : IExpressionNode
	{
		private IExpressionNode[] sequence;
		private IExpressionNode[] inverseSequence;

		public Product(IExpressionNode[] sequence, IExpressionNode[] inverseSequence)
		{
			this.sequence = sequence;
			this.inverseSequence = inverseSequence;
		}

		public IExpressionNode Simplified()
		{
			if (this.sequence[0] is Product)
			{
				var firstOperator = this.sequence[0] as Product;
				this.sequence = firstOperator.sequence.
					Concat(this.sequence.Skip(1)).
					ToArray();
				this.inverseSequence = firstOperator.inverseSequence.Concat(this.inverseSequence).ToArray();
			}

			int constCount = sequence.Count(x => x.IsConstant) + inverseSequence.Count(x => x.IsConstant);

			if (sequence.Length + inverseSequence.Length == 1)
				return sequence.First();
			else if (constCount == sequence.Length + inverseSequence.Length)
				return new Constant(this.Evaluate(null));
			else if (constCount > 1)
				return new Product(
					sequence.Where(x => !x.IsConstant).Concat(new IExpressionNode[]
					{
						new Constant(
							sequence.Where(x => x.IsConstant).Select(x => x.Evaluate(null)).Aggregate(1.0, (a, b) => a * b) /
							inverseSequence.Where(x => x.IsConstant).Select(x => x.Evaluate(null)).Aggregate(1.0, (a, b) => a * b)
						)
					}).ToArray(),
					inverseSequence.Where(x => !x.IsConstant).ToArray()
				);
			else
				return this;
		}

		public IExpressionNode Substitute(Dictionary<string, Formula> mapping)
		{
			return new Product(
				this.sequence.Select(x => x.Substitute(mapping)).ToArray(),
				this.inverseSequence.Select(x => x.Substitute(mapping)).ToArray()
			).Simplified();
		}

		public bool IsConstant
		{
			get { return sequence.All(x => x.IsConstant) && inverseSequence.All(x => x.IsConstant); }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return sequence.Aggregate(1.0, (subProduct, element) => subProduct * element.Evaluate(variables)) /
				inverseSequence.Aggregate(1.0, (subProduct, element) => subProduct * element.Evaluate(variables));
		}
		
		public IEnumerable<string> Variables 
		{ 
			get
			{
				foreach(var node in sequence)
					foreach(var variable in node.Variables)
						yield return variable;
				
				foreach(var node in inverseSequence)
					foreach(var variable in node.Variables)
						yield return variable;
			}
		}
	}

	class IntegerDivision : IExpressionNode
	{
		private IExpressionNode[] sequence;

		public IntegerDivision(IExpressionNode[] sequence)
		{
			this.sequence = sequence;
		}

		public IExpressionNode Simplified()
		{
			if (this.sequence[0] is IntegerDivision)
				this.sequence = (this.sequence[0] as IntegerDivision).
					sequence.
					Concat(this.sequence.Skip(1)).
					ToArray();

			if (sequence.Count(x => x.IsConstant) == sequence.Length)
				return new Constant(this.Evaluate(null));
			else
				return this;
		}

		public IExpressionNode Substitute(Dictionary<string, Formula> mapping)
		{
			return new IntegerDivision(this.sequence.Select(x => x.Substitute(mapping)).ToArray()).Simplified();
		}

		public bool IsConstant
		{
			get { return sequence.All(x => x.IsConstant); }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return sequence.Skip(1).Aggregate(sequence.First().Evaluate(variables), 
				(subResult, element) => {
					double rightOperand = element.Evaluate(variables);
					if (rightOperand < 0) {
						subResult *= -1;
						rightOperand *= -1;
					}
					return Math.Floor(subResult / rightOperand);
				});
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

	class IntegerReminder : IExpressionNode
	{
		private IExpressionNode[] sequence;

		public IntegerReminder(IExpressionNode[] sequence)
		{
			this.sequence = sequence;
		}

		public IExpressionNode Simplified()
		{
			if (this.sequence[0] is IntegerReminder)
				this.sequence = (this.sequence[0] as IntegerReminder).
					sequence.
					Concat(this.sequence.Skip(1)).
					ToArray();

			if (sequence.Count(x => x.IsConstant) == sequence.Length)
				return new Constant(this.Evaluate(null));
			else
				return this;
		}

		public IExpressionNode Substitute(Dictionary<string, Formula> mapping)
		{
			return new IntegerReminder(this.sequence.Select(x => x.Substitute(mapping)).ToArray()).Simplified();
		}

		public bool IsConstant
		{
			get { return sequence.All(x => x.IsConstant); }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return sequence.Skip(1).Aggregate(sequence.First().Evaluate(variables),
				(subResult, element) =>
				{
					double rightOperand = element.Evaluate(variables);
					if (rightOperand < 0) {
						subResult *= -1;
						rightOperand *= -1;
					}
					return subResult - Math.Floor(subResult / rightOperand) * rightOperand;
				});
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
}
