using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Combat;
using Stareater.GLRenderers;
using Stareater.GUI.Reports;
using Stareater.Localization;
using Stareater.Players.Reports;

namespace Stareater.GUI
{
	internal partial class FormMain : Form, IGameStateListener, IBattleEventListener, IGalaxyViewListener
	{
		private const float MaxDeltaTime = 0.5f;
		private const float MinDeltaTime = 0.005f;

		private bool glReady = false;
		private bool resetViewport = true;
		private DateTime lastRender = DateTime.UtcNow;
		
		private ARenderer currentRenderer;
		private GalaxyRenderer galaxyRenderer;
		private SystemRenderer systemRenderer;
		private SpaceCombatRenderer combatRenderer;

		private Queue<Action> delayedGuiEvents = new Queue<Action>();
		private GameController gameController = null;
		private PlayerController[] playerControllers = null;
		private FleetController fleetController = null;
		private SpaceBattleController conflictController = null;
		private OpenReportVisitor reportOpener;
		private int currentPlayerIndex = 0;
		
		public FormMain()
		{
			InitializeComponent();

			this.gameController = new GameController();
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
		
		private PlayerController currentPlayer
		{
			get { return this.playerControllers[currentPlayerIndex]; }
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
			this.currentPlayer.EndGalaxyPhase();
			
			if (this.currentPlayerIndex < this.playerControllers.Length - 1)
			{
				this.currentPlayerIndex++;
				this.galaxyRenderer.CurrentPlayer = this.currentPlayer;
			}

			if (galaxyRenderer != null) galaxyRenderer.ResetLists();
			if (systemRenderer != null) systemRenderer.ResetLists();
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
			using(var form = new FormShipDesignList(this.currentPlayer))
				form.ShowDialog();
		}
		
		private void developmentToolStripMenuItem_Click(object sender, EventArgs e)
		{
			postDelayedEvent(showDevelopment);
		}
		
		private void researchToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using(var form = new FormResearch(this.currentPlayer))
				form.ShowDialog();
		}
		
		private void colonizationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using(var form = new FormColonization(this.currentPlayer))
				form.ShowDialog();
		}
		
