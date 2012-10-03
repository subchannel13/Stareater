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
					var conext = parser.ParseNext() as Context;
					contexts.Add(conext.TypeName, conext);
				}
				stream.Close();
			}
		}

		public Context this[string contextName]
		{
			get { return contexts[contextName.ToLower()]; }
		}
	}
}
