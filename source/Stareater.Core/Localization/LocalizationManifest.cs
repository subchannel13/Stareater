using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using Stareater.AppData;

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

		private LocalizationManifest(Language defaultLanguage, Language currentLanguage, ReadOnlyCollection<string> languageCodes)
		{
			this.CurrentLanguage = currentLanguage;
			this.DefaultLanguage = defaultLanguage;
			this.LanguageCodes = languageCodes;
		}
		#endregion

		public const string LanguagesFolder = "./languages/";
		private const string DefaultLangSufix = "(default)";

		public Language CurrentLanguage { get; internal set; }
		public Language DefaultLanguage { get; private set; }
		public ReadOnlyCollection<string> LanguageCodes { get; private set; }
		
		private ReadOnlyCollection<KeyValuePair<string, string>> languageNames = null;
		public ReadOnlyCollection<KeyValuePair<string, string>> LanguageNames
		{
			get
			{
				if (languageNames == null)
				{
					var names = new List<KeyValuePair<string, string>>();
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
			var codes = new List<string>();

			foreach (var folder in new DirectoryInfo(LanguagesFolder).EnumerateDirectories()) {
				string code = folder.Name;

				if (folder.Name.EndsWith(DefaultLangSufix, StringComparison.InvariantCultureIgnoreCase)) {
					code = code.Remove(code.Length - DefaultLangSufix.Length);
					defaultLangCode = code;
				}

				codes.Add(code);
			}
			
			var currentLanguageSufix = Settings.Get.LanguageId == defaultLangCode ? DefaultLangSufix : "";
			
			instance = new LocalizationManifest(
				new Language(defaultLangCode, LanguagesFolder + defaultLangCode + DefaultLangSufix),
				new Language(Settings.Get.LanguageId, LanguagesFolder + Settings.Get.LanguageId + currentLanguageSufix),
				new ReadOnlyCollection<string>(codes));
		}
	}
}
