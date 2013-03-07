using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;

namespace Stareater.Localization
{
	public class LocalizationManifest
	{
		#region Singleton
		private static LocalizationManifest instance = null;
		public static LocalizationManifest Get
		{
			get
			{
				if (instance == null)
					throw new InvalidOperationException("Localization manifest is not initialized");
				return instance;
			}
		}

		private LocalizationManifest(Language defaultLanguage, ReadOnlyCollection<string> languageCodes)
		{
			this.DefaultLanguage = defaultLanguage;
			this.LanguageCodes = languageCodes;
		}
		#endregion

		public const string LanguagesFolder = "./languages/";
		private const string DefaultLangSufix = "(default)";

		public Language DefaultLanguage { get; private set; }
		public ReadOnlyCollection<string> LanguageCodes { get; private set; }
		
		private ReadOnlyCollection<KeyValuePair<string, string>> languageNames = null;
		public ReadOnlyCollection<KeyValuePair<string, string>> LanguageNames
		{
			get
			{
				if (languageNames == null)
				{
					List<KeyValuePair<string, string>> names = new List<KeyValuePair<string, string>>();
					foreach (var code in LanguageCodes)
					{
						Language lang = LoadLanguage(code);
						names.Add(new KeyValuePair<string, string>(lang.Code, lang["General"]["LanguageName"].Text()));
					}

					languageNames = new ReadOnlyCollection<KeyValuePair<string, string>>(names);
				}

				return languageNames;
			}
		}

		public Language LoadLanguage(string langCode)
		{
			if (langCode == DefaultLanguage.Code)
				return DefaultLanguage;
			else
				return new Language(langCode, LanguagesFolder + langCode);
		}

		public static void Initialize()
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

			instance = new LocalizationManifest(
				new Language(defaultLangCode, LanguagesFolder + defaultLangCode + DefaultLangSufix),
				new ReadOnlyCollection<string>(codes));
		}
	}
}
