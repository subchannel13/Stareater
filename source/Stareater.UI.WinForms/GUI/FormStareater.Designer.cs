namespace Stareater.GUI
{
	partial class FormStareater
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
			if (disposing && (components != null))
			{
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
			this.whoControlsLabel = new System.Windows.Forms.Label();
			this.starSelector = new System.Windows.Forms.ComboBox();
			this.ejectLabel = new System.Windows.Forms.Label();
			this.selectionInfo = new System.Windows.Forms.Label();
			this.gameProgressInfo = new System.Windows.Forms.Label();
			this.closeAction = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// whoControlsLabel
			// 
			this.whoControlsLabel.AutoSize = true;
			this.whoControlsLabel.Location = new System.Drawing.Point(9, 9);
			this.whoControlsLabel.Name = "whoControlsLabel";
			this.whoControlsLabel.Size = new System.Drawing.Size(150, 13);
			this.whoControlsLabel.TabIndex = 0;
			this.whoControlsLabel.Text = "You do/don\'t control Stareater";
			// 
			// starSelector
			// 
			this.starSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.starSelector.FormattingEnabled = true;
			this.starSelector.Location = new System.Drawing.Point(12, 50);
			this.starSelector.Name = "starSelector";
			this.starSelector.Size = new System.Drawing.Size(150, 21);
			this.starSelector.TabIndex = 1;
			this.starSelector.SelectedIndexChanged += new System.EventHandler(this.starSelector_SelectedIndexChanged);
			// 
			// ejectLabel
			// 
			this.ejectLabel.AutoSize = true;
			this.ejectLabel.Location = new System.Drawing.Point(9, 34);
			this.ejectLabel.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
			this.ejectLabel.Name = "ejectLabel";
			this.ejectLabel.Size = new System.Drawing.Size(34, 13);
			this.ejectLabel.TabIndex = 2;
			this.ejectLabel.Text = "Eject:";
			// 
			// selectionInfo
			// 
			this.selectionInfo.AutoSize = true;
			this.selectionInfo.Location = new System.Drawing.Point(12, 74);
			this.selectionInfo.Name = "selectionInfo";
			this.selectionInfo.Size = new System.Drawing.Size(65, 13);
			this.selectionInfo.TabIndex = 3;
			this.selectionInfo.Text = "ETA: x truns";
			// 
			// gameProgressInfo
			// 
			this.gameProgressInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gameProgressInfo.Location = new System.Drawing.Point(168, 9);
			this.gameProgressInfo.Name = "gameProgressInfo";
			this.gameProgressInfo.Size = new System.Drawing.Size(159, 100);
			this.gameProgressInfo.TabIndex = 4;
			this.gameProgressInfo.Text = "Victory points:";
			// 
			// closeAction
			// 
			this.closeAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.closeAction.Location = new System.Drawing.Point(252, 181);
			this.closeAction.Name = "closeAction";
			this.closeAction.Size = new System.Drawing.Size(75, 23);
			this.closeAction.TabIndex = 5;
			this.closeAction.Text = "button1";
			this.closeAction.UseVisualStyleBackColor = true;
			this.closeAction.Click += new System.EventHandler(this.closeAction_Click);
			// 
			// FormStareater
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(339, 216);
			this.Controls.Add(this.closeAction);
			this.Controls.Add(this.gameProgressInfo);
			this.Controls.Add(this.selectionInfo);
			this.Controls.Add(this.ejectLabel);
			this.Controls.Add(this.starSelector);
			this.Controls.Add(this.whoControlsLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormStareater";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormStareater";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label whoControlsLabel;
		private System.Windows.Forms.ComboBox starSelector;
		private System.Windows.Forms.Label ejectLabel;
		private System.Windows.Forms.Label selectionInfo;
		private System.Windows.Forms.Label gameProgressInfo;
		private System.Windows.Forms.Button closeAction;
	}
}