using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Zvjezdojedac.GUI;
#if __MonoCS__
using Gtk;
#endif

[assembly: CLSCompliant(false)]
namespace Zvjezdojedac
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			bool runningOnMono = Type.GetType ("Mono.Runtime") != null;
			if(!runningOnMono)
			{
				System.Windows.Forms.Application.EnableVisualStyles();
				System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

				ZvjezdojedacAppContext.GetInstance.MainForm = new FormMain();
				Application.Run(ZvjezdojedacAppContext.GetInstance);
			}
#if __MonoCS__
			else
			{
				Gtk.Application.Init();
				
				GtkFormMain mainForm = new GtkFormMain();
				mainForm.Show();
	//			mainForm.ShowAll();
				
				Gtk.Application.Run();
			}
#endif
		}
	}
}
