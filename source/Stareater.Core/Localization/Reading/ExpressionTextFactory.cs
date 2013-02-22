using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ikon;
using Stareater.Utils.NumberFormatters;

namespace Stareater.Localization.Reading
{
	class ExpressionTextFactory : IValueFactory
	{
		const string AutomaticThousands = "_";

		public IkonBaseValue Parse(Ikon.Parser parser)
		{
			parser.Reader.SkipWhiteSpaces();
			char formatterType = parser.Reader.Read();

			StringBuilder formatterParameterSB = new StringBuilder();
			while (!char.IsWhiteSpace(parser.Reader.Peek()))
				formatterParameterSB.Append(parser.Reader.Read());

			Func<double, string> formatter;
			switch (formatterType) {
				case 'd':
				case 'D':
					formatter = new DecimalsFormatter(0, 
						formatterParameterSB.Length > 0 ? int.Parse(formatterParameterSB.ToString()) : 0
						).Format;
					break;
				case 't':
				case 'T':
					formatter = (
						(formatterParameterSB.ToString() == AutomaticThousands) ? 
						new ThousandsFormatter() :
						new ThousandsFormatter(formatterParameterSB.ToString())
						).Format;
					break;
				default:
					throw new FormatException("Unexpected expression formatter: " + formatterType);
			}

			parser.Reader.SkipWhiteSpaces();
			StringBuilder expressionText = new StringBuilder();
			while (parser.Reader.HasNext) {
				char nextChar = parser.Reader.Peek();
				if (nextChar == '\n' || nextChar == '\r' || nextChar == TextBlockFactory.EndingChar)
					break;
				expressionText.Append(parser.Reader.Read());
			}

			return new ExpressionText(expressionText.ToString(), formatter);
		}

		public char Sign
		{
			get { return '#'; }
		}
	}
}
