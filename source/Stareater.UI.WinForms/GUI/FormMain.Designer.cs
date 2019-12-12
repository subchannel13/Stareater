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
			this.returnButton = new System.Windows.Forms.Button();
			this.unitInfoPanel = new System.Windows.Forms.Panel();
			this.movementInfo = new System.Windows.Forms.Label();
			this.shieldInfo = new System.Windows.Forms.Label();
			this.armorInfo = new System.Windows.Forms.Label();
			this.shipCount = new System.Windows.Forms.Label();
			this.unitDoneAction = new System.Windows.Forms.Button();
			this.workaroundForWinformsAnchorBug = new System.Windows.Forms.Panel();
			this.abilityList = new System.Windows.Forms.FlowLayoutPanel();
			this.unitInfoPanel.SuspendLayout();
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
			// unitInfoPanel
			// 
			this.unitInfoPanel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.unitInfoPanel.Controls.Add(this.movementInfo);
			this.unitInfoPanel.Controls.Add(this.shieldInfo);
			this.unitInfoPanel.Controls.Add(this.armorInfo);
			this.unitInfoPanel.Controls.Add(this.shipCount);
			this.unitInfoPanel.Controls.Add(this.unitDoneAction);
			this.unitInfoPanel.Location = new System.Drawing.Point(261, 513);
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
			// workaroundForWinformsAnchorBug
			// 
			this.workaroundForWinformsAnchorBug.Controls.Add(this.abilityList);
			this.workaroundForWinformsAnchorBug.Controls.Add(this.unitInfoPanel);
			this.workaroundForWinformsAnchorBug.Controls.Add(this.returnButton);
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
			this.unitInfoPanel.ResumeLayout(false);
			this.unitInfoPanel.PerformLayout();
			this.workaroundForWinformsAnchorBug.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		private System.Windows.Forms.Button returnButton;

		#endregion
		private OpenTK.GLControl glCanvas;
		private System.Windows.Forms.Panel unitInfoPanel;
		private System.Windows.Forms.Label movementInfo;
		private System.Windows.Forms.Label shieldInfo;
		private System.Windows.Forms.Label armorInfo;
		private System.Windows.Forms.Label shipCount;
		private System.Windows.Forms.Button unitDoneAction;
		private System.Windows.Forms.Panel workaroundForWinformsAnchorBug;
		private System.Windows.Forms.FlowLayoutPanel abilityList;
	}
}