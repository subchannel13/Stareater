using System;
using System.IO;
using Ikadn;
using Ikadn.Ikon;
using Ikadn.Ikon.Types;
using Ikadn.Utilities;
using Stareater.Localization;

namespace Stareater.AppData
{
	public abstract class Settings
	{
		#region Singleton
		protected static Settings instance = null;

		public static Settings Get
		{
			get
			{
				return instance;
			}
		}
		#endregion

		private const string SettingsFileName = "settings.txt";

		public Language Language { get; set; }
		public LastGameInfo LastGame { get; private set; }
		//TODO(later) remember other game options like map shape and size

		private static string SettingsFilePath {
			get {
				return AssetController.Get.FileStorageRootPath + SettingsFileName;
			}
		}
		
		#region Initialization
		protected static void initialize()
		{
			try
			{
				instance.load(loadFile());
			}
			catch(Exception e)
			{
				instance.initDefault();
				
				#if DEBUG
				System.Diagnostics.Trace.TraceError(e.ToString());
				#endif
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
		
		protected virtual void initDefault()
		{
			this.Language = LocalizationManifest.Get.DefaultLanguage;
			this.LastGame = new LastGameInfo();
		}
	
		protected virtual void load(TaggableQueue<object, IkadnBaseObject> data)
		{
			IkonComposite baseData = data.Dequeue(BaseSettingsTag).To<IkonComposite>();
			
			this.Language = LocalizationManifest.Get.LoadLanguage(baseData[LanguageKey].To<string>()); //FIXME(later): Avoid implicit heavy initialization
			this.LastGame = new LastGameInfo(baseData[LastGameKey].To<IkonComposite>());
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
