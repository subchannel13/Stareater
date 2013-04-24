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

namespace Stareater.GUI
{
	internal partial class FormMain : Form
	{
		const double DefaultViewSize = 16;
		const double ZoomBase = 1.2f;
		const float FarZ = -10;

		bool glReady = false;
		bool invalidateViewport = true;
		private Matrix4 invProjection;
		private int zoomLevel = 0;

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
		}

		private void glCanvas_Resize(object sender, EventArgs e)
		{
			invalidateViewport = true;
			glCanvas.Refresh();
		}

		private void glCanvas_Paint(object sender, PaintEventArgs e)
		{
			if (!glReady) return;

			if (invalidateViewport) {
				GL.Viewport(glCanvas.Location, glCanvas.Size);
				double aspect = glCanvas.Width / (double)glCanvas.Height;
				double semiRadius = 0.5 * DefaultViewSize / Math.Pow(ZoomBase, zoomLevel);

				GL.MatrixMode(MatrixMode.Projection);
				GL.LoadIdentity();
				GL.Ortho(-aspect * semiRadius, aspect * semiRadius, -semiRadius, semiRadius, 0, -FarZ);

				GL.GetFloat(GetPName.ProjectionMatrix, out invProjection);
				invProjection.Invert();
			}

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
			
			glCanvas.SwapBuffers();
		}

		#endregion
		
	}
}
