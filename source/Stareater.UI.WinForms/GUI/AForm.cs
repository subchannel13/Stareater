using System.Windows.Forms;
using Stareater.Localization;
using Stareater.AppData;

namespace Stareater.GUI
{
	public abstract class AForm : Form, ILanguageListener
	{
		protected AForm()
		{
			SettingsWinforms.Get.OnLanguageEvent.Add(this);
		}

		public bool IsAlive
		{
			get { return !IsDisposed; }
		}

		public abstract void OnLanguageChanged(Language newLanguage);

		protected override void Dispose(bool disposing)
		{
			SettingsWinforms.Get.OnLanguageEvent.Remove(this);
			base.Dispose(disposing);
		}
	}
}
