using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Stareater.Localization
{
	public static class LocalizationManifest
	{
		public const string LanguagesFolder = "./languages/";
		private const string DefaultLangSufix = "(default)";

		public static Language DefaultLanguage { get; private set; }
		public static ReadOnlyCollection<string> LanguageCodes { get; private set; }
		
		private static ReadOnlyCollection<KeyValuePair<string, string>> languageNames = null;
		public static ReadOnlyCollection<KeyValuePair<string, string>> LanguageNames
		{
			get
			{
				if (languageNames == null)
				{
					List<KeyValuePair<string, string>> names = new List<KeyValuePair<string, string>>();
					foreach (var code in LanguageCodes)
					{
						Language lang = LoadLanguage(code);
						names.Add(new KeyValuePair<string, string>(lang.Code, lang["General"]["LanguageName"]));
					}

					languageNames = new ReadOnlyCollection<KeyValuePair<string, string>>(names);
				}

				return languageNames;
			}
		}

		public static Language LoadLanguage(string langCode)
		{
			if (langCode == DefaultLanguage.Code)
				return DefaultLanguage;
			else
				return new Language(langCode, LanguagesFolder + langCode);
		}

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
