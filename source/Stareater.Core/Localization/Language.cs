using System;
using System.Collections.Generic;
using System.IO;
using Ikadn;
using Stareater.Localization.Reading;
using Stareater.Utils;

namespace Stareater.Localization
{
	public class Language
	{
		private readonly Dictionary<string, Context> contexts = new Dictionary<string, Context>();

		public string Code { get; private set; }

		public Language(string code, IEnumerable<TracableStream> dataSources)
		{
			this.Code = code;

			foreach (var source in dataSources)
			{
				var parser = new IkadnParser(source.Stream);
				parser.RegisterFactory(new ContextFactory());
				parser.RegisterFactory(new SingleLineFactory());
				parser.RegisterFactory(new TextBlockFactory());
				parser.RegisterFactory(new ExpressionTextFactory());
				parser.RegisterFactory(new ConditionalTextFactory());
				
				try
				{
					while (parser.HasNext())
					{
						var conext = parser.ParseNext().To<Context>();
						contexts.Add((string)conext.Tag, conext);
					}
				}
				catch (IOException e)
				{
					throw new IOException(source.SourceInfo, e);
				}
				catch(FormatException e)
				{
					throw new FormatException(source.SourceInfo, e);
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