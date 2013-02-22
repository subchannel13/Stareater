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
		public LastGameInfo LastGame { get; private set; }

		#region Initialization
		protected Settings(ValueQueue data)
		{
			if (data.CountOf(BaseSettingsKey)>0) {
				var baseData = data.Dequeue(BaseSettingsKey).To<ObjectValue>();
				string langCode = baseData[LanguageKey].To<string>();
				this.Language = LocalizationManifest.LoadLanguage(langCode);
				this.LastGame = new LastGameInfo(baseData[LastGameKey].To<ObjectValue>());
			}
			else {
				this.Language = LocalizationManifest.DefaultLanguage;
				this.LastGame = new LastGameInfo();
			}
		}

		protected static ValueQueue loadFile()
		{
			ValueQueue data;

			if (File.Exists(SettingsFilePath))
				using (Ikon.Ston.Parser parser = new Ikon.Ston.Parser(new StreamReader(SettingsFilePath)))
					data = parser.ParseAll();
			else
				data = new ValueQueue();

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
				IkonWriter writer = new IkonWriter(output);
				buildSaveData(writer);
			}
		}

		protected virtual void buildSaveData(IkonWriter writer)
		{
			ObjectValue baseSettings = new ObjectValue(BaseSettingsKey);
			baseSettings.Add(LanguageKey, new TextValue(Language.Code));
			baseSettings.Add(LastGameKey, LastGame.BuildSaveData());
			baseSettings.Compose(writer);
		}
	}
}
