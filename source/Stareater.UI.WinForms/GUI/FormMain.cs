using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using OpenTK.Graphics.OpenGL;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.GLRenderers;
using Stareater.Localization;

namespace Stareater.GUI
{
	internal partial class FormMain : Form
	{
		private const float MaxDeltaTime = 0.5f;
		private const float MinDeltaTime = 0.005f;

		private bool glReady = false;
		private bool resetViewport = true;
		private DateTime lastRender = DateTime.UtcNow;
		
		private IRenderer currentRenderer;
		private GalaxyRenderer galaxyRenderer;
		private SystemRenderer systemRenderer;

		private Queue<Action> delayedGuiEvents = new Queue<Action>();
		private GameController controller = new GameController();

		public FormMain()
		{
			InitializeComponent();

			setLanguage();

			postDelayedEvent(showMainMenu);
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			SettingsWinforms.Get.Save();
		}

		private void setLanguage()
		{
			this.Font = SettingsWinforms.Get.FormFont;

			Context context = SettingsWinforms.Get.Language["FormMain"];

		}

		private void eventTimer_Tick(object sender, EventArgs e)
		{
			lock (delayedGuiEvents) {
				eventTimer.Stop();

				while (delayedGuiEvents.Count > 0)
					delayedGuiEvents.Dequeue().Invoke();
			}
		}
		
		private void glRedrawTimer_Tick(object sender, EventArgs e)
		{
			glCanvas.Refresh();
		}
		
		private void mainMenuToolStripMenuItem_Click(object sender, EventArgs e)
		{
			postDelayedEvent(showMainMenu);
		}

		#region Delayed Events
		private void postDelayedEvent(Action eventAction)
		{
			lock (delayedGuiEvents) {
				delayedGuiEvents.Enqueue(eventAction);
				eventTimer.Start();
			}
		}

		private void showMainMenu()
		{
			using (FormMainMenu form = new FormMainMenu())
				if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
					switch (form.Result) {
						case MainMenuResult.NewGame:
							postDelayedEvent(showNewGame);
							break;
						case MainMenuResult.Settings:
							postDelayedEvent(showSettings);
							break;
						case MainMenuResult.Quit:
							Close();
							break;
						default:
							postDelayedEvent(showMainMenu);
							break;
					}
		}

		private void showNewGame()
		{
			using (FormNewGame form = new FormNewGame()) {
				form.Initialize();
				if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
					form.CreateGame(controller);
					galaxyRenderer = new GalaxyRenderer(controller, systemOpened);
					galaxyRenderer.Load();
					galaxyRenderer.AttachToCanvas(glCanvas);
					currentRenderer = galaxyRenderer;
					
					systemRenderer = new SystemRenderer(systemClosed);
					redraw();
				}
				else
					postDelayedEvent(showMainMenu);
			}
		}

		private void showSettings()
		{
			using (FormSettings form = new FormSettings())
				if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
					setLanguage();
			postDelayedEvent(showMainMenu);
		}
		#endregion

		private void redraw()
		{
			if (controller.State != Controllers.Data.GameState.Running)
				return;
		}
		
		#region Canvas events

		private void glCanvas_Load(object sender, EventArgs e)
		{
			glReady = true;
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
		}

		private void glCanvas_Paint(object sender, PaintEventArgs e)
		{
			if (!glReady) return;

			if (resetViewport) {
				GL.Viewport(glCanvas.Location, glCanvas.Size);
				resetViewport = false;
				
				if (glReady && currentRenderer != null)
					currentRenderer.ResetProjection();
			}
			
			var thisMoment = DateTime.UtcNow;
			double dt = (thisMoment - lastRender).TotalSeconds;
			
			if (dt < MinDeltaTime)
				return;
			if (dt > MaxDeltaTime)
				dt = MaxDeltaTime;

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();

			if (glReady && currentRenderer != null)
				currentRenderer.Draw(dt);

			lastRender = thisMoment;
			glCanvas.SwapBuffers();
			
			//this.Text = (1 / dt).ToString("0.#");
		}

		private void glCanvas_Resize(object sender, EventArgs e)
		{
			resetViewport = true;
			glCanvas.Refresh();
		}
		
		#endregion

		#region Renderer events
		
		private void systemOpened(StarSystemController systemController)
		{
			galaxyRenderer.DetachFromCanvas();
			
			systemRenderer.AttachToCanvas(glCanvas);
			systemRenderer.SetStarSystem(systemController);
			currentRenderer = systemRenderer;
			
			constructionManagement.Visible = true;
		}
		
		private void systemClosed()
		{
			systemRenderer.DetachFromCanvas();
			
			galaxyRenderer.AttachToCanvas(glCanvas);
			currentRenderer = galaxyRenderer;
			
			constructionManagement.Visible = false;
		}
		
		#endregion
	}
}
