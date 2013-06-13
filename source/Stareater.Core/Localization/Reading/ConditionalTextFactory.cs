using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ikadn;

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
			if (!parser.HasNext())
				throw new FormatException("IKON value expected at " + parser.Reader.PositionDescription);

			return new ConditionalText(expressionText, parser.ParseNext().To<IText>());
		}

		public char Sign
		{
			get { return '?'; }
		}
	}
}
