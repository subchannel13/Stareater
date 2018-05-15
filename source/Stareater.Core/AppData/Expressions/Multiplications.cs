using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.AppData.Expressions
{
	class Product : IExpressionNode
	{
		IExpressionNode[] sequence;
		IExpressionNode[] inverseSequence;

		public Product(IExpressionNode[] sequence, IExpressionNode[] inverseSequence)
		{
			this.sequence = sequence;
			this.inverseSequence = inverseSequence;
		}

		public IExpressionNode Simplified()
		{
			int constCount = sequence.Count(x => x.IsConstant) + inverseSequence.Count(x => x.IsConstant);

			if (sequence.Length + inverseSequence.Length == 1)
				return sequence.First();
			else if (constCount == sequence.Length + inverseSequence.Length)
				return new Constant(this.Evaluate(null));
			else if (constCount > 1) {
				List<IExpressionNode> newSequence = new List<IExpressionNode>();
				List<IExpressionNode> newInverseSequence = new List<IExpressionNode>();

				return new Product(
					sequence.Where(x => !x.IsConstant).Concat(new IExpressionNode[] 
					{
						new Constant(
							sequence.Where(x => x.IsConstant).Aggregate(1.0, (subProduct, element) => subProduct * element.Evaluate(null)) /
							inverseSequence.Where(x => x.IsConstant).Aggregate(1.0, (subProduct, element) => subProduct * element.Evaluate(null))
						)
					}).ToArray(),
					inverseSequence.Where(x => !x.IsConstant).ToArray()
					);
			}
			else
				return this;
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
		IExpressionNode[] sequence;

		public IntegerDivision(IExpressionNode[] sequence)
		{
			this.sequence = sequence;
		}

		public IExpressionNode Simplified()
		{
			if (sequence.Count(x => x.IsConstant) == sequence.Length)
				return new Constant(this.Evaluate(null));
			else
				return this;
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
		IExpressionNode[] sequence;

		public IntegerReminder(IExpressionNode[] sequence)
		{
			this.sequence = sequence;
		}

		public IExpressionNode Simplified()
		{
			if (sequence.Count(x => x.IsConstant) == sequence.Length)
				return new Constant(this.Evaluate(null));
			else
				return this;
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
