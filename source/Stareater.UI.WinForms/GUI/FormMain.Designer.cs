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
			this.glCanvas.Size = new System.Drawing.Size(514, 262);
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
			this.constructionManagement.Location = new System.Drawing.Point(78, 146);
			this.constructionManagement.Name = "constructionManagement";
			this.constructionManagement.Size = new System.Drawing.Size(358, 116);
			this.constructionManagement.TabIndex = 1;
			this.constructionManagement.Visible = false;
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(514, 262);
			this.Controls.Add(this.constructionManagement);
			this.Controls.Add(this.glCanvas);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormMain";
			this.Text = "Stareater";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
			this.ResumeLayout(false);
		}
		private Stareater.GUI.ConstructionSiteView constructionManagement;

		#endregion

		private System.Windows.Forms.Timer eventTimer;
		private OpenTK.GLControl glCanvas;
		private System.Windows.Forms.Timer glRedrawTimer;
	}
}