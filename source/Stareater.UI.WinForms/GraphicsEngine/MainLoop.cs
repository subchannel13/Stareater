using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.AppData;
using Stareater.GLRenderers;

namespace Stareater.GraphicsEngine
{
	internal class MainLoop
	{
		private const float MaxDeltaTime = 0.5f;
		private const float MinDeltaTime = 0.005f;
		
		private GLControl glCanvas;
		private Object lockObj = new Object();
		private Vector2d screenSize;
		private Thread thread;
		private Stopwatch watch;
		
		private bool resetViewport;
		private bool shouldStop;
		private bool settingsChanged;

		private int frameDuration; //in milliseconds
		private ARenderer nextRenderer;
		private Action waitMethod;
		
		#region Lifecycle control
		public MainLoop(GLControl glCanvas)
		{
			this.glCanvas = glCanvas;
			this.thread = new Thread(renderLoop);
			this.watch = new Stopwatch();
		}
		
		public void Start()
		{
			this.nextRenderer = null;
			this.resetViewport = true;
			this.shouldStop = false;
			SystemEvents.PowerModeChanged += onPowerModeChange;
			this.pullSettings();
			
			this.glCanvas.Context.MakeCurrent(null);
			this.thread.Start();
		}
		
		public void Stop()
		{
			SystemEvents.PowerModeChanged -= onPowerModeChange;
			this.shouldStop = true;
		}
		#endregion
		
		#region Event messages
		public void ChangeScene(ARenderer renderer)
		{
			this.nextRenderer = renderer;
		}
		
		public void OnResize()
		{
			var screen = Screen.FromControl(this.glCanvas);
			this.screenSize = new Vector2d(screen.Bounds.Width, screen.Bounds.Height);
			
			lock(this.lockObj)
				this.resetViewport = true;
		}
		
		public void OnSettingsChange()
		{
			lock(this.lockObj)
				this.settingsChanged = true;
		}
		#endregion
		
		public ARenderer CurrentRenderer
		{
			get { return this.nextRenderer; }
		}
		
		private void renderLoop()
		{
			ARenderer currentRenderer;
			
			this.glCanvas.MakeCurrent();
			GalaxyTextures.Get.Load();
			
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			
			while(!this.shouldStop)
			{
				double dt = this.watch.Elapsed.TotalSeconds;
				this.watch.Restart();
				
				lock(this.lockObj)
					if (this.settingsChanged)
						this.pullSettings();
				
				
				currentRenderer = this.nextRenderer;
				lock(this.lockObj)
					if (resetViewport) {
						resetViewport = false;
						GL.Viewport(glCanvas.ClientRectangle); //TODO(v0.6) move to scene object
						
						if (currentRenderer != null)
							currentRenderer.ResetProjection(this.screenSize); //TODO(v0.6) move to scene object
					}
				
				GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
				GL.MatrixMode(MatrixMode.Modelview);
				GL.LoadIdentity();
				
				if (dt > MaxDeltaTime)
					dt = MaxDeltaTime;
	#if DEBUG
				try {
	#endif
				if (currentRenderer != null/* && gameController.State == GameState.Running*/)
					currentRenderer.Draw(dt); //TODO(v0.6) move to scene object
	#if DEBUG
				} catch(Exception ex)
				{
					Trace.WriteLine(ex);
				}
	#endif
				glCanvas.SwapBuffers();
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
