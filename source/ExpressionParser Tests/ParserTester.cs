using System;
using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.Utils.Collections;

namespace ExpressionParser_Tests
{
	class ParserTester
	{
		private readonly ExpressionParser parser;
		private readonly Formula substituedExpr;
		private readonly double expectedOutput;
		private readonly IDictionary<string, double> variables;
		private readonly double delta = 0;

		public ParserTester(string input, Dictionary<string, string> subformulas, Var variables, double expectedOutput)
		{
			this.parser = new ExpressionParser(input);
			this.parser.Parse();

			subformulas = subformulas ?? new Dictionary<string, string>();
			var parsedSubformulas = new Dictionary<string, Formula>();
			foreach (var subformula in subformulas)
			{
				var subparser = new ExpressionParser(subformula.Value);
				subparser.Parse();

				if (parser.errors.count > 0)
					throw new FormatException(subparser.errors.errorMessages.ToString());

				parsedSubformulas[subformula.Key] = subparser.ParsedFormula;
			}
			parsedSubformulas = ExpressionParser.ResloveSubformulaNesting(parsedSubformulas);

			this.substituedExpr = (this.parser.errors.count == 0) ? this.parser.ParsedFormula.Substitute(parsedSubformulas) : null;

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
				if (this.parser.errors.count > 0 || 
					!this.substituedExpr.Variables.SetEquals(this.variables.Keys))
					return false;

				return this.isCorrect(this.substituedExpr) && (
					!this.substituedExpr.Variables.SetEquals(this.parser.ParsedFormula.Variables) || this.isCorrect(this.parser.ParsedFormula)
				);
			}
		}

		public string Message
		{
			get
			{
				if (this.parser.errors.count != 0)
					return this.parser.errors.errorMessages.ToString();
				
				if (!this.substituedExpr.Variables.SetEquals(variables.Keys))
					return "Unexpected variables";
				
				return "Expected: " + this.expectedOutput + Environment.NewLine + "Evaluated: " + this.parser.ParsedFormula.Evaluate(this.variables);
			}
		}

		private bool isCorrect(Formula formula)
		{
			double evaluated = formula.Evaluate(variables);

			return Math.Abs(evaluated - this.expectedOutput) <= delta ||
					(double.IsNaN(evaluated) && double.IsNaN(this.expectedOutput)) ||
					(double.IsNegativeInfinity(evaluated) && double.IsNegativeInfinity(this.expectedOutput)) ||
					(double.IsPositiveInfinity(evaluated) && double.IsPositiveInfinity(this.expectedOutput));
		}
	}
}
