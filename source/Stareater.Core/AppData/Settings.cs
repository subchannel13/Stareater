using System;
using System.IO;
using Ikadn;
using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using Ikadn.Utilities;
using Stareater.Localization;

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

		private const string SettingsFileName = "settings.txt";

		public Language Language { get; set; }
		public LastGameInfo LastGame { get; private set; }

		private static string SettingsFilePath {
			get {
				return AssetController.Get.FileStorageRootPath + SettingsFileName;
			}
		}
		
		#region Initialization
		protected Settings(TaggableQueue<object, IkadnBaseObject> data)
		{
			if (data.CountOf(BaseSettingsKey) > 0) {
				var baseData = data.Dequeue(BaseSettingsKey).To<IkonComposite>();
				string langCode = baseData[LanguageKey].To<string>();
				this.Language = LocalizationManifest.Get.LoadLanguage(langCode); //FIXME: Avoid implicit heavy initialization
				this.LastGame = new LastGameInfo(baseData[LastGameKey].To<IkonComposite>());
			}
			else {
				this.Language = LocalizationManifest.Get.DefaultLanguage;
				this.LastGame = new LastGameInfo();
			}
		}

		protected static TaggableQueue<object, IkadnBaseObject> loadFile()
		{
			TaggableQueue<object, IkadnBaseObject> data;

			if (File.Exists(SettingsFilePath))
				using (var parser = new IkonParser(new StreamReader(SettingsFilePath)))
					data = parser.ParseAll();
			else
				data = new TaggableQueue<object, IkadnBaseObject>();

			return data;
		}
		#endregion

		#region Saving
		public void Save()
		{
			FileInfo saveFile = new FileInfo(SettingsFilePath);
			saveFile.Directory.Create();

			using (var output = new StreamWriter(SettingsFilePath))
			{
				IkadnWriter writer = new IkadnWriter(output);
				buildSaveData(writer);
			}
		}

		protected virtual void buildSaveData(IkadnWriter writer)
		{
			IkonComposite baseSettings = new IkonComposite(BaseSettingsKey);
			baseSettings.Add(LanguageKey, new IkonText(Language.Code));
			baseSettings.Add(LastGameKey, LastGame.BuildSaveData());
			baseSettings.Compose(writer);
		}
		#endregion
		
		#region Attribute keys
		const string BaseSettingsKey = "base";
		const string LanguageKey = "language";
		const string LastGameKey = "lastgame";
		#endregion
	}
}
