using IKON;
using IKON.Utils;
using System.Collections.Generic;

namespace Stareater.Localization.Reading
{
	class ContextFactory : IValueFactory
	{
		public const char ClosingChar = '-';

		public Value Parse(IKON.Parser parser)
		{
			Context res = new Context(parser.ReadIdentifier());

			while (parser.ReadNextNonWhite() != ClosingChar) {
				res[parser.ReadIdentifier()] = parser.ParseNext();
			}
			parser.ReadChar();

			return res;
		}

		public char Sign
		{
			get { return ':'; }
		}
	}
}
