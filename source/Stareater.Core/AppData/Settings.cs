using System;
using Ikadn;
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

		public string LanguageId { get; private set; }
		public LastGameInfo LastGame { get; private set; }
		//TODO(v0.6) remember other game options like map shape and size

		public void ChangeLanguage(string id, Language language)
		{
			this.LanguageId = id;
			LocalizationManifest.Get.CurrentLanguage = language;
		}
		
		#region Initialization
		protected static void initialize()
		{
			try
			{
				instance.load(instance.loadData());
			}
			catch(Exception e)
			{
				instance.initDefault();
				
				#if DEBUG
				System.Diagnostics.Trace.TraceError(e.ToString());
				#endif
			}
		}
		
		protected virtual void initDefault()
		{
			this.LastGame = new LastGameInfo();
		}
	
		protected abstract TaggableQueue<object, IkadnBaseObject> loadData();
		
		protected virtual void load(TaggableQueue<object, IkadnBaseObject> data)
		{
			IkonComposite baseData = data.Dequeue(BaseSettingsTag).To<IkonComposite>();
			
			this.LanguageId = baseData[LanguageKey].To<string>();
			this.LastGame = new LastGameInfo(baseData[LastGameKey].To<IkonComposite>());
		}
		#endregion
		
		protected virtual void buildSaveData(IkadnWriter writer)
		{
			var baseSettings = new IkonComposite(BaseSettingsTag);
			baseSettings.Add(LanguageKey, new IkonText(this.LanguageId));
			baseSettings.Add(LastGameKey, this.LastGame.BuildSaveData());
			baseSettings.Compose(writer);
		}
		
		#region Attribute keys
		const string BaseSettingsTag = "base";
		const string LanguageKey = "language";
		const string LastGameKey = "lastgame";
		#endregion
	}
}
