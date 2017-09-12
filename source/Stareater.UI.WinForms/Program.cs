using Stareater.AppData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace StareaterUI
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.ThreadException += guiExceptionLogger;

#if !DEBUG
			try
			{
#endif
				parseArguments(new Queue<string>(args));
				Application.Run(new Stareater.GUI.FormMain());
#if !DEBUG
			}
			catch (Exception e)
			{
				using(var form = new Stareater.GUI.FormError(e.ToString()))
					form.ShowDialog();
			}
#endif
		}

		static void guiExceptionLogger(object sender, ThreadExceptionEventArgs e)
		{
#if !DEBUG			
			using(var form = new Stareater.GUI.FormError(e.Exception.ToString()))
					form.ShowDialog();
#else			
			Trace.TraceError(e.Exception.ToString());
#endif
		}

		private static void parseArguments(Queue<string> args)
		{
			while(args.Count > 0)
			{
				var option = args.Dequeue();

				switch(option)
				{
					case "-root":
						if (args.Count == 0)
							throw new ArgumentException("Missing path arguments for " + option);
						SettingsWinforms.Get.DataRootPath = args.Dequeue();
						break;
					default:
						throw new ArgumentException("Unknown option " + option);
				}
			}
		}
	}
}
