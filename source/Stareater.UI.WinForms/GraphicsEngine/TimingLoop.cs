using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using Stareater.AppData;

namespace Stareater.GraphicsEngine
{
	public class TimingLoop
	{
		private Thread thread;
		private Stopwatch watch;
		private AutoResetEvent unpauseEvent = new AutoResetEvent(true);
		private Control invocationTarget;
		private Action<double> listener;
		
		private bool shouldStop;
		private SignalFlag settingsChanged = new SignalFlag();
		
		private int frameDuration; //in milliseconds
		private Action waitMethod;
		
		#region Lifecycle control
		public TimingLoop(Control invocationTarget, Action<double> listener)
		{
			this.invocationTarget = invocationTarget;
			this.listener = listener;
			this.thread = new Thread(loop);
			this.watch = new Stopwatch();
		}
		
		public void Start()
		{
			this.shouldStop = false;
			this.settingsChanged.Clear();
			SystemEvents.PowerModeChanged += onPowerModeChange;
			this.pullSettings();
			
			this.thread.Start();
		}
		
		public void Stop()
		{
			this.shouldStop = true;
			this.unpauseEvent.Set();
			this.thread.Join();
			
			SystemEvents.PowerModeChanged -= onPowerModeChange;
		}
		#endregion
		
		#region Event messages
		public void Continue()
		{
			this.unpauseEvent.Set();
		}
		
		public void OnSettingsChange()
		{
			this.settingsChanged.Set();
		}
		#endregion
		
		#region The loop
		private void loop()
		{
			while(!this.shouldStop)
			{
				if (settingsChanged.Check())
					this.pullSettings();

				double dt = this.watch.Elapsed.TotalSeconds;
				
				this.watch.Restart();
				unpauseEvent.WaitOne();
				invocationTarget.BeginInvoke(this.listener, dt);
				
				if (this.waitMethod != null)
					this.waitMethod();
			}
		}
		#endregion
		
		#region Frame timing methods
		private void busyWait()
		{
			var sw = new SpinWait();
			while(this.watch.ElapsedMilliseconds < this.frameDuration)
				sw.SpinOnce();
		}
		
		private void sleepWait()
		{
			int sleepFor = (int)(this.frameDuration - this.watch.ElapsedMilliseconds);
			if (sleepFor < 1)
				sleepFor = 1;
			
			Thread.Sleep(sleepFor);
		}
		#endregion
		
		#region Settings
		void onPowerModeChange(object sender, PowerModeChangedEventArgs e)
		{
			if (e.Mode == PowerModes.StatusChange)
				this.OnSettingsChange();
		}
		
		private void pullSettings()
		{
			bool onBattery = SystemInformation.PowerStatus.PowerLineStatus != PowerLineStatus.Online; //assumes battery if status is unknown
			switch(SettingsWinforms.Get.FramerateBusySpinUsage)
			{
				case BusySpinMode.Always:
					this.waitMethod = busyWait;
					break;
				case BusySpinMode.Never:
					this.waitMethod = sleepWait;
					break;
				case BusySpinMode.NotOnBattery:
					this.waitMethod = onBattery ? (Action)sleepWait : busyWait;
					break;
			}
			
			if(SettingsWinforms.Get.VSync || SettingsWinforms.Get.UnlimitedFramerate)
				this.waitMethod = null;
			
			this.frameDuration = 1000 / SettingsWinforms.Get.Framerate;
		}
		#endregion
	}
}
