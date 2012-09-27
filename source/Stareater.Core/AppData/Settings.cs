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
					instance = new Settings();
				return instance;
			}
		}
		#endregion

		const string SettingsFilePath = "settings.txt";

		public Language Language { get; private set; }

		protected Settings()
		{
			Dictionary<string, Object> data = new Dictionary<string, Object>();

			if (File.Exists(SettingsFilePath)) {
				StreamReader stream = new StreamReader(SettingsFilePath);
				Parser parser = new Parser(stream);
				
				while (parser.HasNext()) {
					Object dataChunk = parser.ParseNext() as Object;
					data.Add(dataChunk.TypeName.ToLower(), dataChunk);
				}
				stream.Close();
			}

			loadData(data);
		}

		protected virtual void loadData(Dictionary<string, Object> data)
		{
			if (data.ContainsKey(BaseSettingsKey)) {
				string langCode = data[BaseSettingsKey][LanguageKey].AsText().GetText;
				this.Language = new Language(langCode, LocalizationManifest.LanguagesFolder + langCode);
			}
			else
				this.Language = LocalizationManifest.DefaultLanguage;
		}

		#region Attribute keys
		const string BaseSettingsKey = "base";
		const string LanguageKey = "language";
		const string LastGameKey = "lastgame";
		#endregion
	}
}