		private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using(var form = new FormReports(this.currentPlayer.Reports))
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
			using(var form = new FormDevelopment(this.currentPlayer))
				form.ShowDialog();
		}
		
		private void showMainMenu()
		{
			using (var form = new FormMainMenu(this.gameController))
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
			using (var form = new FormNewGame()) {
				form.Initialize();
				if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
					this.gameController.Stop();	
					form.CreateGame(gameController);
					this.gameController.Start(this);
					this.initPlayers();
					this.restartRenderers();
				}
				else
					postDelayedEvent(showMainMenu);
			}
		}

		private void showResearch()
		{
			using(var form = new FormResearch(this.currentPlayer))
				form.ShowDialog();
		}
		
		private void showSaveGame()
		{
			var saveController = new SavesController(gameController);
			
			using(var form = new FormSaveLoad(saveController))
				if (form.ShowDialog() != DialogResult.OK)
					postDelayedEvent(showMainMenu);
				else if (form.Result == MainMenuResult.LoadGame) {
					this.gameController.Stop();
					saveController.Load(form.SelectedGameData);
					this.gameController.Start(this);
					this.initPlayers();
					this.restartRenderers();
				}
		}
		
		private void showSettings()
		{
			using (var form = new FormSettings())
				if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
					setLanguage();
			postDelayedEvent(showMainMenu);
		}
		#endregion

		private void initPlayers()
		{
			this.playerControllers = this.gameController.LocalHumanPlayers().ToArray();
			this.currentPlayerIndex = 0;
		}
		
		private void restartRenderers()
		{
			if (this.galaxyRenderer != null) {
				this.galaxyRenderer.DetachFromCanvas();
				this.galaxyRenderer.Unload();
			}
			
			if (this.systemRenderer != null) {
				this.systemRenderer.DetachFromCanvas();
				this.systemRenderer.Unload();
			}
			
			this.galaxyRenderer = new GalaxyRenderer(this);
			this.galaxyRenderer.CurrentPlayer = this.currentPlayer;
			this.galaxyRenderer.Load();
			
			this.systemRenderer = new SystemRenderer(switchToGalaxyView, constructionManagement, empyPlanetView);
			this.combatRenderer = new SpaceCombatRenderer();
			
			switchToGalaxyView();
		}
		
		private void shipGroupItem_SelectedIndexChanged(object sender, EventArgs e)
		{
			var groupItem = sender as ShipGroupItem;
			if (groupItem.IsSelected)
				this.fleetController.SelectGroup(groupItem.Data, groupItem.SelectedQuantity);
			else
				this.fleetController.DeselectGroup(groupItem.Data);
		}
		
		private void shipGroupItem_SplitRequested(object sender, EventArgs e)
		{
			var groupItem = sender as ShipGroupItem;
			using(var form = new FormSelectQuantity(groupItem.Data.Quantity, 1)) {
				form.ShowDialog();
				groupItem.PartialSelect(form.SelectedValue);
				groupItem.IsSelected = form.SelectedValue > 0;
			}
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
			empyPlanetView.Visible = false;
			endTurnButton.Visible = true;
			returnButton.Visible = false;
		}
		
		#endregion
		
		
		#region IGameStateListener implementation
		public void OnNewTurn()
		{
			if (this.InvokeRequired) {
				postDelayedEvent(this.OnNewTurn);
				return;
			}
			
			this.currentPlayerIndex = 0;
			this.galaxyRenderer.CurrentPlayer = this.currentPlayer;
			
			if (this.currentRenderer == this.combatRenderer)
			{
				this.currentRenderer.DetachFromCanvas();
				
				this.currentRenderer = this.galaxyRenderer;
				this.currentRenderer.AttachToCanvas(this.glCanvas);
				
				endTurnButton.Visible = true;
				menuStrip.Visible = true;
			}
			
			if (galaxyRenderer != null) galaxyRenderer.OnNewTurn();
			if (systemRenderer != null) systemRenderer.OnNewTurn();
		}
		
		public IBattleEventListener OnDoCombat(SpaceBattleController battleController)
		{
			if (this.InvokeRequired)
				this.Invoke(new Action<SpaceBattleController>(initCombatGui), battleController);
			
			return this;
		}
		
		private void initCombatGui(SpaceBattleController battleController)
		{
			this.conflictController = battleController;
			
			this.fleetController = null;
			this.currentRenderer.DetachFromCanvas();
			
			this.combatRenderer.StartCombat(battleController);
			this.currentRenderer = this.combatRenderer;
			this.currentRenderer.AttachToCanvas(this.glCanvas);
			
			constructionManagement.Visible = false;
			empyPlanetView.Visible = false;
			fleetPanel.Visible = false;
			endTurnButton.Visible = false;
			returnButton.Visible = false;
			menuStrip.Visible = false;
		}
		#endregion
		
		#region IBattleEventListener implementation
		public void PlayUnit(CombatantInfo unitInfo)
		{
			//TODO(v0.5) render selection
			this.combatRenderer.Controller.UnitDone();
		}
		#endregion
		
		#region IGalaxyViewListener
		void IGalaxyViewListener.FleetDeselected() 
		{
			this.fleetController = null;
		}
		
		void IGalaxyViewListener.FleetSelected(FleetController fleetController)
		{
			this.fleetController = fleetController;
			
			this.shipList.SuspendLayout();
			foreach (var control in this.shipList.Controls) {
				(control as ShipGroupItem).SelectionChanged -= shipGroupItem_SelectedIndexChanged;
				(control as ShipGroupItem).SplitRequested -= shipGroupItem_SplitRequested;
			}
			this.shipList.Controls.Clear();
			
			foreach (var fleet in fleetController.ShipGroups) {
				var fleetView = new ShipGroupItem();
				fleetView.SetData(fleet);
				fleetView.SelectionChanged += shipGroupItem_SelectedIndexChanged;
				fleetView.SplitRequested += shipGroupItem_SplitRequested;
				this.shipList.Controls.Add(fleetView);
				fleetView.IsSelected = true;
			}
			
			this.shipList.ResumeLayout();
			
			this.constructionManagement.Visible = false;
			empyPlanetView.Visible = false;
			this.fleetPanel.Visible = true;
		}
		
		void IGalaxyViewListener.SystemOpened(StarSystemController systemController)
		{
			this.fleetController = null;
			galaxyRenderer.DetachFromCanvas();
			
			systemRenderer.AttachToCanvas(glCanvas);
			systemRenderer.SetStarSystem(systemController, this.currentPlayer);
			currentRenderer = systemRenderer;
			
			constructionManagement.Visible = true;
			endTurnButton.Visible = false;
			returnButton.Visible = true;
			
			this.fleetController = null;
			this.fleetPanel.Visible = false;
		}
		
		void IGalaxyViewListener.SystemSelected(StarSystemController systemController)
		{
			//TODO(v0.5)
			this.constructionManagement.Visible = false;
			empyPlanetView.Visible = false;
			
			this.fleetController = null;
			this.fleetPanel.Visible = false;
		}
		#endregion
		
	}
}
