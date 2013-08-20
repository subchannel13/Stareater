using System;
using Ikadn;
using Stareater.AppData.Expressions;

namespace Stareater.GameData.Reading
{
	/// <summary>
	/// Description of ExpressionFactory.
	/// </summary>
	public class ExpressionFactory : IIkadnObjectFactory
	{
		const char EndingChar = ';';

		public IkadnBaseObject Parse(IkadnParser parser)
		{
			parser.Reader.SkipWhiteSpaces();
			string expressionText = parser.Reader.ReadUntil(EndingChar);
			parser.Reader.Read();
			
			if (expressionText.Length == 0)
				throw new FormatException("Expression at " + parser.Reader + " is empty (zero length)");

			ExpressionParser expParser = new ExpressionParser(expressionText);
			expParser.Parse();
			
			if (expParser.errors.count > 0)
				throw new FormatException("Expression at " + parser.Reader.PositionDescription + " is invalid: " + expParser.errors.errorMessages);
			
			return new Expression(expParser.ParsedFormula);
		}

		public char Sign
		{
			get { return '#'; }
		}
	}
}
