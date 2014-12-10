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
			this.glRedrawTimer = new System.Windows.Forms.Timer(this.components);
			this.constructionManagement = new Stareater.GUI.ConstructionSiteView();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.mainMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.designsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.developmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.researchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.endTurnButton = new System.Windows.Forms.Button();
			this.returnButton = new System.Windows.Forms.Button();
			this.fleetPanel = new System.Windows.Forms.Panel();
			this.fleetMissionButton = new System.Windows.Forms.Button();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.menuStrip.SuspendLayout();
			this.fleetPanel.SuspendLayout();
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
			this.glCanvas.Location = new System.Drawing.Point(0, 24);
			this.glCanvas.Name = "glCanvas";
			this.glCanvas.Size = new System.Drawing.Size(884, 538);
			this.glCanvas.TabIndex = 0;
			this.glCanvas.VSync = false;
			this.glCanvas.Load += new System.EventHandler(this.glCanvas_Load);
			this.glCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.glCanvas_Paint);
			this.glCanvas.Resize += new System.EventHandler(this.glCanvas_Resize);
			// 
			// glRedrawTimer
			// 
			this.glRedrawTimer.Enabled = true;
			this.glRedrawTimer.Interval = 10;
			this.glRedrawTimer.Tick += new System.EventHandler(this.glRedrawTimer_Tick);
			// 
			// constructionManagement
			// 
			this.constructionManagement.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.constructionManagement.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.constructionManagement.Location = new System.Drawing.Point(263, 446);
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
									this.reportsToolStripMenuItem});
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
			// reportsToolStripMenuItem
			// 
			this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
			this.reportsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
			this.reportsToolStripMenuItem.Text = "Reports";
			this.reportsToolStripMenuItem.Click += new System.EventHandler(this.reportsToolStripMenuItem_Click);
			// 
			// endTurnButton
			// 
			this.endTurnButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.endTurnButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.endTurnButton.Location = new System.Drawing.Point(792, 470);
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
			this.returnButton.Location = new System.Drawing.Point(792, 27);
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
			this.fleetPanel.Controls.Add(this.flowLayoutPanel1);
			this.fleetPanel.Controls.Add(this.fleetMissionButton);
			this.fleetPanel.Location = new System.Drawing.Point(234, 446);
			this.fleetPanel.Name = "fleetPanel";
			this.fleetPanel.Size = new System.Drawing.Size(417, 116);
			this.fleetPanel.TabIndex = 5;
			this.fleetPanel.Visible = false;
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
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoScroll = true;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(332, 109);
			this.flowLayoutPanel1.TabIndex = 1;
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(884, 562);
			this.Controls.Add(this.fleetPanel);
			this.Controls.Add(this.returnButton);
			this.Controls.Add(this.endTurnButton);
			this.Controls.Add(this.constructionManagement);
			this.Controls.Add(this.glCanvas);
			this.Controls.Add(this.menuStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip;
			this.Name = "FormMain";
			this.Text = "Stareater";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.fleetPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button fleetMissionButton;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
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
		private System.Windows.Forms.Timer glRedrawTimer;
	}
}