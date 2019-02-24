using System;
using System.Collections.Generic;
using Ikadn;
using Stareater.AppData.Expressions;

namespace Stareater.GameData.Reading
{
	class ExpressionFactory : IIkadnObjectFactory
	{
		const char EndingChar = ';';

		private readonly Dictionary<string, Formula> subformulas;

		public ExpressionFactory(Dictionary<string, Formula> subformulas)
		{
			this.subformulas = subformulas;
		}

		public IkadnBaseObject Parse(IkadnParser parser)
		{
			parser.Reader.SkipWhiteSpaces();
			string expressionText = parser.Reader.ReadUntil(EndingChar);
			parser.Reader.Read();
			
			if (expressionText.Length == 0)
				throw new FormatException("Expression at " + parser.Reader + " is empty (zero length)");

			//TODO(v0.8) remove subformula parameter
			var expParser = new ExpressionParser(expressionText, new Dictionary<string, Formula>());
			expParser.Parse();
			
			if (expParser.errors.count > 0)
				throw new FormatException("Expression at " + parser.Reader.PositionDescription + " is invalid: " + expParser.errors.errorMessages);
			
			return new Expression(expParser.ParsedFormula.Substitute(this.subformulas));
		}

		public char Sign
		{
			get { return '#'; }
		}
	}
}
