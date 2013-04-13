﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.AppData.Expressions
{
	class Product : IExpressionNode
	{
		ICollection<IExpressionNode> sequence;
		ICollection<IExpressionNode> inverseSequence;

		public Product(ICollection<IExpressionNode> sequence, ICollection<IExpressionNode> inverseSequence)
		{
			this.sequence = sequence;
			this.inverseSequence = inverseSequence;
		}

		public IExpressionNode Simplified()
		{
			int constCount = sequence.Count(x => x.isConstant) + inverseSequence.Count(x => x.isConstant);

			if (sequence.Count + inverseSequence.Count == 1)
				return sequence.First();
			else if (constCount == sequence.Count + inverseSequence.Count)
				return new Constant(this.Evaluate(null));
			else if (constCount > 1) {
				List<IExpressionNode> newSequence = new List<IExpressionNode>();
				List<IExpressionNode> newInverseSequence = new List<IExpressionNode>();

				return new Product(
					sequence.Where(x => !x.isConstant).Concat(new IExpressionNode[] 
					{
						new Constant(
							sequence.Where(x => x.isConstant).Aggregate(1.0, (subProduct, element) => subProduct * element.Evaluate(null)) /
							inverseSequence.Where(x => x.isConstant).Aggregate(1.0, (subProduct, element) => subProduct * element.Evaluate(null))
						)
					}).ToArray(),
					inverseSequence.Where(x => !x.isConstant).ToArray()
					);
			}
			else
				return this;
		}

		public bool isConstant
		{
			get { return sequence.All(x => x.isConstant) && inverseSequence.All(x => x.isConstant); }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return sequence.Aggregate(1.0, (subProduct, element) => subProduct * element.Evaluate(variables)) /
				inverseSequence.Aggregate(1.0, (subProduct, element) => subProduct * element.Evaluate(variables));
		}
	}

	class IntegerDivision : IExpressionNode
	{
		ICollection<IExpressionNode> sequence;

		public IntegerDivision(ICollection<IExpressionNode> sequence)
		{
			this.sequence = sequence;
		}

		public IExpressionNode Simplified()
		{
			if (sequence.Count == 1)
				return sequence.First();
			else if (sequence.Count(x => x.isConstant) == sequence.Count)
				return new Constant(this.Evaluate(null));
			else
				return this;
		}

		public bool isConstant
		{
			get { return sequence.All(x => x.isConstant); }
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
	}

	class IntegerReminder : IExpressionNode
	{
		ICollection<IExpressionNode> sequence;

		public IntegerReminder(ICollection<IExpressionNode> sequence)
		{
			this.sequence = sequence;
		}

		public IExpressionNode Simplified()
		{
			if (sequence.Count == 1)
				return sequence.First();
			else if (sequence.Count(x => x.isConstant) == sequence.Count)
				return new Constant(this.Evaluate(null));
			else
				return this;
		}

		public bool isConstant
		{
			get { return sequence.All(x => x.isConstant); }
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
	}
}
