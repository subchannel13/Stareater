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

		#region Language events
		private Set<ILanguageListener> languageListeners = new Set<ILanguageListener>();

		public ICollection<ILanguageListener> OnLanguageEvent
		{
			get { return languageListeners; }
		}

		private void LanguageEvent(Action<ILanguageListener> listenerAction)
		{
			foreach (ILanguageListener listener in languageListeners)
				if (listener.IsAlive)
					listenerAction(listener);
				else
					languageListeners.PendRemove(listener);
			languageListeners.ApplyRemove();
		}
		#endregion
	}
}
