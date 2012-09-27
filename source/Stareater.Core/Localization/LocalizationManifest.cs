using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Stareater.Localization
{
	static class LocalizationManifest
	{
		public const string LanguagesFolder = "./languages/";

		public static Language DefaultLanguage { get; private set; }
		public static ReadOnlyCollection<string> LanguageCodes { get; private set; }

		private const string DefaultLangSufix = "(default)";

		static LocalizationManifest()
		{
			string defaultLangCode = null;
			List<string> codes = new List<string>();

			foreach (var folder in new DirectoryInfo(LanguagesFolder).EnumerateDirectories()) {
				string code = folder.Name;

				if (folder.Name.EndsWith(DefaultLangSufix)) {
					code = code.Remove(code.Length - DefaultLangSufix.Length);
					defaultLangCode = code;
				}

				codes.Add(code);
			}

			DefaultLanguage = new Language(defaultLangCode, LanguagesFolder + defaultLangCode + DefaultLangSufix);
			LanguageCodes = new ReadOnlyCollection<string>(codes);
		}
	}
}
