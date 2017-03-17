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
			var entries = new Dictionary<string, IText>();

			while (parser.Reader.PeekNextNonwhite() != ClosingChar)
			{
				var id = IkonParser.ReadIdentifier(parser.Reader).ToLower();
				
				if (!entries.ContainsKey(id))
					entries.Add(id, parser.ParseNext().To<IText>());
				else
					System.Diagnostics.Trace.WriteLine("Duplicate localization entry, id: " + id + " in context: " + contextName);
			}

			parser.Reader.Read();

			return new Context(contextName, entries);
		}

		public char Sign
		{
			get { return ':'; }
		}
	}
}
