using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Data;
using Stareater.GLRenderers;
using Stareater.GUI.Reports;
using Stareater.Localization;
using Stareater.Players.Reports;

namespace Stareater.GUI
{
	internal partial class FormMain : Form, IGameStateListener, IGalaxyViewListener
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
		private GameController controller = null;
		private OpenReportVisitor reportOpener;		
		
		private bool aisReady = false;
		private bool humansReady = false;

		public FormMain()
		{
			InitializeComponent();

			this.controller = new GameController();
			this.reportOpener = new OpenReportVisitor(showDevelopment, showResearch);
			
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
			this.endTurnButton.Text = context["EndTurn"].Text();
			this.returnButton.Text = context["Return"].Text();
			this.mainMenuToolStripMenuItem.Text = context["MainMenu"].Text();
			this.developmentToolStripMenuItem.Text = context["DevelopmentMenu"].Text();
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

		private void endTurnButton_Click(object sender, EventArgs e)
		{
			lock(this) {
				humansReady = true;
			}
			
			tryEndTurn();
		}
		
		private void returnButton_Click(object sender, EventArgs e)
		{
			if (currentRenderer == systemRenderer)
				switchToGalaxyView();
		}
		
		private void mainMenuToolStripMenuItem_Click(object sender, EventArgs e)
		{
			postDelayedEvent(showMainMenu);
		}
		
		private void designsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using(var form = new FormShipDesignList(controller))
				form.ShowDialog();
		}
		
		private void developmentToolStripMenuItem_Click(object sender, EventArgs e)
		{
			postDelayedEvent(showDevelopment);
		}
		
		private void researchToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using(var form = new FormResearch(controller))
				form.ShowDialog();
		}
		
		private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using(var form = new FormReports(controller.Reports))
				if (form.ShowDialog() == DialogResult.OK)
					form.Result.Accept(this.reportOpener);
		}
		
		#region Delayed Events
		private void postDelayedEvent(Action eventAction)
		{
			lock (delayedGuiEvents) {
				delayedGuiEvents.Enqueue(eventAction);
				if (this.InvokeRequired)
					this.BeginInvoke(new Action(eventTimer.Start));
				else
					eventTimer.Start();
			}
		}
		private void startEventTimer()
		{
			eventTimer.Start();
		}

		private void showDevelopment()
		{
			using(var form = new FormDevelopment(controller))
				form.ShowDialog();
		}
		
		private void showMainMenu()
		{
			using (FormMainMenu form = new FormMainMenu(this.controller))
				if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
					switch (form.Result) {
						case MainMenuResult.NewGame:
							postDelayedEvent(showNewGame);
							break;
						case MainMenuResult.SaveGame:
							postDelayedEvent(showSaveGame);
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
					this.controller.Stop();	
					form.CreateGame(controller);
					this.controller.Start(this);
					this.restartRenderers();
				}
				else
					postDelayedEvent(showMainMenu);
			}
		}

		private void showResearch()
		{
			using(var form = new FormResearch(controller))
				form.ShowDialog();
		}
		
		private void showSaveGame()
		{
			var saveController = new SavesController(controller);
			
			using(var form = new FormSaveLoad(saveController))
				if (form.ShowDialog() != DialogResult.OK)
					postDelayedEvent(showMainMenu);
				else if (form.Result == MainMenuResult.LoadGame) {
					this.controller.Stop();
					saveController.Load(form.SelectedGameData);
					this.controller.Start(this);
					this.restartRenderers();
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
		
		private void restartRenderers()
		{
			if (galaxyRenderer != null) {
				galaxyRenderer.DetachFromCanvas();
				galaxyRenderer.Unload();
			}
			
			if (systemRenderer != null) {
				systemRenderer.DetachFromCanvas();
				systemRenderer.Unload();
			}
			
			galaxyRenderer = new GalaxyRenderer(controller, this);
			galaxyRenderer.Load();
			
			systemRenderer = new SystemRenderer(switchToGalaxyView, constructionManagement);
			
			switchToGalaxyView();
			redraw();
		}
		
		private void tryEndTurn()
		{
			lock(this)
			{
				if (!aisReady || !humansReady)
					return;
				
				aisReady = false;
				humansReady = false;
			}
			
			controller.EndGalaxyPhase();

			if (galaxyRenderer != null) galaxyRenderer.ResetLists();
			if (systemRenderer != null) systemRenderer.ResetLists();
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
				GL.Viewport(glCanvas.ClientRectangle);
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
#if DEBUG
			try {
#endif
			if (glReady && currentRenderer != null)
				currentRenderer.Draw(dt);
#if DEBUG
			} catch(Exception ex)
			{
				Trace.WriteLine(ex);
			}
#endif
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
		
		private void switchToGalaxyView()
		{
			if (currentRenderer == systemRenderer)
				systemRenderer.DetachFromCanvas();
			
			galaxyRenderer.AttachToCanvas(glCanvas);
			currentRenderer = galaxyRenderer;
			
			constructionManagement.Visible = false;
			endTurnButton.Visible = true;
			returnButton.Visible = false;
		}
		
		#endregion
		
		
		#region IGameStateListener implementation
		public void OnAiGalaxyPhaseDone()
		{
			lock(this)
			{
				aisReady = true;
			}
			
			postDelayedEvent(tryEndTurn);
		}
		
		public void OnNewTurn()
		{
			if (this.InvokeRequired) {
				postDelayedEvent(this.OnNewTurn);
				return;
			}
			
			if (galaxyRenderer != null) galaxyRenderer.ResetLists();
			if (systemRenderer != null) systemRenderer.ResetLists();
		}
		
		public void OnCombatPhaseStart()
		{
			if (this.InvokeRequired) {
				postDelayedEvent(this.OnCombatPhaseStart);
				return;
			}
			
			//TODO(v0.5): open conflict GUI
			
			controller.EndCombatPhase();
		}
		#endregion
		
		#region IGalaxyViewListener
		void IGalaxyViewListener.FleetSelected(IdleFleetInfo fleetInfo)
		{
			this.constructionManagement.Visible = false;
			this.fleetPanel.Visible = true;
		}
		
		void IGalaxyViewListener.SystemOpened(StarSystemController systemController)
		{
			galaxyRenderer.DetachFromCanvas();
			
			systemRenderer.AttachToCanvas(glCanvas);
			systemRenderer.SetStarSystem(systemController);
			currentRenderer = systemRenderer;
			
			constructionManagement.Visible = true;
			endTurnButton.Visible = false;
			returnButton.Visible = true;
		}
		
		void IGalaxyViewListener.SystemSelected(StarSystemController systemController)
		{
			//TODO(v0.5)
			this.constructionManagement.Visible = false;
			this.fleetPanel.Visible = false;
		}
		#endregion
		
		
	}
}
