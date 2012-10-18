using System.Collections.Generic;
using System.IO;
using Stareater.Localization;
using Ikon.Ston;
using Ikon.Ston.Values;
using Ikon;

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

		public Language Language { get; set; }

		#region Initialization
		protected Settings(Dictionary<string, Object> data)
		{
			if (data.ContainsKey(BaseSettingsKey))
			{
				string langCode = (data[BaseSettingsKey][LanguageKey] as Text).GetText;
				this.Language = LocalizationManifest.LoadLanguage(langCode);
			}
			else
				this.Language = LocalizationManifest.DefaultLanguage;
		}

		protected static Dictionary<string, Object> loadFile()
		{
			Dictionary<string, Object> data = new Dictionary<string,Object>();

			if (File.Exists(SettingsFilePath))
			{
				using (Ikon.Ston.Parser parser = new Ikon.Ston.Parser(new StreamReader(SettingsFilePath)))
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

		public void Save()
		{
			using (var output = new StreamWriter(SettingsFilePath))
			{
				Composer composer = new Composer(output);
				buildSaveData(composer);
			}
		}

		protected virtual void buildSaveData(Composer composer) {
			Object baseSettings = new Object(BaseSettingsKey);
			baseSettings.Add(LanguageKey, new Text(Language.Code));
			composer.Write(baseSettings);
		}
	}
}
