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

		public ParserTester(string input, Var variables, double expectedOutput)
		{
			this.parser = new ExpressionParser(input);
			this.parser.Parse();

			this.expectedOutput = expectedOutput;
			this.variables = (variables ?? new Var()).Get;
		}

		public ParserTester(string input, Var variables, double expectedOutput, double delta)
			: this(input, variables, expectedOutput)
		{
			this.delta = delta;
		}

		public bool IsOK
		{
			get
			{
				return this.parser.errors.count == 0 && this.isCorrect(this.parser.ParsedFormula);
			}
		}

		public string Message
		{
			get
			{
				if (this.parser.errors.count != 0)
					return this.parser.errors.errorMessages.ToString();
				
				return "Expected: " + this.expectedOutput + Environment.NewLine + "Evaluated: " + this.parser.ParsedFormula.Evaluate(this.variables);
			}
		}

		private bool isCorrect(Formula formula)
		{
			double evaluated = formula.Evaluate(variables);

			return Math.Abs(evaluated - this.expectedOutput) <= this.delta ||
					(double.IsNaN(evaluated) && double.IsNaN(this.expectedOutput)) ||
					(double.IsNegativeInfinity(evaluated) && double.IsNegativeInfinity(this.expectedOutput)) ||
					(double.IsPositiveInfinity(evaluated) && double.IsPositiveInfinity(this.expectedOutput));
		}
	}
}
