using System.Collections.Generic;
using System.IO;
using Stareater.Localization.Reading;

namespace Stareater.Localization
{
	public class Language
	{
		Dictionary<string, Context> contexts = new Dictionary<string, Context>();

		public string Code { get; private set; }

		public Language(string code, string folderPath)
		{
			this.Code = code;

			foreach (FileInfo file in new DirectoryInfo(folderPath).EnumerateFiles()) {
				StreamReader stream = new StreamReader(file.FullName);
				Parser parser = new Parser(stream);

				while (parser.HasNext()) {
					var conext = parser.ParseNext().To<Context>();
					contexts.Add((string)conext.Tag, conext);
				}
				stream.Close();
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