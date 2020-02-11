using System.Collections.Generic;
using Ikadn;
using Ikadn.Ikon;
using System;

namespace Stareater.Localization.Reading
{
	class ContextFactory : IIkadnObjectFactory
	{
		public const char ClosingChar = '-';

		public IkadnBaseObject Parse(IkadnReader reader)
		{
			string contextName = IkonParser.ReadIdentifier(reader);
			var entries = new Dictionary<string, IText>();

			while (reader.PeekNextNonwhite() != ClosingChar)
			{
				var id = IkonParser.ReadIdentifier(reader).ToUpperInvariant();

				if (!entries.ContainsKey(id))
					entries.Add(id, reader.ReadObject().To<IText>());
				else
					AppData.ErrorReporter.Get.Report(new ArgumentException("Duplicate localization entry, id: " + id + " in context: " + contextName));
			}

			reader.Read();

			return new Context(contextName, entries);
		}

		public char Sign
		{
			get { return ':'; }
		}
	}
}
