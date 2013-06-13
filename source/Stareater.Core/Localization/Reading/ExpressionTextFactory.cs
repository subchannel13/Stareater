using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Utils.NumberFormatters;
using Ikadn;

namespace Stareater.Localization.Reading
{
	class ExpressionTextFactory : IIkadnObjectFactory
	{
		const char EndingChar = ';';
		const string AutomaticThousands = "_";

		public IkadnBaseObject Parse(IkadnParser parser)
		{
			parser.Reader.SkipWhiteSpaces();
			char formatterType = parser.Reader.Read();
			string formatterParameter = parser.Reader.ReadWhile(c => !char.IsWhiteSpace(c));

			Func<double, string> formatter;
			switch (formatterType) {
				case 'd':
				case 'D':
					formatter = new DecimalsFormatter(0,
						formatterParameter.Length > 0 ? int.Parse(formatterParameter) : 0
						).Format;
					break;
				case 't':
				case 'T':
					formatter = (
						(formatterParameter == AutomaticThousands) ? 
						new ThousandsFormatter() :
						new ThousandsFormatter(formatterParameter)
						).Format;
					break;
				default:
					throw new FormatException("Unexpected expression formatter: " + formatterType);
			}

			parser.Reader.SkipWhiteSpaces();
			string expressionText = parser.Reader.ReadUntil(EndingChar);
			parser.Reader.Read();
			
			if (expressionText.Length == 0)
				throw new FormatException("Expression at " + parser.Reader + " is empty (zero length)");

			return new ExpressionText(expressionText, formatter);
		}

		public char Sign
		{
			get { return '#'; }
		}
	}
}
