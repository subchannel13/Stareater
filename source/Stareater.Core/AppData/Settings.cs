using System.Collections.Generic;
using System.IO;
using Stareater.Localization;
using IKON.STON;
using IKON.STON.Values;

namespace Stareater.AppData
{
	public class Settings
	{
		#region Singleton
		static Settings instance = null;

		public static Settings Get
		{
			get
			{
				if (instance == null)
					instance = new Settings(loadFile());
				return instance;
			}
		}
		#endregion

		const string SettingsFilePath = "settings.txt";

		public Language Language { get; private set; }

		#region Initialization
		protected Settings(Dictionary<string, Object> data)
		{
			if (data.ContainsKey(BaseSettingsKey))
			{
				string langCode = data[BaseSettingsKey][LanguageKey].AsText().GetText;
				this.Language = new Language(langCode, LocalizationManifest.LanguagesFolder + langCode);
			}
			else
				this.Language = LocalizationManifest.DefaultLanguage;
		}

		protected static Dictionary<string, Object> loadFile()
		{
			Dictionary<string, Object> data = new Dictionary<string,Object>();

			if (File.Exists(SettingsFilePath))
			{
				using (Parser parser = new Parser(new StreamReader(SettingsFilePath)))
					foreach (var value in parser.ParseAll())
						data.Add(value.TypeName.ToLower(), value as Object);
			}

			return data;
		}
		#endregion

		#region Attribute keys
		const string BaseSettingsKey = "base";
		const string LanguageKey = "language";
		const string LastGameKey = "lastgame";
		#endregion
	}
}
