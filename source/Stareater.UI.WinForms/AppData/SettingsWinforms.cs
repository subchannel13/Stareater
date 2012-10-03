using System.Collections.Generic;
using System;
using Stareater.Collections;

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
					instance = new SettingsWinforms();
				return instance;
			}
		}
		#endregion
	}
}
