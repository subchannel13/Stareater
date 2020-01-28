using Ikadn;
using Ikadn.Ikon.Types;
using Ikadn.Utilities;
using Stareater.Localization;
using System;

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
				var data = instance.loadData();
				if (data != null)
					instance.load(data);
				else
					instance.initDefault();
			}
			#if DEBUG
			catch(Exception e)
			{
				System.Diagnostics.Trace.TraceError(e.ToString());
			}
			#else
			catch(Exception)
			{
				instance.initDefault();
			}
			#endif
		}
		
		protected virtual void initDefault()
		{
			this.LanguageId = null;
			this.LastGame = new LastGameInfo();
		}
	
		protected abstract LabeledQueue<object, IkadnBaseObject> loadData();
		
		protected virtual void load(LabeledQueue<object, IkadnBaseObject> data)
		{
			var baseData = data.Dequeue(BaseSettingsTag).To<IkonComposite>();
			
			this.LanguageId = baseData[LanguageKey].To<string>();
			this.LastGame = LastGameInfo.Load(baseData[LastGameKey].To<IkonComposite>());
		}
		#endregion
		
		protected virtual void buildSaveData(IkadnWriter writer)
		{
			var baseSettings = new IkonComposite(BaseSettingsTag)
			{
				{ LanguageKey, new IkonText(this.LanguageId) },
				{ LastGameKey, this.LastGame.BuildSaveData() }
			};
			baseSettings.Compose(writer);
		}
		
		#region Attribute keys
		const string BaseSettingsTag = "base";
		const string LanguageKey = "language";
		const string LastGameKey = "lastgame";
		#endregion
	}
}
