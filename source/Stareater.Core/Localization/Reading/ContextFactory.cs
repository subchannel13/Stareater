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
			string contextName = parser.ReadIdentifier();
			Dictionary<string, IText> entries = new Dictionary<string, IText>();

			while (parser.ReadNextNonWhite() != ClosingChar)
				entries.Add(
					parser.ReadIdentifier().ToLower(),
					parser.ParseNext() as IText);
			
			parser.ReadChar();

			return new Context(contextName, entries);
		}

		public char Sign
		{
			get { return ':'; }
		}
	}
}
