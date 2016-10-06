using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.ThreadException += guiExceptionLogger;
			
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

		static void guiExceptionLogger(object sender, ThreadExceptionEventArgs e)
		{
#if !DEBUG			
			using(var form = new FormError(e.Exception.ToString()))
					form.ShowDialog();
#else			
			Trace.TraceError(e.Exception.ToString());
#endif
		}
	}
}
