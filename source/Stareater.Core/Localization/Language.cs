using System.Collections.Generic;
using System.IO;
using Stareater.Localization.Reading;

namespace Stareater.Localization
{
	public class Language
	{
		private readonly Dictionary<string, Context> contexts = new Dictionary<string, Context>();

		public string Code { get; private set; }

		public Language(string code, IEnumerable<TextReader> dataSources)
		{
			this.Code = code;

			foreach (var source in dataSources)
			{
				var parser = new Parser(source);

				while (parser.HasNext())
				{
					var conext = parser.ParseNext().To<Context>();
					contexts.Add((string)conext.Tag, conext);
				}
			}
		}

		public Context this[string contextName]
		{
			get
			{
				contextName = contextName.ToLower();

				if (contexts.ContainsKey(contextName))
					return contexts[contextName.ToLower()];
				else if (this != LocalizationManifest.Get.DefaultLanguage)
					return LocalizationManifest.Get.DefaultLanguage[contextName];
				else
					throw new KeyNotFoundException("Context name: " + contextName);
			}
		}
	}
}