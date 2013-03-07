using System.Collections.Generic;
using Ikadn;

namespace Stareater.Localization.Reading
{
	class ContextFactory : IValueFactory
	{
		public const char ClosingChar = '-';

		public IkadnBaseValue Parse(Ikadn.Parser parser)
		{
			string contextName = Ikadn.Ikon.Parser.ReadIdentifier(parser.Reader);
			Dictionary<string, IText> entries = new Dictionary<string, IText>();

			while (parser.Reader.PeekNextNonwhite() != ClosingChar)
				entries.Add(
					Ikadn.Ikon.Parser.ReadIdentifier(parser.Reader).ToLower(),
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
