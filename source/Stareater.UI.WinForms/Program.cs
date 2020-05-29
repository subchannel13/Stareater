using Stareater.AppData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
			ErrorReporter.Get.OnException += logException;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

#if !DEBUG
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.ThreadException += guiExceptionLogger;

			try
			{
#endif
				parseArguments(new Queue<string>(args));
				Application.Run(new Stareater.GUI.FormMain());
#if !DEBUG
			}
			catch (Exception e)
			{
				logException(e);
			}
#endif
		}

		static void guiExceptionLogger(object sender, ThreadExceptionEventArgs e)
		{
			logException(e.Exception);
		}

		static void logException(Exception e)
		{
			var textBuilder = new StringBuilder();
			unpackText(textBuilder, e);

#if !DEBUG
			using(var form = new Stareater.GUI.FormError(textBuilder.ToString()))
					form.ShowDialog();
#else
			Trace.TraceError(textBuilder.ToString());
#endif
		}

		private static void unpackText(StringBuilder textBuilder, Exception e)
		{
			textBuilder.AppendLine(e.ToString());

			if (e is AggregateException)
				foreach (var innerException in (e as AggregateException).InnerExceptions)
					unpackText(textBuilder, innerException);

			if (e is System.Reflection.ReflectionTypeLoadException)
				foreach (var innerException in (e as System.Reflection.ReflectionTypeLoadException).LoaderExceptions)
					unpackText(textBuilder, innerException);
		}

		private static void parseArguments(Queue<string> args)
		{
			while(args.Count > 0)
			{
				var option = args.Dequeue();

				switch(option)
				{
					case "-plugins":
						if (args.Count == 0)
							throw new ArgumentException("Missing path arguments for " + option);
						SettingsWinforms.Get.PluginRootPath = args.Dequeue();
						break;
					case "-root":
						if (args.Count == 0)
							throw new ArgumentException("Missing path arguments for " + option);
						SettingsWinforms.Get.DataRootPath = args.Dequeue();
						break;
					case "-wait":
						if (args.Count == 0)
							throw new ArgumentException("Missing path arguments for " + option);
						int waitTime;
						if (!int.TryParse(args.Dequeue(), out waitTime) || args.Count == 0)
							throw new ArgumentException("Invalid arguments format for " + option);
						waitTime = Math.Min(waitTime, 60);
						if (waitTime > 0)
							Thread.Sleep(waitTime * 1000);
						break;
					default:
						throw new ArgumentException("Unknown option " + option);
				}
			}
		}
	}
}
