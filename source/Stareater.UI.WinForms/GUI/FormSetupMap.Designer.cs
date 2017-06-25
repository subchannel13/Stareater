namespace Stareater.GUI
{
	partial class FormSetupMap
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
			this.shapeSelector = new System.Windows.Forms.ComboBox();
			this.parametersPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.acceptButton = new System.Windows.Forms.Button();
			this.wormholeSelector = new System.Windows.Forms.ComboBox();
			this.populatorSelector = new System.Windows.Forms.ComboBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.mapPreview = new System.Windows.Forms.PictureBox();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mapPreview)).BeginInit();
			this.SuspendLayout();
			// 
			// shapeSelector
			// 
			this.shapeSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.shapeSelector.FormattingEnabled = true;
			this.shapeSelector.Location = new System.Drawing.Point(3, 3);
			this.shapeSelector.Name = "shapeSelector";
			this.shapeSelector.Size = new System.Drawing.Size(170, 21);
			this.shapeSelector.TabIndex = 0;
			this.shapeSelector.SelectedIndexChanged += new System.EventHandler(this.shapeSelector_SelectedIndexChanged);
			// 
			// parametersPanel
			// 
			this.parametersPanel.AutoScroll = true;
			this.parametersPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.parametersPanel.Location = new System.Drawing.Point(0, 81);
			this.parametersPanel.Name = "parametersPanel";
			this.parametersPanel.Size = new System.Drawing.Size(176, 252);
			this.parametersPanel.TabIndex = 2;
			// 
			// acceptButton
			// 
			this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.acceptButton.Location = new System.Drawing.Point(107, 351);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.Size = new System.Drawing.Size(75, 23);
			this.acceptButton.TabIndex = 3;
			this.acceptButton.Text = "button1";
			this.acceptButton.UseVisualStyleBackColor = true;
			this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
			// 
			// wormholeSelector
			// 
			this.wormholeSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.wormholeSelector.FormattingEnabled = true;
			this.wormholeSelector.Location = new System.Drawing.Point(3, 30);
			this.wormholeSelector.Name = "wormholeSelector";
			this.wormholeSelector.Size = new System.Drawing.Size(170, 21);
			this.wormholeSelector.TabIndex = 1;
			// 
			// populatorSelector
			// 
			this.populatorSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.populatorSelector.FormattingEnabled = true;
			this.populatorSelector.Location = new System.Drawing.Point(3, 57);
			this.populatorSelector.Name = "populatorSelector";
			this.populatorSelector.Size = new System.Drawing.Size(170, 21);
			this.populatorSelector.TabIndex = 2;
			// 
			// panel1
			// 
			this.panel1.AutoSize = true;
			this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel1.Controls.Add(this.shapeSelector);
			this.panel1.Controls.Add(this.populatorSelector);
			this.panel1.Controls.Add(this.wormholeSelector);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(176, 81);
			this.panel1.TabIndex = 1;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.parametersPanel);
			this.panel2.Controls.Add(this.panel1);
			this.panel2.Location = new System.Drawing.Point(12, 12);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(176, 333);
			this.panel2.TabIndex = 0;
			// 
			// mapPreview
			// 
			this.mapPreview.BackColor = System.Drawing.Color.Black;
			this.mapPreview.Location = new System.Drawing.Point(194, 12);
			this.mapPreview.Name = "mapPreview";
			this.mapPreview.Size = new System.Drawing.Size(333, 333);
			this.mapPreview.TabIndex = 42;
			this.mapPreview.TabStop = false;
			// 
			// FormSetupMap
			// 
			this.AcceptButton = this.acceptButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.acceptButton;
			this.ClientSize = new System.Drawing.Size(542, 382);
			this.Controls.Add(this.mapPreview);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.acceptButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormSetupMap";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormSetupMap";
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.mapPreview)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox shapeSelector;
		private System.Windows.Forms.FlowLayoutPanel parametersPanel;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.ComboBox wormholeSelector;
		private System.Windows.Forms.ComboBox populatorSelector;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.PictureBox mapPreview;
	}
}