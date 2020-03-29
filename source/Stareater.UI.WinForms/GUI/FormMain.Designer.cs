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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.glCanvas = new OpenTK.GLControl(OpenTK.Graphics.GraphicsMode.Default, 3, 2, OpenTK.Graphics.GraphicsContextFlags.Default);
			this.workaroundForWinformsAnchorBug = new System.Windows.Forms.Panel();
			this.abilityList = new System.Windows.Forms.FlowLayoutPanel();
			this.workaroundForWinformsAnchorBug.SuspendLayout();
			this.SuspendLayout();
			// 
			// glCanvas
			// 
			this.glCanvas.BackColor = System.Drawing.Color.Black;
			this.glCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
			this.glCanvas.Location = new System.Drawing.Point(0, 0);
			this.glCanvas.Name = "glCanvas";
			this.glCanvas.Size = new System.Drawing.Size(884, 562);
			this.glCanvas.TabIndex = 0;
			this.glCanvas.VSync = false;
			this.glCanvas.Load += new System.EventHandler(this.glCanvas_Load);
			this.glCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.glCanvas_Paint);
			this.glCanvas.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.glCanvas_KeyPress);
			this.glCanvas.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.glCanvas_MouseDoubleClick);
			this.glCanvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glCanvas_MouseDown);
			this.glCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glCanvas_MouseMove);
			this.glCanvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glCanvas_MouseUp);
			this.glCanvas.Resize += new System.EventHandler(this.glCanvas_Resize);
			// 
			// workaroundForWinformsAnchorBug
			// 
			this.workaroundForWinformsAnchorBug.Controls.Add(this.abilityList);
			this.workaroundForWinformsAnchorBug.Controls.Add(this.glCanvas);
			this.workaroundForWinformsAnchorBug.Dock = System.Windows.Forms.DockStyle.Fill;
			this.workaroundForWinformsAnchorBug.Location = new System.Drawing.Point(0, 0);
			this.workaroundForWinformsAnchorBug.Name = "workaroundForWinformsAnchorBug";
			this.workaroundForWinformsAnchorBug.Size = new System.Drawing.Size(884, 562);
			this.workaroundForWinformsAnchorBug.TabIndex = 9;
			// 
			// abilityList
			// 
			this.abilityList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.abilityList.AutoScroll = true;
			this.abilityList.BackColor = System.Drawing.Color.Black;
			this.abilityList.Location = new System.Drawing.Point(12, 50);
			this.abilityList.Name = "abilityList";
			this.abilityList.Size = new System.Drawing.Size(110, 475);
			this.abilityList.TabIndex = 8;
			this.abilityList.Visible = false;
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(884, 562);
			this.Controls.Add(this.workaroundForWinformsAnchorBug);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormMain";
			this.Text = "Stareater";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.workaroundForWinformsAnchorBug.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private OpenTK.GLControl glCanvas;
		private System.Windows.Forms.Panel workaroundForWinformsAnchorBug;
		private System.Windows.Forms.FlowLayoutPanel abilityList;
	}
}