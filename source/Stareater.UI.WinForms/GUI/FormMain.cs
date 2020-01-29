using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Combat;
using Stareater.Controllers.Views.Ships;
using Stareater.Localization;
using Stareater.Utils.Collections;
using Stareater.Utils.NumberFormatters;
using Stareater.GLData;
using Stareater.GameScenes;
using Stareater.GraphicsEngine;

namespace Stareater.GUI
{
	internal partial class FormMain : Form, IGameStateListener, IBattleEventListener, IBombardEventListener, IGalaxyViewListener
	{
		private const float MaxDeltaTime = 0.5f;
		
		private readonly TimingLoop timingLoop;
		private AScene currentRenderer = null;
		private AScene nextRenderer = null;
		private BombardmentScene bombardRenderer;
		private GalaxyScene galaxyRenderer;
		private StarSystemScene systemRenderer;
		private SpaceCombatScene combatRenderer;
		private GameOverScene gameOverRenderer;
		private double animationDeltaTime = 0;

		private readonly GameController gameController = null;
		private PlayerController[] playerControllers = null;
		private SpaceBattleController conflictController = null;
		private BombardmentController bombardmentController = null;
		private int currentPlayerIndex = 0;
		
		public FormMain()
		{
			InitializeComponent();
			
			this.glCanvas.MouseWheel += glCanvas_MouseScroll;
			this.gameController = new GameController();
			this.timingLoop = new TimingLoop(this, onNextFrame);
		}
		
		private void FormMain_Load(object sender, EventArgs e)
		{
			AssetController.Get.AddLoader(LoadingMethods.InitializeLocalization, this.languageReady);
			AssetController.Get.AddLoader(LoadingMethods.LoadOrganizations);
			AssetController.Get.AddLoader(LoadingMethods.LoadPlayerColors);
			AssetController.Get.AddLoader(LoadingMethods.LoadAis);
			AssetController.Get.AddLoader(LoadingMethods.LoadStartConditions);
			AssetController.Get.AddLoader(LoadingMethods.LoadStarPositioners);
			AssetController.Get.AddLoader(LoadingMethods.LoadStarConnectors);
			AssetController.Get.AddLoader(LoadingMethods.LoadStarPopulators);
		}
		
		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			SettingsWinforms.Get.Save();
			this.timingLoop.Stop();
		}

