using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using System.Linq;
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

		private LocalizationManifest(Language defaultLanguage, Language currentLanguage, IEnumerable<LanguageInfo> languageInfos)
		{
			this.CurrentLanguage = currentLanguage;
			this.DefaultLanguage = defaultLanguage;
			this.Languages = new ReadOnlyCollection<LanguageInfo>(languageInfos.ToList());
		}
		#endregion

		public Language CurrentLanguage { get; internal set; }
		public Language DefaultLanguage { get; private set; }

		public ReadOnlyCollection<LanguageInfo> Languages { get; private set; }
		
		public static void Initialize(IEnumerable<LanguageInfo> languages, Language defaultLanguage, Language currentLanguage)
		{
			instance = new LocalizationManifest(defaultLanguage, currentLanguage, languages);
		}
	}
}
