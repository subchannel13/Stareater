using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.AppData.Expressions;

namespace ExpressionParser_Tests
{
	class ParserTester
	{
		ExpressionParser parser;
		double expectedOutput;
		IDictionary<string, double> variables;
		double delta = 0;

		public ParserTester(string input, IDictionary<string, double> variables, double expectedOutput)
		{
			this.parser = new ExpressionParser(input);
			parser.Parse();

			this.expectedOutput = expectedOutput;
			this.variables = variables;
		}

		public ParserTester(string input, IDictionary<string, double> variables, double expectedOutput, double delta)
			: this(input, variables, expectedOutput)
		{
			this.delta = delta;
		}

		public bool IsOK
		{
			get
			{
				if (parser.errors.count > 0)
					return false;

				double evaluated = parser.ParsedFormula.Evaluate(variables);
				
				return Math.Abs(evaluated - expectedOutput) <= delta ||
					(double.IsNaN(evaluated) && double.IsNaN(expectedOutput)) ||
					(double.IsNegativeInfinity(evaluated) && double.IsNegativeInfinity(expectedOutput)) ||
					(double.IsPositiveInfinity(evaluated) && double.IsPositiveInfinity(expectedOutput))
					;				
			}
		}

		public string Message
		{
			get
			{
				if (parser.errors.count != 0)
					return parser.errors.errorMessages.ToString();
				
				return "Expected: " + expectedOutput + Environment.NewLine + "Evaluated: " + parser.ParsedFormula.Evaluate(variables);
			}
		}
	}
}
