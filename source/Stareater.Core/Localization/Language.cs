using Ikadn;
using Ikadn.Utilities;
using Stareater.Localization.Reading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stareater.Localization
{
	public class Language
	{
		private readonly Dictionary<string, Context> contexts = new Dictionary<string, Context>();

		public string Code { get; private set; }

		public Language(string code, IEnumerable<NamedStream> dataSources)
		{
			this.Code = code;
			var factories = new IIkadnObjectFactory[] {
				new ContextFactory(),
				new SingleLineFactory(),
				new TextBlockFactory(),
				new ExpressionTextFactory(),
				new ConditionalTextFactory()
			};

			using (var parser = new IkadnParser(dataSources, factories))
				foreach (var context in parser.ParseAll().Select(x => x.Value.To<Context>()))
					contexts.Add((string)context.Tag, context);
		}

		public Context this[string contextName]
		{
			get
			{
				if (contextName == null)
					throw new ArgumentNullException(nameof(contextName));

				contextName = contextName.ToUpperInvariant();

				if (contexts.ContainsKey(contextName))
					return contexts[contextName.ToUpperInvariant()];
				else if (this != LocalizationManifest.Get.DefaultLanguage)
					return LocalizationManifest.Get.DefaultLanguage[contextName];
				else
					throw new KeyNotFoundException("Context name: " + contextName);
			}
		}
	}
}