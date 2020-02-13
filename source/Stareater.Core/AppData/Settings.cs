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
		public static Settings Get { get; protected set; } = null;
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
				var data = Get.loadData();
				if (data != null)
					Get.load(data);
				else
					Get.initDefault();
			}
#if DEBUG
#pragma warning disable CA1031 // Do not catch general exception types
			catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
			{
				System.Diagnostics.Trace.TraceError(e.ToString());
				Get.initDefault();
			}
#else
#pragma warning disable CA1031 // Do not catch general exception types
			catch(Exception)
#pragma warning restore CA1031 // Do not catch general exception types
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
			if (data == null)
				throw new ArgumentNullException(nameof(data));

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
