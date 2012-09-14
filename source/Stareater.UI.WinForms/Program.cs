using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Stareater_WinForms_UI
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
			Application.Run(new Stareater.GUI.FormMain());
		}
	}
}