		private void languageReady()
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new Action(this.languageReady));
				return;
			}
			
			applySettings();
			this.nextRenderer = new IntroScene(
				() => postDelayedEvent(showNewGame),
				() => postDelayedEvent(showSaveGame),
				() => postDelayedEvent(showSettings),
				() => Close()
			);
		}

		private void applySettings()
		{
			this.Font = SettingsWinforms.Get.FormFont;

			var context = LocalizationManifest.Get.CurrentLanguage["FormMain"];
			this.returnButton.Text = context["Return"].Text();
			
			this.glCanvas.VSync = SettingsWinforms.Get.VSync;
			this.timingLoop.OnSettingsChange();
		}
		
		private void onNextFrame(double dt)
		{
			this.animationDeltaTime = Math.Min(dt, MaxDeltaTime);
			glCanvas.Invalidate();
		}
		
		private PlayerController currentPlayer
		{
			get { return this.playerControllers[currentPlayerIndex]; }
		}

		private void returnButton_Click(object sender, EventArgs e)
		{
			if (this.currentRenderer == systemRenderer)
				switchToGalaxyView();
			else if (this.currentRenderer == bombardRenderer)
				this.bombardmentController.Leave();
		}

		#region Delayed Events
		private void postDelayedEvent(Action eventAction)
		{
			this.BeginInvoke(new Action(eventAction));
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
			using (var form = new FormNewGame())
			{
				form.Initialize();
				if (NewGameController.CanCreateGame && form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
				{
					this.gameController.Stop();
					form.CreateGame(gameController);
					this.gameController.Start(this);
					this.initPlayers();
					this.restartRenderers(); //TODO(v0.8) can cause race condition where no stellarises has been initialized yet
				}
			}
		}

		private void showSaveGame()
		{
			using (var form = new FormSaveLoad(gameController))
				if (form.ShowDialog() == DialogResult.OK && form.Result == MainMenuResult.LoadGame)
				{
					this.gameController.Stop();
					var saveController = new SavesController(gameController, SettingsWinforms.Get.FileStorageRootPath);
					saveController.Load(form.SelectedGameData, LoadingMethods.GameDataSources());
					this.gameController.Start(this);
					this.initPlayers();
					this.restartRenderers(); //TODO(v0.8) render thread my try to draw old map before new one is available
				}
		}
		
		private void showSettings()
		{
			using (var form = new FormSettings(GL.GetString(StringName.Renderer)))
				if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
					applySettings();
		}
		#endregion

		private void initPlayers()
		{
			this.playerControllers = this.gameController.LocalHumanPlayers().ToArray();
			this.currentPlayerIndex = 0;
		}

		private void restartRenderers()
		{
			if (this.galaxyRenderer != null)
				this.galaxyRenderer.Deactivate();

			this.galaxyRenderer = new GalaxyScene(this, () => postDelayedEvent(showMainMenu));
			this.galaxyRenderer.SwitchPlayer(this.currentPlayer);
			
			this.bombardRenderer = new BombardmentScene();
			this.systemRenderer = new StarSystemScene(switchToGalaxyView);
			this.combatRenderer = new SpaceCombatScene();
			this.gameOverRenderer = new GameOverScene();
			
			switchToGalaxyView();
		}

		private void unitDoneAction_Click(object sender, EventArgs e)
		{
			this.combatRenderer.OnUnitDone();
		}
		
		private void selectAbility_Click(object sender, EventArgs e)
		{
			this.combatRenderer.SelectedAbility = (sender as Control).Tag as AbilityInfo;
		}

		#region Canvas events

		private void glCanvas_Load(object sender, EventArgs e)
		{
			ShaderLibrary.Load();
			GalaxyTextures.Get.Load(); //TODO(v0.8) make general initialization logic for rendering
			TextRenderUtil.Get.Initialize();
			
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			
			this.timingLoop.Start();
		}

		private void glCanvas_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (this.currentRenderer != null)
				this.currentRenderer.HandleKeyPress(e);
		}

		private void glCanvas_MouseDown(object sender, MouseEventArgs e)
		{
			if (this.currentRenderer != null)
				this.currentRenderer.HandleMouseDown(e);
		}

		private void glCanvas_MouseUp(object sender, MouseEventArgs e)
		{
			if (this.currentRenderer != null)
				this.currentRenderer.HandleMouseUp(e, ModifierKeys & Keys.Modifiers);
		}

		private void glCanvas_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (this.currentRenderer != null)
				this.currentRenderer.HandleMouseDoubleClick(e);
		}
		
		private void glCanvas_MouseMove(object sender, MouseEventArgs e)
		{
			if (this.currentRenderer != null)
				this.currentRenderer.HandleMouseMove(e, ModifierKeys & Keys.Modifiers);
		}
		
		private void glCanvas_MouseScroll(object sender, MouseEventArgs e)
		{
			if (this.currentRenderer != null)
				this.currentRenderer.HandleMouseScroll(e);
		}
		
		private void glCanvas_Paint(object sender, PaintEventArgs e)
		{
			if (this.currentRenderer != this.nextRenderer)
			{
				if (this.currentRenderer != null)
					this.currentRenderer.Deactivate();

				this.currentRenderer = this.nextRenderer;
				this.currentRenderer.ResetProjection(this.glCanvas);
				this.currentRenderer.Activate();
			}
			
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

#if DEBUG
			try {
#endif
				if (currentRenderer != null)
					currentRenderer.Draw(this.animationDeltaTime);
#if DEBUG
			} catch(Exception ex)
			{
				System.Diagnostics.Trace.WriteLine("Canvas paint exception:");
				System.Diagnostics.Trace.TraceError(ex.ToString());
			}
#endif
			glCanvas.SwapBuffers();
			this.timingLoop.Continue();
		}
		
		private void glCanvas_Resize(object sender, EventArgs e)
		{
			if (this.currentRenderer != null)
				this.currentRenderer.ResetProjection(this.glCanvas);
		}
		
		#endregion

		#region Renderer events
		
		private void switchToGalaxyView()
		{
			this.nextRenderer = galaxyRenderer;
		}
		
		#endregion
		
		
		#region IGameStateListener implementation
		public void OnDoAudience(AudienceController controller)
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new Action<AudienceController>(OnDoAudience), controller);
				return;
			}
			
			using(var form = new FormAudience(controller))
				form.ShowDialog();
		}

		public IBattleEventListener OnDoCombat(SpaceBattleController battleController)
		{
			this.conflictController = battleController;

			return this;
		}
		
		public IBombardEventListener OnDoBombardment(BombardmentController bombardController)
		{
			if (this.InvokeRequired)
				this.BeginInvoke(new Action<BombardmentController>(initBombardGui), bombardController);
			else
				initBombardGui(bombardController);
			
			return this;
		}

		public void OnGameOver()
		{
			if (this.InvokeRequired) {
				postDelayedEvent(this.OnGameOver);
				return;
			}

			this.gameOverRenderer.SetResults(this.gameController.Results);
			this.nextRenderer = this.gameOverRenderer;
			
			this.abilityList.Visible = false;
			this.returnButton.Visible = false;
			this.unitInfoPanel.Visible = false;
		}

		public void OnNewTurn()
		{
			if (this.InvokeRequired) {
				postDelayedEvent(this.OnNewTurn);
				return;
			}
			
			this.currentPlayerIndex = 0;
			this.galaxyRenderer.SwitchPlayer(this.currentPlayer);
			
			if (this.currentRenderer == this.combatRenderer || this.currentRenderer == this.bombardRenderer)
			{
				this.nextRenderer = this.galaxyRenderer;
				
				abilityList.Visible = false;
				returnButton.Visible = false;
				unitInfoPanel.Visible = false;
			}
			
			if (galaxyRenderer != null) galaxyRenderer.OnNewTurn();
			if (systemRenderer != null) systemRenderer.OnNewTurn();
		}

		private void initCombatGui()
		{
			this.combatRenderer.StartCombat(this.conflictController);
			this.nextRenderer = this.combatRenderer;

			abilityList.Visible = true;
			returnButton.Visible = false;
			unitInfoPanel.Visible = true;
		}
		
		private void initBombardGui(BombardmentController bombardController)
		{
			this.bombardmentController = bombardController;
			
			this.bombardRenderer.StartBombardment(bombardmentController);
			this.nextRenderer = this.bombardRenderer;
			
			abilityList.Visible = false;
			returnButton.Visible = true;
			unitInfoPanel.Visible = false;
		}
		#endregion
		
		#region IBattleEventListener implementation
		void IBattleEventListener.OnStart()
		{
			if (this.InvokeRequired)
				this.Invoke(new Action(initCombatGui));
			else
				initCombatGui();
	}
		public void PlayUnit(CombatantInfo unitInfo)
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new Action<CombatantInfo>(PlayUnit), unitInfo);
				return;
			}
			
			this.combatRenderer.OnUnitTurn(unitInfo);
			
			var context = LocalizationManifest.Get.CurrentLanguage["FormMain"];
			var formatter = new ThousandsFormatter();
			var decimalFormat = new DecimalsFormatter(0, 0);
			
			Func<string, double, double, string> hpText = (label, x, max) => 
			{
				var hpFormat = ThousandsFormatter.MaxMagnitudeFormat(x, max);
				return context[label].Text() + ": " + hpFormat.Format(x) + " / " + hpFormat.Format(max);
			};
				
			shipCount.Text = context["ShipCount"].Text() + ": " + formatter.Format(unitInfo.Count);
			armorInfo.Text = hpText("ArmorLabel", unitInfo.ArmorHp, unitInfo.ArmorHpMax);
			shieldInfo.Text = hpText("ShieldLabel", unitInfo.ShieldHp, unitInfo.ShieldHpMax);
			
			if (unitInfo.MovementEta > 0)
				movementInfo.Text = context["MovementEta"].Text(
					new Var("eta", unitInfo.MovementEta).Get, 
					new TextVar("eta", unitInfo.MovementEta.ToString()).Get
				);
			else
				movementInfo.Text = context["MovementPoints"].Text() + " (" + decimalFormat.Format(unitInfo.MovementPoints * 100) + " %)"; 
			
			this.abilityList.Controls.Clear();
			Func<Image, string, object, Button> buttonMaker = (image, text, tag) =>
			{
				var button = new Button
				{
					Image = image,
					ImageAlign = ContentAlignment.MiddleLeft,
					Margin = new Padding(3, 3, 3, 0),
					Size = new Size(80, 32),
					Text = text,
					TextImageRelation = TextImageRelation.ImageBeforeText,
					UseVisualStyleBackColor = true,
					Tag = tag
				};
				button.Click += selectAbility_Click;
				
				return button;
			};
			
			this.abilityList.Controls.Add(buttonMaker(
					null,
					context["MoveAction"].Text(),
					null
				));
			
			foreach(var ability in unitInfo.Abilities)
				this.abilityList.Controls.Add(buttonMaker(
					ImageCache.Get.Resized(ability.ImagePath, new Size(24, 24)),
					"x " + formatter.Format(ability.Quantity),
					ability
				));
		}
		#endregion
		
		#region IBombardEventListener implementation

		public void BombardTurn()
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new Action(BombardTurn));
				return;
			}
			
			this.bombardRenderer.NewTurn();
		}

		#endregion

		#region IGalaxyViewListener
		void IGalaxyViewListener.TurnEnded()
		{
			FormSaveLoad.Autosave(this.gameController);
			this.currentPlayer.EndGalaxyPhase();

			if (this.currentPlayerIndex < this.playerControllers.Length - 1)
			{
				this.currentPlayerIndex++;
				this.galaxyRenderer.SwitchPlayer(this.currentPlayer);
			}

			if (galaxyRenderer != null) galaxyRenderer.ResetLists();
			if (systemRenderer != null) systemRenderer.ResetLists();
		}

		void IGalaxyViewListener.SystemOpened(StarSystemController systemController)
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new Action<StarSystemController>(((IGalaxyViewListener)this).SystemOpened), systemController);
				return;
			}
			
			this.systemRenderer.SetStarSystem(systemController, this.currentPlayer);
			this.nextRenderer = systemRenderer;
		}
		#endregion
	}
}
