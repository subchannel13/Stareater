using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.AppData.Expressions;

namespace ExpressionParser_Tests
{
	class ParserTester
	{
		private readonly ExpressionParser parser;
		private readonly double expectedOutput;
		private readonly IDictionary<string, double> variables;
		private double delta = 0;

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

				if (variables == null)
					return parser.ParsedFormula.Variables.Count == 0;
				else if (!parser.ParsedFormula.Variables.SetEquals(variables.Keys))
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
				
				if (variables == null && parser.ParsedFormula.Variables.Count > 0 ||
				    variables != null && !parser.ParsedFormula.Variables.SetEquals(variables.Keys))
					return "Unexpected variables";
				
				return "Expected: " + expectedOutput + Environment.NewLine + "Evaluated: " + parser.ParsedFormula.Evaluate(variables);
			}
		}
	}
}
