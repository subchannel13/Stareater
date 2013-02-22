using Ikon;
using Ikon.Utilities;
using System.Collections.Generic;

namespace Stareater.Localization.Reading
{
	class ContextFactory : IValueFactory
	{
		public const char ClosingChar = '-';

		public IkonBaseValue Parse(Ikon.Parser parser)
		{
			string contextName = Ikon.Ston.Parser.ReadIdentifier(parser.Reader);
			Dictionary<string, IText> entries = new Dictionary<string, IText>();

			while (parser.Reader.PeekNextNonwhite() != ClosingChar)
				entries.Add(
					Ikon.Ston.Parser.ReadIdentifier(parser.Reader).ToLower(),
					parser.ParseNext().To<IText>());

			parser.Reader.Read();

			return new Context(contextName, entries);
		}

		public char Sign
		{
			get { return ':'; }
		}
	}
}
