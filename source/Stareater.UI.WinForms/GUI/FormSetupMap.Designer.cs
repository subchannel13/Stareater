﻿namespace Stareater.GUI
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
			this.SuspendLayout();
			// 
			// shapeSelector
			// 
			this.shapeSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.shapeSelector.FormattingEnabled = true;
			this.shapeSelector.Location = new System.Drawing.Point(12, 12);
			this.shapeSelector.Name = "shapeSelector";
			this.shapeSelector.Size = new System.Drawing.Size(170, 21);
			this.shapeSelector.TabIndex = 0;
			this.shapeSelector.SelectedIndexChanged += new System.EventHandler(this.shapeSelector_SelectedIndexChanged);
			// 
			// parametersPanel
			// 
			this.parametersPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.parametersPanel.AutoScroll = true;
			this.parametersPanel.Location = new System.Drawing.Point(12, 93);
			this.parametersPanel.Name = "parametersPanel";
			this.parametersPanel.Size = new System.Drawing.Size(170, 252);
			this.parametersPanel.TabIndex = 36;
			// 
			// acceptButton
			// 
			this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.acceptButton.Location = new System.Drawing.Point(107, 351);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.Size = new System.Drawing.Size(75, 23);
			this.acceptButton.TabIndex = 37;
			this.acceptButton.Text = "button1";
			this.acceptButton.UseVisualStyleBackColor = true;
			this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
			// 
			// wormholeSelector
			// 
			this.wormholeSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.wormholeSelector.FormattingEnabled = true;
			this.wormholeSelector.Location = new System.Drawing.Point(12, 39);
			this.wormholeSelector.Name = "wormholeSelector";
			this.wormholeSelector.Size = new System.Drawing.Size(170, 21);
			this.wormholeSelector.TabIndex = 38;
			// 
			// populatorSelector
			// 
			this.populatorSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.populatorSelector.FormattingEnabled = true;
			this.populatorSelector.Location = new System.Drawing.Point(12, 66);
			this.populatorSelector.Name = "populatorSelector";
			this.populatorSelector.Size = new System.Drawing.Size(170, 21);
			this.populatorSelector.TabIndex = 39;
			// 
			// FormSetupMap
			// 
			this.AcceptButton = this.acceptButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.acceptButton;
			this.ClientSize = new System.Drawing.Size(195, 382);
			this.Controls.Add(this.populatorSelector);
			this.Controls.Add(this.wormholeSelector);
			this.Controls.Add(this.acceptButton);
			this.Controls.Add(this.parametersPanel);
			this.Controls.Add(this.shapeSelector);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormSetupMap";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormSetupMap";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox shapeSelector;
		private System.Windows.Forms.FlowLayoutPanel parametersPanel;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.ComboBox wormholeSelector;
		private System.Windows.Forms.ComboBox populatorSelector;
	}
}