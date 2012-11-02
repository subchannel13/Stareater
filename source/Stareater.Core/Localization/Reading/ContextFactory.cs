using Ikon;
using Ikon.Utilities;
using System.Collections.Generic;

namespace Stareater.Localization.Reading
{
	class ContextFactory : IValueFactory
	{
		public const char ClosingChar = '-';

		public Value Parse(Ikon.Parser parser)
		{
			string contextName = parser.Reader.ReadIdentifier();
			Dictionary<string, IText> entries = new Dictionary<string, IText>();

			while (parser.Reader.PeekNextNonwhite() != ClosingChar)
				entries.Add(
					parser.Reader.ReadIdentifier().ToLower(),
					parser.ParseNext() as IText);

			parser.Reader.Read();

			return new Context(contextName, entries);
		}

		public char Sign
		{
			get { return ':'; }
		}
	}
}
