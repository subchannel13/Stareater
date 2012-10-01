using Stareater.Localization;
using System;

namespace Stareater.AppData
{
	public interface ILanguageListener
	{
		void OnLanguageChanged(Language newLanguage);
	}
}
