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
			this.developmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.endTurnButton = new System.Windows.Forms.Button();
			this.menuStrip.SuspendLayout();
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
									this.developmentToolStripMenuItem});
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
			// developmentToolStripMenuItem
			// 
			this.developmentToolStripMenuItem.Name = "developmentToolStripMenuItem";
			this.developmentToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
			this.developmentToolStripMenuItem.Text = "Development";
			this.developmentToolStripMenuItem.Click += new System.EventHandler(this.developmentToolStripMenuItem_Click);
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
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(884, 562);
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
			this.ResumeLayout(false);
			this.PerformLayout();
		}
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