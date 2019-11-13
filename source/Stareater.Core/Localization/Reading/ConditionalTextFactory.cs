using System;
using Ikadn;
using Stareater.AppData.Expressions;

namespace Stareater.Localization.Reading
{
	class ConditionalTextFactory : IIkadnObjectFactory
	{
		const char EndingChar = ';';

		public IkadnBaseObject Parse(IkadnReader reader)
		{
			reader.SkipWhiteSpaces();
			string expressionText = reader.ReadUntil(EndingChar);
			reader.Read();

			if (expressionText.Length == 0)
				throw new FormatException("Expression at " + reader.PositionDescription + " is empty (zero length)");
			
			ExpressionParser expressionParser = new ExpressionParser(expressionText);
			expressionParser.Parse();
			
			if (expressionParser.errors.count > 0)
				throw new FormatException("Invalid expression at " + reader.PositionDescription + ". Errors: " + expressionParser.errors.errorMessages);
			
			if (!reader.HasNextObject())
				throw new FormatException("IKON value expected at " + reader.PositionDescription);
			if (expressionParser.ParsedFormula.Variables.Count == 0)
				throw new FormatException("Condition expression at " + reader.PositionDescription + " is constant. If intentional, use format witout condition.");
			
			//TODO(later) substitute subformulas
			return new ConditionalText(expressionParser.ParsedFormula, reader.ReadObject().To<IText>());
		}

		public char Sign
		{
			get { return '?'; }
		}
	}
}
