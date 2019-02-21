using System;
using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.Utils.Collections;

namespace ExpressionParser_Tests
{
	class ParserTester
	{
		private readonly ExpressionParser parser;
		private readonly double expectedOutput;
		private readonly IDictionary<string, double> variables;
		private readonly double delta = 0;

		public ParserTester(string input, Dictionary<string, string> subformulas, Var variables, double expectedOutput)
		{
			subformulas = subformulas ?? new Dictionary<string, string>();
			var parsedSubformulas = new Dictionary<string, Formula>();
			foreach (var subformula in subformulas)
			{
				this.parser = new ExpressionParser(subformula.Value, new Dictionary<string, Formula>());
				parser.Parse();

				if (parser.errors.count == 0)
					parsedSubformulas[subformula.Key] = parser.ParsedFormula;
				else
					break;
			}

			if (parser == null || parser.errors.count == 0)
			{
				this.parser = new ExpressionParser(input, parsedSubformulas);
				parser.Parse();
			}

			this.expectedOutput = expectedOutput;
			this.variables = (variables ?? new Var()).Get;
		}

		public ParserTester(string input, Dictionary<string, string> subformulas, Var variables, double expectedOutput, double delta)
			: this(input, subformulas, variables, expectedOutput)
		{
			this.delta = delta;
		}

		public bool IsOK
		{
			get
			{
				if (parser.errors.count > 0 || 
					!parser.ParsedFormula.Variables.SetEquals(variables.Keys))
					return false;
					
				double evaluated = parser.ParsedFormula.Evaluate(variables);
				
				return Math.Abs(evaluated - expectedOutput) <= delta ||
					(double.IsNaN(evaluated) && double.IsNaN(expectedOutput)) ||
					(double.IsNegativeInfinity(evaluated) && double.IsNegativeInfinity(expectedOutput)) ||
					(double.IsPositiveInfinity(evaluated) && double.IsPositiveInfinity(expectedOutput));				
			}
		}

		public string Message
		{
			get
			{
				if (parser.errors.count != 0)
					return parser.errors.errorMessages.ToString();
				
				if (!parser.ParsedFormula.Variables.SetEquals(variables.Keys))
					return "Unexpected variables";
				
				return "Expected: " + expectedOutput + Environment.NewLine + "Evaluated: " + parser.ParsedFormula.Evaluate(variables);
			}
		}
	}
}
