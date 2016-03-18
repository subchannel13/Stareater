using System;
using System.IO;
using Ikadn;
using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using Ikadn.Utilities;
using Stareater.Localization;
using Stareater.Utils;

namespace Stareater.AppData
{
	public class Settings
	{
		#region Singleton
		protected static Settings instance = null;

		public static Settings Get
		{
			get
			{
				if (instance == null)
					instance = new Settings(loadFile()); //TODO(v0.5) separate loading from constructor and make loading fall back to default if data format is invalid
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
			IkonComposite baseData = (data.CountOf(BaseSettingsTag) > 0) ?
				data.Dequeue(BaseSettingsTag).To<IkonComposite>() :
				new IkonComposite(BaseSettingsTag);
			
			this.Language = baseData.ToOrDefault(
				LanguageKey,
				x => LocalizationManifest.Get.LoadLanguage(x.To<string>()), //FIXME(later): Avoid implicit heavy initialization
				LocalizationManifest.Get.DefaultLanguage
			);
			
			this.LastGame = baseData.ToOrDefault(
				LastGameKey,
				x => new LastGameInfo(x.To<IkonComposite>()),
				new LastGameInfo()
			);
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
			var saveFile = new FileInfo(SettingsFilePath);
			saveFile.Directory.Create();

			using (var output = new StreamWriter(SettingsFilePath))
			{
				var writer = new IkadnWriter(output);
				buildSaveData(writer);
			}
		}

		protected virtual void buildSaveData(IkadnWriter writer)
		{
			var baseSettings = new IkonComposite(BaseSettingsTag);
			baseSettings.Add(LanguageKey, new IkonText(Language.Code));
			baseSettings.Add(LastGameKey, LastGame.BuildSaveData());
			baseSettings.Compose(writer);
		}
		#endregion
		
		#region Attribute keys
		const string BaseSettingsTag = "base";
		const string LanguageKey = "language";
		const string LastGameKey = "lastgame";
		#endregion
	}
}
