namespace Stareater.GUI
{
	partial class FormMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.eventTimer = new System.Windows.Forms.Timer(this.components);
			this.glCanvas = new OpenTK.GLControl();
			this.constructionManagement = new Stareater.GUI.ConstructionSiteView();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.mainMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.designsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.developmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.researchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.colonizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.libraryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.endTurnButton = new System.Windows.Forms.Button();
			this.returnButton = new System.Windows.Forms.Button();
			this.fleetPanel = new System.Windows.Forms.Panel();
			this.shipList = new System.Windows.Forms.FlowLayoutPanel();
			this.fleetMissionButton = new System.Windows.Forms.Button();
			this.empyPlanetView = new Stareater.GUI.EmpyPlanetView();
			this.unitInfoPanel = new System.Windows.Forms.Panel();
			this.movementInfo = new System.Windows.Forms.Label();
			this.shieldInfo = new System.Windows.Forms.Label();
			this.armorInfo = new System.Windows.Forms.Label();
			this.shipCount = new System.Windows.Forms.Label();
			this.unitDoneAction = new System.Windows.Forms.Button();
			this.abilityList = new System.Windows.Forms.FlowLayoutPanel();
			this.workaroundForWinformsAnchorBug = new System.Windows.Forms.Panel();
			this.menuStrip.SuspendLayout();
			this.fleetPanel.SuspendLayout();
			this.unitInfoPanel.SuspendLayout();
			this.workaroundForWinformsAnchorBug.SuspendLayout();
			this.SuspendLayout();
			// 
			// eventTimer
			// 
			this.eventTimer.Interval = 1;
			this.eventTimer.Tick += new System.EventHandler(this.eventTimer_Tick);
			// 
			// glCanvas
			// 
			this.glCanvas.BackColor = System.Drawing.Color.Black;
			this.glCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
			this.glCanvas.Location = new System.Drawing.Point(0, 0);
			this.glCanvas.Name = "glCanvas";
			this.glCanvas.Size = new System.Drawing.Size(884, 538);
			this.glCanvas.TabIndex = 0;
			this.glCanvas.VSync = false;
			this.glCanvas.Load += new System.EventHandler(this.glCanvas_Load);
			this.glCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.glCanvas_Paint);
			this.glCanvas.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.glCanvas_KeyPress);
			this.glCanvas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.glCanvas_MouseClick);
			this.glCanvas.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.glCanvas_MouseDoubleClick);
			this.glCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glCanvas_MouseMove);
			this.glCanvas.Resize += new System.EventHandler(this.glCanvas_Resize);
			// 
			// constructionManagement
			// 
			this.constructionManagement.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.constructionManagement.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.constructionManagement.Location = new System.Drawing.Point(263, 422);
			this.constructionManagement.Name = "constructionManagement";
			this.constructionManagement.Size = new System.Drawing.Size(358, 116);
			this.constructionManagement.TabIndex = 1;
			this.constructionManagement.Visible = false;
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.mainMenuToolStripMenuItem,
			this.designsToolStripMenuItem,
			this.developmentToolStripMenuItem,
			this.researchToolStripMenuItem,
			this.colonizationToolStripMenuItem,
			this.reportsToolStripMenuItem,
			this.libraryToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(884, 24);
			this.menuStrip.TabIndex = 2;
			this.menuStrip.Text = "menuStrip1";
			// 
			// mainMenuToolStripMenuItem
			// 
			this.mainMenuToolStripMenuItem.Name = "mainMenuToolStripMenuItem";
			this.mainMenuToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
			this.mainMenuToolStripMenuItem.Text = "Main menu";
			this.mainMenuToolStripMenuItem.Click += new System.EventHandler(this.mainMenuToolStripMenuItem_Click);
			// 
			// designsToolStripMenuItem
			// 
			this.designsToolStripMenuItem.Name = "designsToolStripMenuItem";
			this.designsToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
			this.designsToolStripMenuItem.Text = "Designs";
			this.designsToolStripMenuItem.Click += new System.EventHandler(this.designsToolStripMenuItem_Click);
			// 
			// developmentToolStripMenuItem
			// 
			this.developmentToolStripMenuItem.Name = "developmentToolStripMenuItem";
			this.developmentToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
			this.developmentToolStripMenuItem.Text = "Development";
			this.developmentToolStripMenuItem.Click += new System.EventHandler(this.developmentToolStripMenuItem_Click);
			// 
			// researchToolStripMenuItem
			// 
			this.researchToolStripMenuItem.Name = "researchToolStripMenuItem";
			this.researchToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
			this.researchToolStripMenuItem.Text = "Research";
			this.researchToolStripMenuItem.Click += new System.EventHandler(this.researchToolStripMenuItem_Click);
			// 
			// colonizationToolStripMenuItem
			// 
			this.colonizationToolStripMenuItem.Name = "colonizationToolStripMenuItem";
			this.colonizationToolStripMenuItem.Size = new System.Drawing.Size(86, 20);
			this.colonizationToolStripMenuItem.Text = "Colonization";
			this.colonizationToolStripMenuItem.Click += new System.EventHandler(this.colonizationToolStripMenuItem_Click);
			// 
			// reportsToolStripMenuItem
			// 
			this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
			this.reportsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
			this.reportsToolStripMenuItem.Text = "Reports";
			this.reportsToolStripMenuItem.Click += new System.EventHandler(this.reportsToolStripMenuItem_Click);
			// 
			// libraryToolStripMenuItem
			// 
			this.libraryToolStripMenuItem.Name = "libraryToolStripMenuItem";
			this.libraryToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
			this.libraryToolStripMenuItem.Text = "Library";
			this.libraryToolStripMenuItem.Click += new System.EventHandler(this.libraryToolStripMenuItem_Click);
			// 
			// endTurnButton
			// 
			this.endTurnButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.endTurnButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.endTurnButton.Location = new System.Drawing.Point(792, 446);
			this.endTurnButton.Name = "endTurnButton";
			this.endTurnButton.Size = new System.Drawing.Size(80, 80);
			this.endTurnButton.TabIndex = 3;
			this.endTurnButton.Text = "button1";
			this.endTurnButton.UseVisualStyleBackColor = true;
			this.endTurnButton.Visible = false;
			this.endTurnButton.Click += new System.EventHandler(this.endTurnButton_Click);
			// 
			// returnButton
			// 
			this.returnButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.returnButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.returnButton.Location = new System.Drawing.Point(792, 10);
			this.returnButton.Name = "returnButton";
			this.returnButton.Size = new System.Drawing.Size(80, 80);
			this.returnButton.TabIndex = 4;
			this.returnButton.Text = "button2";
			this.returnButton.UseVisualStyleBackColor = true;
			this.returnButton.Visible = false;
			this.returnButton.Click += new System.EventHandler(this.returnButton_Click);
			// 
			// fleetPanel
			// 
			this.fleetPanel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.fleetPanel.Controls.Add(this.shipList);
			this.fleetPanel.Controls.Add(this.fleetMissionButton);
			this.fleetPanel.Location = new System.Drawing.Point(234, 422);
			this.fleetPanel.Name = "fleetPanel";
			this.fleetPanel.Size = new System.Drawing.Size(417, 116);
			this.fleetPanel.TabIndex = 5;
			this.fleetPanel.Visible = false;
			// 
			// shipList
			// 
			this.shipList.AutoScroll = true;
			this.shipList.Location = new System.Drawing.Point(3, 3);
			this.shipList.Name = "shipList";
			this.shipList.Size = new System.Drawing.Size(332, 109);
			this.shipList.TabIndex = 1;
			// 
			// fleetMissionButton
			// 
			this.fleetMissionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.fleetMissionButton.Location = new System.Drawing.Point(339, 3);
			this.fleetMissionButton.Name = "fleetMissionButton";
			this.fleetMissionButton.Size = new System.Drawing.Size(75, 23);
			this.fleetMissionButton.TabIndex = 0;
			this.fleetMissionButton.Text = "Missions";
			this.fleetMissionButton.UseVisualStyleBackColor = true;
			// 
			// empyPlanetView
			// 
			this.empyPlanetView.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.empyPlanetView.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.empyPlanetView.Location = new System.Drawing.Point(263, 422);
			this.empyPlanetView.Name = "empyPlanetView";
			this.empyPlanetView.Size = new System.Drawing.Size(358, 116);
			this.empyPlanetView.TabIndex = 6;
			this.empyPlanetView.Visible = false;
			// 
			// unitInfoPanel
			// 
			this.unitInfoPanel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.unitInfoPanel.Controls.Add(this.movementInfo);
			this.unitInfoPanel.Controls.Add(this.shieldInfo);
			this.unitInfoPanel.Controls.Add(this.armorInfo);
			this.unitInfoPanel.Controls.Add(this.shipCount);
			this.unitInfoPanel.Controls.Add(this.unitDoneAction);
			this.unitInfoPanel.Location = new System.Drawing.Point(261, 489);
			this.unitInfoPanel.Name = "unitInfoPanel";
			this.unitInfoPanel.Size = new System.Drawing.Size(362, 49);
			this.unitInfoPanel.TabIndex = 7;
			this.unitInfoPanel.Visible = false;
			// 
			// movementInfo
			// 
			this.movementInfo.AutoSize = true;
			this.movementInfo.Location = new System.Drawing.Point(6, 23);
			this.movementInfo.Name = "movementInfo";
			this.movementInfo.Size = new System.Drawing.Size(82, 13);
			this.movementInfo.TabIndex = 18;
			this.movementInfo.Text = "Move: in x turns";
			// 
			// shieldInfo
			// 
			this.shieldInfo.AutoSize = true;
			this.shieldInfo.Location = new System.Drawing.Point(231, 23);
			this.shieldInfo.Name = "shieldInfo";
			this.shieldInfo.Size = new System.Drawing.Size(119, 13);
			this.shieldInfo.TabIndex = 17;
			this.shieldInfo.Text = "Shield: xx.xx X / xx.xx X";
			// 
			// armorInfo
			// 
			this.armorInfo.AutoSize = true;
			this.armorInfo.Location = new System.Drawing.Point(231, 7);
			this.armorInfo.Name = "armorInfo";
			this.armorInfo.Size = new System.Drawing.Size(117, 13);
			this.armorInfo.TabIndex = 16;
			this.armorInfo.Text = "Armor: xx.xx X / xx.xx X";
			// 
			// shipCount
			// 
			this.shipCount.AutoSize = true;
			this.shipCount.Location = new System.Drawing.Point(6, 7);
			this.shipCount.Name = "shipCount";
			this.shipCount.Size = new System.Drawing.Size(72, 13);
			this.shipCount.TabIndex = 15;
			this.shipCount.Text = "Ships: xx.xx X";
			// 
			// unitDoneAction
			// 
			this.unitDoneAction.Location = new System.Drawing.Point(145, 4);
			this.unitDoneAction.Name = "unitDoneAction";
			this.unitDoneAction.Size = new System.Drawing.Size(80, 40);
			this.unitDoneAction.TabIndex = 14;
			this.unitDoneAction.Text = "Done";
			this.unitDoneAction.UseVisualStyleBackColor = true;
			this.unitDoneAction.Click += new System.EventHandler(this.unitDoneAction_Click);
			// 
			// abilityList
			// 
			this.abilityList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left)));
			this.abilityList.AutoScroll = true;
			this.abilityList.BackColor = System.Drawing.Color.Black;
			this.abilityList.Location = new System.Drawing.Point(12, 50);
			this.abilityList.Name = "abilityList";
			this.abilityList.Size = new System.Drawing.Size(110, 451);
			this.abilityList.TabIndex = 8;
			this.abilityList.Visible = false;
			// 
			// workaroundForWinformsAnchorBug
			// 
			this.workaroundForWinformsAnchorBug.Controls.Add(this.abilityList);
			this.workaroundForWinformsAnchorBug.Controls.Add(this.unitInfoPanel);
			this.workaroundForWinformsAnchorBug.Controls.Add(this.fleetPanel);
			this.workaroundForWinformsAnchorBug.Controls.Add(this.returnButton);
			this.workaroundForWinformsAnchorBug.Controls.Add(this.endTurnButton);
			this.workaroundForWinformsAnchorBug.Controls.Add(this.constructionManagement);
			this.workaroundForWinformsAnchorBug.Controls.Add(this.empyPlanetView);
			this.workaroundForWinformsAnchorBug.Controls.Add(this.glCanvas);
			this.workaroundForWinformsAnchorBug.Dock = System.Windows.Forms.DockStyle.Fill;
			this.workaroundForWinformsAnchorBug.Location = new System.Drawing.Point(0, 24);
			this.workaroundForWinformsAnchorBug.Name = "workaroundForWinformsAnchorBug";
			this.workaroundForWinformsAnchorBug.Size = new System.Drawing.Size(884, 538);
			this.workaroundForWinformsAnchorBug.TabIndex = 9;
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(884, 562);
			this.Controls.Add(this.workaroundForWinformsAnchorBug);
			this.Controls.Add(this.menuStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip;
			this.Name = "FormMain";
			this.Text = "Stareater";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.fleetPanel.ResumeLayout(false);
			this.unitInfoPanel.ResumeLayout(false);
			this.unitInfoPanel.PerformLayout();
			this.workaroundForWinformsAnchorBug.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private System.Windows.Forms.Button fleetMissionButton;
		private System.Windows.Forms.FlowLayoutPanel shipList;
		private System.Windows.Forms.Panel fleetPanel;
		private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem researchToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem designsToolStripMenuItem;
		private System.Windows.Forms.Button returnButton;
		private System.Windows.Forms.ToolStripMenuItem developmentToolStripMenuItem;
		private System.Windows.Forms.Button endTurnButton;
		private System.Windows.Forms.ToolStripMenuItem mainMenuToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip;
		private Stareater.GUI.ConstructionSiteView constructionManagement;

		#endregion

		private System.Windows.Forms.Timer eventTimer;
		private OpenTK.GLControl glCanvas;
		private Stareater.GUI.EmpyPlanetView empyPlanetView;
		private System.Windows.Forms.ToolStripMenuItem colonizationToolStripMenuItem;
		private System.Windows.Forms.Panel unitInfoPanel;
		private System.Windows.Forms.Label movementInfo;
		private System.Windows.Forms.Label shieldInfo;
		private System.Windows.Forms.Label armorInfo;
		private System.Windows.Forms.Label shipCount;
		private System.Windows.Forms.Button unitDoneAction;
		private System.Windows.Forms.FlowLayoutPanel abilityList;
		private System.Windows.Forms.ToolStripMenuItem libraryToolStripMenuItem;
		private System.Windows.Forms.Panel workaroundForWinformsAnchorBug;
	}
}