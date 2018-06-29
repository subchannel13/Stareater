using System;
using System.Collections.Generic;

namespace Stareater.AppData.Expressions
{
	abstract class Comparison : IExpressionNode
	{
		private readonly IExpressionNode leftSide;
		private readonly IExpressionNode rightSide;
		private readonly double tolerance;

		protected Comparison(IExpressionNode leftSide, IExpressionNode rightSide, double tolerance)
		{
			this.leftSide = leftSide;
			this.rightSide = rightSide;
			this.tolerance = tolerance;
		}

		public IExpressionNode Simplified()
		{
			if (this.IsConstant)
				return new Constant(this.Evaluate(null));
			return this;
		}

		public bool IsConstant
		{
			get { return leftSide.IsConstant && rightSide.IsConstant; }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			double leftValue = leftSide.Evaluate(variables);
			double rightValue = rightSide.Evaluate(variables);

			return compare(leftValue, rightValue, Math.Abs(leftValue - rightValue) <= tolerance) ? 1 : -1;
		}

		public IEnumerable<string> Variables
		{ 
			get
			{
				foreach(var variable in leftSide.Variables)
					yield return variable;
				
				foreach(var variable in rightSide.Variables)
					yield return variable;
			}
		}
		
		protected abstract bool compare(double leftValue, double rightValue, bool withinTolerance);
	}

	class Equality : Comparison
	{
		public Equality(IExpressionNode leftSide, IExpressionNode rightSide, double tolerance) :
			base(leftSide, rightSide, tolerance)
		{ }

		protected override bool compare(double leftValue, double rightValue, bool withinTolerance)
		{
			return withinTolerance;
		}
	}

	class Inequality : Comparison
	{
		public Inequality(IExpressionNode leftSide, IExpressionNode rightSide, double tolerance) :
			base(leftSide, rightSide, tolerance)
		{ }

		protected override bool compare(double leftValue, double rightValue, bool withinTolerance)
		{
			return !withinTolerance;
		}
	}

	class LessThen : Comparison
	{
		private readonly bool allowEqual;

		public LessThen(IExpressionNode leftSide, IExpressionNode rightSide, double tolerance, bool strictlyLess) :
			base(leftSide, rightSide, tolerance)
		{
			this.allowEqual = !strictlyLess;
		}

		protected override bool compare(double leftValue, double rightValue, bool withinTolerance)
		{
			if (allowEqual && withinTolerance)
				return true;
			return leftValue < rightValue;
		}
	}

	class GreaterThen : Comparison
	{
		private readonly bool allowEqual;

		public GreaterThen(IExpressionNode leftSide, IExpressionNode rightSide, double tolerance, bool strictlyGreater) :
			base(leftSide, rightSide, tolerance)
		{
			this.allowEqual = !strictlyGreater;
		}

		protected override bool compare(double leftValue, double rightValue, bool withinTolerance)
		{
			if (allowEqual && withinTolerance)
				return true;
			return leftValue > rightValue;
		}
	}
}
