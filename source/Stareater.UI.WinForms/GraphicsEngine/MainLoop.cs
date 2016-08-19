using System;
using System.Collections.Concurrent;
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
		private const int IntTrue = 1;
		private const int IntFalse = -1;

		private GLControl glCanvas;
		private Object lockObj = new Object();
		private Thread thread;
		private Stopwatch watch;
		
		private int resetViewport;
		private int shouldStop;
		private int settingsChanged;

		private int frameDuration; //in milliseconds
		private Action waitMethod;
		
		private readonly ConcurrentQueue<Action> inputEvents = new ConcurrentQueue<Action>();
		private AScene currentRenderer;
		private AScene nextRenderer;
		private Vector2d screenSize;
		
		#region Lifecycle control
		public MainLoop(GLControl glCanvas)
		{
			this.glCanvas = glCanvas;
			this.thread = new Thread(renderLoop);
			this.watch = new Stopwatch();
		}

		public void Start()
		{
			this.currentRenderer = null;
			this.nextRenderer = null;
			this.resetViewport = IntTrue;
			this.shouldStop = IntFalse;
			SystemEvents.PowerModeChanged += onPowerModeChange;
			this.pullSettings();
			
			this.glCanvas.Context.MakeCurrent(null);
			this.glCanvas.KeyPress += keyPress;
			this.glCanvas.MouseMove += this.mouseMove;
			this.glCanvas.MouseWheel += this.mouseScroll;
			this.glCanvas.MouseClick += this.mouseClick;
			this.glCanvas.MouseDoubleClick += this.mouseDoubleClick;
			
			this.thread.Start();
		}
		
		public void Stop()
		{
			this.shouldStop = IntTrue;
			
			SystemEvents.PowerModeChanged -= onPowerModeChange;
			this.glCanvas.KeyPress -= keyPress;
			this.glCanvas.MouseMove -= this.mouseMove;
			this.glCanvas.MouseWheel -= this.mouseScroll;
			this.glCanvas.MouseClick -= this.mouseClick;
			this.glCanvas.MouseDoubleClick -= this.mouseDoubleClick;
			this.thread.Join();
		}
		#endregion
		
		#region Event messages
		public void ChangeScene(AScene renderer)
		{
			lock(this.lockObj)
				this.nextRenderer = renderer;
		}
		
		public void OnResize()
		{
			var screen = Screen.FromControl(this.glCanvas);
			this.screenSize = new Vector2d(screen.Bounds.Width, screen.Bounds.Height);

			this.resetViewport = IntTrue;
		}
		
		public void OnSettingsChange()
		{
			this.settingsChanged = IntTrue;
		}
		#endregion
		
		//TODO(later) remove the need for getting current renderer outside the loop
		public AScene CurrentRenderer
		{
			get { return this.nextRenderer; }
		}
		
		#region The loop
		private void renderLoop()
		{
			this.initLoop();
			
			while(!checkFlag(ref this.shouldStop))
			{
				double dt = Math.Min(this.watch.Elapsed.TotalSeconds, MaxDeltaTime);
				
				this.watch.Restart();
				this.prepareFrameRendering();
				
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

			this.cleanUp();
		}
		
		private void initLoop()
		{
			this.glCanvas.MakeCurrent();
			GalaxyTextures.Get.Load();
			
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
		}

		private void cleanUp()
		{
			GalaxyTextures.Get.Unload();
			if (this.currentRenderer != null)
				this.currentRenderer.Deactivate();
		}
		
		private void prepareFrameRendering()
		{
			if (checkFlag(ref settingsChanged))
					this.pullSettings();
			
			lock(this.lockObj)
				if (this.currentRenderer != this.nextRenderer)
				{
					if (this.currentRenderer != null)
						this.currentRenderer.Deactivate();

					this.resetViewport = IntTrue;
					this.currentRenderer = this.nextRenderer;
					this.currentRenderer.Activate();
				}
			
			Action eventHandler;
			while(this.inputEvents.TryDequeue(out eventHandler))
				if (this.currentRenderer != null)
					eventHandler();

			if (checkFlag(ref this.resetViewport))
			{
				GL.Viewport(this.glCanvas.ClientRectangle); //TODO(v0.6) move to scene object
					
				if (this.currentRenderer != null)
					this.currentRenderer.ResetProjection(this.screenSize, new Vector2d(this.glCanvas.Width, this.glCanvas.Height)); //TODO(v0.6) move to scene object
			}
			
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
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
		}
		#endregion
		
		#region Input
		private void keyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (Thread.CurrentThread != this.thread)
			{
				inputEvents.Enqueue(() => this.keyPress(sender, e));
				return;
			}
			
			this.CurrentRenderer.OnKeyPress(e);
		}
		
		private void mouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (Thread.CurrentThread != this.thread)
			{
				inputEvents.Enqueue(() => this.mouseDoubleClick(sender, e));
				return;
			}
			
			this.CurrentRenderer.OnMouseDoubleClick(e);
		}
		
		private void mouseClick(object sender, MouseEventArgs e)
		{
			if (Thread.CurrentThread != this.thread)
			{
				inputEvents.Enqueue(() => this.mouseClick(sender, e));
				return;
			}
			
			this.CurrentRenderer.OnMouseClick(e);
		}
		
		private void mouseMove(object sender, MouseEventArgs e)
		{
			if (Thread.CurrentThread != this.thread)
			{
				inputEvents.Enqueue(() => this.mouseMove(sender, e));
				return;
			}
			
			this.CurrentRenderer.OnMouseMove(e);
		}

		private void mouseScroll(object sender, MouseEventArgs e)
		{
			if (Thread.CurrentThread != this.thread)
			{
				inputEvents.Enqueue(() => this.mouseScroll(sender, e));
				return;
			}
			
			this.CurrentRenderer.OnMouseScroll(e);
		}
		#endregion

		private static bool checkFlag(ref int flag)
		{
			return Interlocked.CompareExchange(ref flag, IntFalse, IntTrue) == IntTrue;
		}
	}
}
