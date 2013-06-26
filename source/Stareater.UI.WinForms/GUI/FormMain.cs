using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stareater.Localization;
using Stareater.AppData;
using Stareater.Controllers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.GLRenderers;

namespace Stareater.GUI
{
	internal partial class FormMain : Form
	{
		private const float MaxDeltaTime = 0.5f;

		private bool glReady = false;
		private DateTime lastRender = DateTime.UtcNow;
		private IRenderer glRenderer = null;

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
				if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					switch (form.Result)
					{
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
				if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					form.CreateGame(controller);
					glRenderer = new GalaxyRenderer(controller);
					glRenderer.AttachToCanvas(glCanvas);
					redraw();
				}
				else
					postDelayedEvent(showMainMenu);
			}
		}

		private void showSettings()
		{
			using (FormSettings form = new FormSettings())
				if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();

			var thisMoment = DateTime.UtcNow;
			double dt = Math.Min(Math.Max((thisMoment - lastRender).TotalSeconds, 0), MaxDeltaTime);

			if (glReady && glRenderer != null)
				glRenderer.Draw(dt);

			lastRender = thisMoment;
			glCanvas.SwapBuffers();
			
			//this.Text = (1 / dt).ToString("0.#");
		}

		#endregion

		private void glRedrawTimer_Tick(object sender, EventArgs e)
		{
			glCanvas.Refresh();
		}
		
	}
}
