using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Stareater.AppData.Expressions
{
	public partial class ExpressionParser
	{
		private IExpressionNode makeComparison(IExpressionNode leftSide, string operatorSymbol, IExpressionNode rightSide, double tolerance)
		{
			IExpressionNode result;
			
			switch (operatorSymbol) {
				case "=":
					result = new Equality(leftSide, rightSide, tolerance);
					break;
				case "<>":
				case "≠":
					result = new Inequality(leftSide, rightSide, tolerance);
					break;
				case "<":
					result = new LessThen(leftSide, rightSide, tolerance, true);
					break;
				case "<=":
				case "≤":
					result = new LessThen(leftSide, rightSide, tolerance, false);
					break;
				case ">":
					result = new GreaterThen(leftSide, rightSide, tolerance, true);
					break;
				case ">=":
				case "≥":
					result = new GreaterThen(leftSide, rightSide, tolerance, false);
					break;
				default:
					SemErr("Unimplemented comparison operator " + operatorSymbol);
					result = new Constant(double.NaN);
					break;
			}
			
			return result.Simplified();
		}

		private IExpressionNode makeBooleanAritmenthics(IExpressionNode leftSide, IExpressionNode rightSide, string currentOperator)
		{
			var operands = new[] { leftSide, rightSide };

			switch (currentOperator)
			{
				case "&":
				case "∧":
				case "⋀":
					return new ConjunctionSequence(operands).Simplified();
				case "|":
				case "∨":
				case "⋁":
					return new DisjunctionSequence(operands).Simplified();
				case "@":
				case "⊕":
					return new XorSequence(operands).Simplified();
				default:
					SemErr("Unimplemented boolean operator " + currentOperator);
					return new Constant(double.NaN);
			}
		}

		private IExpressionNode makeSummation(Queue<IExpressionNode> operands, Queue<string> operators)
		{
			if (operands.Count == 1)
				return operands.Dequeue();

			List<IExpressionNode> sequence = new List<IExpressionNode>();
			List<IExpressionNode> inverseSequence = new List<IExpressionNode>();
			sequence.Add(operands.Dequeue());

			while (operators.Count > 0)
				if (operators.Dequeue() == "+")
					sequence.Add(operands.Dequeue());
				else
					inverseSequence.Add(operands.Dequeue());

			return new Summation(sequence.ToArray(), inverseSequence.ToArray());
		}

		private IExpressionNode makeMultiplications(IExpressionNode leftSide, IExpressionNode rightSide, string currentOperator)
		{
			var sequence = new[] { leftSide, rightSide };
			var inverseSequence = new IExpressionNode[0];

			if (currentOperator == "/")
			{
				sequence = new[] { leftSide };
				inverseSequence = new[] { rightSide };
			}

			switch (currentOperator)
			{
				case "*":
				case "/":
					return new Product(sequence, inverseSequence).Simplified();
				case "\\":
					return new IntegerDivision(sequence).Simplified();
				case "%":
					return new IntegerReminder(sequence).Simplified();
				default:
					SemErr("Unimplemented multiplication operator " + currentOperator);
					return new Constant(double.NaN);
			}
		}

		private IExpressionNode makeList(IExpressionNode indexNode, IList<IExpressionNode> segmentPoints, int listStart)
		{
			if (segmentPoints.Count == 0)
				return indexNode;
			if (segmentPoints.Count < 2) {
				SemErr("Insufficient arguments in the list at " + listStart + "th character.");
				return new Constant(double.NaN);
			}

			return new LinearSegmentsFunction(indexNode, segmentPoints.ToArray()).Simplified();
		}

		private IExpressionNode makeUnaryOperation(IExpressionNode child, string currentOperator)
		{
			if (currentOperator == null)
				return child;

			switch (currentOperator)
			{
				case "-":
					return new Negation(child).Simplified();
				case "'":
					return new ToBoolean(child).Simplified();
				case "-'":
					return new NegateToBoolean(child).Simplified();
				default:
					SemErr("Unimplemented unary operator " + currentOperator);
					return new Constant(double.NaN);
			}
		}

		private IExpressionNode makeFunction(string identifierName, IList<IExpressionNode> segmentPoints, int listStart)
		{
			switch (identifierName.ToUpperInvariant()) {
				case "MIN":
					return paramCountAtLeast(segmentPoints.Count, 2, identifierName, listStart) ? 
						new MinFunction(segmentPoints.ToArray()).Simplified() :
						new Constant(double.NaN);

				case "MAX":
					return paramCountAtLeast(segmentPoints.Count, 2, identifierName, listStart) ?
						new MaxFunction(segmentPoints.ToArray()).Simplified() :
						new Constant(double.NaN);

				case "FLOOR":
					return paramCountExact(segmentPoints.Count, 1, identifierName, listStart) ?
						new FloorFunction(segmentPoints[0]).Simplified() :
						new Constant(double.NaN);

				case "CEIL":
					return paramCountExact(segmentPoints.Count, 1, identifierName, listStart) ?
						new CeilFunction(segmentPoints[0]).Simplified() :
						new Constant(double.NaN);

				case "ROUND":
					return paramCountExact(segmentPoints.Count, 1, identifierName, listStart) ?
						new RoundFunction(segmentPoints[0]).Simplified() :
						new Constant(double.NaN);

				case "TRUNC":
					return paramCountExact(segmentPoints.Count, 1, identifierName, listStart) ?
						new TruncFunction(segmentPoints[0]).Simplified() :
						new Constant(double.NaN);

				case "ABS":
					return paramCountExact(segmentPoints.Count, 1, identifierName, listStart) ?
						new AbsFunction(segmentPoints[0]).Simplified() :
						new Constant(double.NaN);

				case "SGN":
					return paramCountExact(segmentPoints.Count, 1, identifierName, listStart) ?
						new SgnFunction(segmentPoints[0]).Simplified() :
						new Constant(double.NaN);

				case "LIMIT":
					return paramCountExact(segmentPoints.Count, 3, identifierName, listStart) ?
						new LimitFunction(segmentPoints[0], segmentPoints[1], segmentPoints[2]).Simplified() :
						new Constant(double.NaN);

				case "IF":
					return paramCountExact(segmentPoints.Count, 3, identifierName, listStart) ?
						new IfThenElseFunction(segmentPoints[0], segmentPoints[1], segmentPoints[2]).Simplified() :
						new Constant(double.NaN);
				case "CASE":
					return paramCountAtLeast(segmentPoints.Count, 1, identifierName, listStart) ?
						new CaseFunction(segmentPoints.ToArray()).Simplified() :
						new Constant(double.NaN);
				case "RATIO":
					return paramCountExact(segmentPoints.Count, 2, identifierName, listStart) ?
						new RatioFunction(segmentPoints[0], segmentPoints[1]).Simplified() :
						new Constant(double.NaN);
				default:
					if (segmentPoints.Count != 0) {
						SemErr("Invalid function name \"" + identifierName + "\" at " + listStart + "th character.");
						return new Constant(double.NaN);
					}
					return new Variable(identifierName);
			}
		}

		private bool paramCountAtLeast(int count, int minimumCount, string name, int listStart)
		{
			if (count < minimumCount)
			{
				SemErr("Function \"" + name + "\" at " + listStart + "th character has insufficient parameters.");
				return false;
			}

			return true;
		}

		private bool paramCountExact(int count, int targetCount, string name, int listStart)
		{
			if (count < targetCount)
			{
				SemErr("Function \"" + name + "\" at " + listStart + "th character has insufficient parameters.");
				return false;
			}
			else if (count > targetCount)
			{
				SemErr("Function \"" + name + "\" at " + listStart + "th character has too many parameters.");
				return false;
			}
			else
				return true;
		}

		private double toDouble(string text)
		{
			return double.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);
		}
	}
}
