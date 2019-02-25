using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ikadn;
using Stareater.AppData.Expressions;

namespace Stareater.Localization.Reading
{
	class ConditionalTextFactory : IIkadnObjectFactory
	{
		const char EndingChar = ';';

		public IkadnBaseObject Parse(IkadnParser parser)
		{
			parser.Reader.SkipWhiteSpaces();
			string expressionText = parser.Reader.ReadUntil(EndingChar);
			parser.Reader.Read();

			if (expressionText.Length == 0)
				throw new FormatException("Expression at " + parser.Reader + " is empty (zero length)");
			
			ExpressionParser expressionParser = new ExpressionParser(expressionText);
			expressionParser.Parse();
			
			if (expressionParser.errors.count > 0)
				throw new FormatException("Invalid expression at " + parser.Reader + ". Errors: " + expressionParser.errors.errorMessages);
			
			if (!parser.HasNext())
				throw new FormatException("IKON value expected at " + parser.Reader.PositionDescription);
			if (expressionParser.ParsedFormula.Variables.Count == 0)
				throw new FormatException("Condition expression at " + parser.Reader + " is constant. If intentional, use format witout condition.");
			
			//TODO(later) substitute subformulas
			return new ConditionalText(expressionParser.ParsedFormula, parser.ParseNext().To<IText>());
		}

		public char Sign
		{
			get { return '?'; }
		}
	}
}
