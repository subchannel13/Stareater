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

		public Language Language { get; private set; }

		protected Settings()
		{
			Parser parser = new Parser(new StreamReader("settings.txt"));
			Object allSettings = parser.ParseNext() as Object;

			this.Language = new Language(allSettings[LanguageKey].AsText().GetText);
		}

		#region Attribute keys
		const string LanguageKey = "language";
		#endregion
	}
}
