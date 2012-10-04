using System.Collections.Generic;
using IKON.STON.Values;

namespace Stareater.AppData
{
	class SettingsWinforms : Settings
	{
		#region Singleton
		static SettingsWinforms instance = null;

		public static new SettingsWinforms Get
		{
			get
			{
				if (instance == null)
					instance = new SettingsWinforms(loadFile());
				return instance;
			}
		}
		#endregion

		public SettingsWinforms(Dictionary<string, Object> data)
			:base(data)
		{
			// TODO: Complete member initialization
		}
	}
}
