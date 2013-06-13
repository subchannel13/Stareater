using System.Collections.Generic;
using Ikadn;
using Ikadn.Ikon;

namespace Stareater.Localization.Reading
{
	class ContextFactory : IIkadnObjectFactory
	{
		public const char ClosingChar = '-';

		public IkadnBaseObject Parse(IkadnParser parser)
		{
			string contextName = IkonParser.ReadIdentifier(parser.Reader);
			Dictionary<string, IText> entries = new Dictionary<string, IText>();

			while (parser.Reader.PeekNextNonwhite() != ClosingChar)
				entries.Add(
					IkonParser.ReadIdentifier(parser.Reader).ToLower(),
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
