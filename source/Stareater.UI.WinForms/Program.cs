using System;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Localization;
using Stareater.GUI;

namespace StareaterUI
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			LocalizationManifest.Initialize();
			AssetController.Get.Start();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
#if !DEBUG
			try
			{
#endif
				Application.Run(new Stareater.GUI.FormMain());
#if !DEBUG
			}
			catch (Exception e)
			{
				using(var form = new FormError(e.ToString()))
					form.ShowDialog();
			}
#endif
		}
	}
}
