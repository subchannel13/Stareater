using System;
using System.Diagnostics;
using System.Threading;
using Stareater.AppData;

namespace Stareater.GraphicsEngine
{
	public class RenderThread
	{
		private Thread thread;
		private Stopwatch watch;
		private bool shouldStop;
		
		private int frameDuration; //in milliseconds
		private Action waitMethod;
		
		public RenderThread()
		{
			this.thread = new Thread(renderLoop);
			this.watch = new Stopwatch();
		}
		
		public void Start()
		{
			this.pullSettings();
			this.shouldStop = false;
			this.thread.Start();
		}
		
		public void Stop()
		{
			this.shouldStop = true;
		}
		
		private void renderLoop()
		{
			while(!this.shouldStop)
			{
				this.watch.Restart();
				
				//TODO(v0.6) contact renderer
				
				this.waitMethod();
			}
		}
		
		private void sleepWait()
		{
			int sleepFor = (int)(this.frameDuration - this.watch.ElapsedMilliseconds);
			if (sleepFor < 1)
				sleepFor = 1;
			
			Thread.Sleep(sleepFor);
		}
		
		private void pullSettings()
		{
			this.frameDuration = 1000 / SettingsWinforms.Get.Framerate;
			//TODO(v0.6) select wait method
			this.waitMethod = sleepWait;
		}
	}
}
