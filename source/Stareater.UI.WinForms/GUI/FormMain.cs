using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Stareater.GUI
{
	internal partial class FormMain : Form
	{
		private Queue<Action> delayedGuiEvents = new Queue<Action>();

		public FormMain()
		{
			InitializeComponent();
			
			postDelayedEvent(showMainMenu);
		}

		private void eventTimer_Tick(object sender, EventArgs e)
		{
			lock (delayedGuiEvents) {
				eventTimer.Enabled = false;

				while (delayedGuiEvents.Count > 0)
					delayedGuiEvents.Dequeue().Invoke();
			}
		}

		#region Delayed Events
		private void postDelayedEvent(Action eventAction)
		{
			lock (delayedGuiEvents) {
				delayedGuiEvents.Enqueue(eventAction);
				eventTimer.Enabled = true;
			}
		}

		private void showMainMenu()
		{
			using (FormMainMenu mainMenu = new FormMainMenu())
				mainMenu.ShowDialog();
		}
		#endregion
	}
}
