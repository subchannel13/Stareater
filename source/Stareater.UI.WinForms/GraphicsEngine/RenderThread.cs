using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Win32;
using Stareater.AppData;

namespace Stareater.GraphicsEngine
{
	public class RenderThread
	{
		private Thread thread;
		private Stopwatch watch;
		private bool shouldStop;
		private bool settingsChanged;
		
		private int frameDuration; //in milliseconds
		private Action waitMethod;
		
		#region Lifecycle control
		public RenderThread()
		{
			this.thread = new Thread(renderLoop);
			this.watch = new Stopwatch();
		}
		
		public void Start()
		{
			this.pullSettings();
			this.shouldStop = false;
			SystemEvents.PowerModeChanged += onPowerModeChange;
			
			this.thread.Start();
		}
		
		public void Stop()
		{
			SystemEvents.PowerModeChanged -= onPowerModeChange;
			this.shouldStop = true;
		}
		#endregion
		
		public void OnSettingsChange()
		{
			this.settingsChanged = true;
		}
		
		private void renderLoop()
		{
			while(!this.shouldStop)
			{
				this.watch.Restart();
				
				if (this.settingsChanged)
					this.pullSettings();
				
				//TODO(v0.6) contact renderer
				
				this.waitMethod();
			}
		}
		
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
		
		void onPowerModeChange(object sender, PowerModeChangedEventArgs e)
		{
			if (e.Mode == PowerModes.StatusChange)
				this.OnSettingsChange();
		}
		
		private void pullSettings()
		{
			bool onBattery = System.Windows.Forms.SystemInformation.PowerStatus.PowerLineStatus != System.Windows.Forms.PowerLineStatus.Online; //assumes battery if status is unknown
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
			
			this.frameDuration = SettingsWinforms.Get.UnlimitedFramerate ? 0 : (1000 / SettingsWinforms.Get.Framerate);
			this.settingsChanged = false;
		}
	}
}
