using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Stareater.AppData.Expressions
{
	public partial class ExpressionParser
	{
		private IExpressionNode makeComparison(IExpressionNode leftSide, string operatorSymbol, IExpressionNode rightSide, double tolerance)
		{
			IExpressionNode result = new Constant(double.NaN);
			
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
			}
			
			return result.Simplified();
		}

		private IExpressionNode makeBooleanAritmenthics(Queue<IExpressionNode> operands, Queue<string> operators)
		{
			IExpressionNode result = operands.Dequeue();

			List<IExpressionNode> sequence = new List<IExpressionNode>();
			string lastOperator = null;
			sequence.Add(result);

			while (operators.Count > 0 || lastOperator != null) {
				if (operators.Count > 0 && (lastOperator == null || CompatableOperators[operators.Peek()].Contains(lastOperator))) {
					lastOperator = operators.Dequeue();
					sequence.Add(operands.Dequeue());
				}
				else {
					switch (lastOperator) {
						case "&":
						case "∧":
						case "⋀":
							result = new ConjunctionSequence(sequence.ToArray());
							break;
						case "|":
						case "∨":
						case "⋁":
							result = new DisjunctionSequence(sequence.ToArray());
							break;
						case "@":
						case "⊕":
							result = new XorSequence(sequence.ToArray());
							break;
					}
					sequence.Clear();
					lastOperator = null;
				}

			}

			return result.Simplified();
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

		private IExpressionNode makeMultiplications(Queue<IExpressionNode> operands, Queue<string> operators)
		{
			IExpressionNode result = operands.Dequeue();

			List<IExpressionNode> sequence = new List<IExpressionNode>();
			List<IExpressionNode> inverseSequence = new List<IExpressionNode>();
			string lastOperator = null;
			sequence.Add(result);

			while (operators.Count > 0 || lastOperator != null) {
				if (operators.Count > 0 && (lastOperator == null || CompatableOperators[operators.Peek()].Contains(lastOperator))) {
					lastOperator = operators.Dequeue();
					if (lastOperator == "/")
						inverseSequence.Add(operands.Dequeue());
					else
						sequence.Add(operands.Dequeue());
				}
				else {
					switch (lastOperator) {
						case "*":
						case "/":
							result = new Product(sequence.ToArray(), inverseSequence.ToArray());
							break;
						case "\\":
							result = new IntegerDivision(sequence.ToArray());
							break;
						case "%":
							result = new IntegerReminder(sequence.ToArray());
							break;
					}
					sequence.Clear();
					inverseSequence.Clear();
					lastOperator = null;
				}

			}

			return result.Simplified();
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

		private IExpressionNode makeFunction(string identifierName, IList<IExpressionNode> segmentPoints, int listStart)
		{
			switch (identifierName.ToLower()) {
				case "min":
					if (segmentPoints.Count < 2) {
						SemErr("Function \"" + identifierName + "\" at " + listStart + "th character has insufficient parameters.");
						return new Constant(double.NaN);
					}
					return new MinFunction(segmentPoints.ToArray());

				case "max":
					if (segmentPoints.Count < 2) {
						SemErr("Function \"" + identifierName + "\" at " + listStart + "th character has insufficient parameters.");
						return new Constant(double.NaN);
					}
					return new MaxFunction(segmentPoints.ToArray());

				case "floor":
					if (segmentPoints.Count > 1) {
						SemErr("Function \"" + identifierName + "\" at " + listStart + "th character has too many parameters.");
						return new Constant(double.NaN);
					}
					return new FloorFunction(segmentPoints[0]);

				case "ceil":
					if (segmentPoints.Count > 1) {
						SemErr("Function \"" + identifierName + "\" at " + listStart + "th character has too many parameters.");
						return new Constant(double.NaN);
					}
					return new CeilFunction(segmentPoints[0]);

				case "round":
					if (segmentPoints.Count > 1) {
						SemErr("Function \"" + identifierName + "\" at " + listStart + "th character has too many parameters.");
						return new Constant(double.NaN);
					}
					return new RoundFunction(segmentPoints[0]);

				case "trunc":
					if (segmentPoints.Count > 1) {
						SemErr("Function \"" + identifierName + "\" at " + listStart + "th character has too many parameters.");
						return new Constant(double.NaN);
					}
					return new TruncFunction(segmentPoints[0]);

				case "abs":
					if (segmentPoints.Count > 1) {
						SemErr("Function \"" + identifierName + "\" at " + listStart + "th character has too many parameters.");
						return new Constant(double.NaN);
					}
					return new AbsFunction(segmentPoints[0]);

				case "sgn":
					if (segmentPoints.Count > 1) {
						SemErr("Function \"" + identifierName + "\" at " + listStart + "th character has too many parameters.");
						return new Constant(double.NaN);
					}
					return new SgnFunction(segmentPoints[0]);

				case "limit":
					if (segmentPoints.Count != 3) {
						if (segmentPoints.Count < 3)
							SemErr("Function \"" + identifierName + "\" at " + listStart + "th character has insufficient parameters.");
						else
							SemErr("Function \"" + identifierName + "\" at " + listStart + "th character has too many parameters.");
						return new Constant(double.NaN);
					}
					return new LimitFunction(segmentPoints[0], segmentPoints[1], segmentPoints[2]);

				case "if":
					if (segmentPoints.Count != 3) {
						if (segmentPoints.Count < 3)
							SemErr("Function \"" + identifierName + "\" at " + listStart + "th character has insufficient parameters.");
						else
							SemErr("Function \"" + identifierName + "\" at " + listStart + "th character has too many parameters.");
						return new Constant(double.NaN);
					}
					return new IfThenElseFunction(segmentPoints[0], segmentPoints[1], segmentPoints[2]);

				default:
					if (segmentPoints.Count != 0) {
						SemErr("Invalid function name \"" + identifierName + "\" at " + listStart + "th character.");
						return new Constant(double.NaN);
					}
					return new Variable(identifierName);
			}
		}

		private double toDouble(string text)
		{
			return double.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat);
		}

		private static readonly Dictionary<string, ICollection<string>> CompatableOperators = new Dictionary<string, ICollection<string>>()
		{
			{"&", new string[]{"&", "∧", "⋀"}},
			{"∧", new string[]{"&", "∧", "⋀"}},
			{"⋀", new string[]{"&", "∧", "⋀"}},
			{"|", new string[]{"|", "∨", "⋁"}},
			{"∨", new string[]{"|", "∨", "⋁"}},
			{"⋁", new string[]{"|", "∨", "⋁"}},
			{"@", new string[]{"@", "⊕"}},
			{"⊕", new string[]{"@", "⊕"}},

			{"*", new string[]{"*", "/"}},
			{"/", new string[]{"*", "/"}},
			{"%", new string[]{"%"}},
			{"\\", new string[]{"\\"}}
		};
	}
}
