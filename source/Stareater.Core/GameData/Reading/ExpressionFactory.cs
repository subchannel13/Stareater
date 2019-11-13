using System;
using System.Collections.Generic;
using Ikadn;
using Ikadn.Ikon.Factories;
using Stareater.AppData.Expressions;

namespace Stareater.GameData.Reading
{
	class ExpressionFactory : AIkonFactory
	{
		const char EndingChar = ';';

		private readonly Dictionary<string, Formula> subformulas = new Dictionary<string, Formula>();

		protected override IkadnBaseObject ParseObject(IkadnReader reader)
		{
			reader.SkipWhiteSpaces();
			string expressionText = reader.ReadUntil(EndingChar);
			reader.Read();
			
			if (expressionText.Length == 0)
				throw new FormatException("Expression at " + reader.PositionDescription + " is empty (zero length)");

			var expParser = new ExpressionParser(expressionText);
			expParser.Parse();
			
			if (expParser.errors.count > 0)
				throw new FormatException("Expression at " + reader.PositionDescription + " is invalid: " + expParser.errors.errorMessages);
			
			return new Expression(expParser.ParsedFormula.Substitute(this.subformulas));
		}

		public override char Sign => '#';
	}
}
