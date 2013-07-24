namespace Winforms_Mockups
{
	partial class FormStarSystem
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
			this.managementPanel = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
			this.button1 = new System.Windows.Forms.Button();
			this.systemMockup1 = new Winforms_Mockups.SystemMockup();
			this.managementPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// managementPanel
			// 
			this.managementPanel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.managementPanel.BackColor = System.Drawing.SystemColors.Control;
			this.managementPanel.BackgroundImage = global::Winforms_Mockups.Properties.Resources.metalic;
			this.managementPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.managementPanel.Controls.Add(this.label1);
			this.managementPanel.Controls.Add(this.hScrollBar1);
			this.managementPanel.Controls.Add(this.button1);
			this.managementPanel.Location = new System.Drawing.Point(126, 296);
			this.managementPanel.Name = "managementPanel";
			this.managementPanel.Size = new System.Drawing.Size(316, 105);
			this.managementPanel.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(87, 30);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(39, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "7 turns";
			// 
			// hScrollBar1
			// 
			this.hScrollBar1.Location = new System.Drawing.Point(90, 13);
			this.hScrollBar1.Name = "hScrollBar1";
			this.hScrollBar1.Size = new System.Drawing.Size(206, 17);
			this.hScrollBar1.TabIndex = 1;
			this.hScrollBar1.Value = 75;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.Image = global::Winforms_Mockups.Properties.Resources.small_cruiser;
			this.button1.Location = new System.Drawing.Point(8, 11);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(80, 80);
			this.button1.TabIndex = 0;
			this.button1.UseVisualStyleBackColor = true;
			// 
			// systemMockup1
			// 
			this.systemMockup1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.systemMockup1.Location = new System.Drawing.Point(0, 0);
			this.systemMockup1.Name = "systemMockup1";
			this.systemMockup1.Size = new System.Drawing.Size(568, 401);
			this.systemMockup1.TabIndex = 2;
			// 
			// FormStarSystem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(568, 401);
			this.Controls.Add(this.managementPanel);
			this.Controls.Add(this.systemMockup1);
			this.Name = "FormStarSystem";
			this.Text = "FormStarSystem";
			this.managementPanel.ResumeLayout(false);
			this.managementPanel.PerformLayout();
			this.ResumeLayout(false);
		}
		private Winforms_Mockups.SystemMockup systemMockup1;

		#endregion

		private System.Windows.Forms.Panel managementPanel;
		private System.Windows.Forms.HScrollBar hScrollBar1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;

	}
}