namespace Stareater.GUI
{
	partial class FormSettings
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
			this.languageTitle = new System.Windows.Forms.Label();
			this.languageSelector = new System.Windows.Forms.ComboBox();
			this.confirmButton = new System.Windows.Forms.Button();
			this.guiScaleTitle = new System.Windows.Forms.Label();
			this.guiScaleSelector = new System.Windows.Forms.ComboBox();
			this.rendererInfo = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// languageTitle
			// 
			this.languageTitle.AutoSize = true;
			this.languageTitle.Location = new System.Drawing.Point(12, 23);
			this.languageTitle.Name = "languageTitle";
			this.languageTitle.Size = new System.Drawing.Size(34, 13);
			this.languageTitle.TabIndex = 0;
			this.languageTitle.Text = "Jezik:";
			// 
			// languageSelector
			// 
			this.languageSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.languageSelector.FormattingEnabled = true;
			this.languageSelector.Location = new System.Drawing.Point(12, 39);
			this.languageSelector.Name = "languageSelector";
			this.languageSelector.Size = new System.Drawing.Size(176, 21);
			this.languageSelector.TabIndex = 1;
			this.languageSelector.SelectedIndexChanged += new System.EventHandler(this.languageSelector_SelectedIndexChanged);
			// 
			// confirmButton
			// 
			this.confirmButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.confirmButton.Location = new System.Drawing.Point(113, 154);
			this.confirmButton.Name = "confirmButton";
			this.confirmButton.Size = new System.Drawing.Size(75, 23);
			this.confirmButton.TabIndex = 3;
			this.confirmButton.Text = "U redu";
			this.confirmButton.UseVisualStyleBackColor = true;
			this.confirmButton.Click += new System.EventHandler(this.confirmButton_Click);
			// 
			// guiScaleTitle
			// 
			this.guiScaleTitle.AutoSize = true;
			this.guiScaleTitle.Location = new System.Drawing.Point(12, 72);
			this.guiScaleTitle.Name = "guiScaleTitle";
			this.guiScaleTitle.Size = new System.Drawing.Size(83, 13);
			this.guiScaleTitle.TabIndex = 4;
			this.guiScaleTitle.Text = "Velicina sučelja:";
			// 
			// guiScaleSelector
			// 
			this.guiScaleSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.guiScaleSelector.FormattingEnabled = true;
			this.guiScaleSelector.Location = new System.Drawing.Point(127, 69);
			this.guiScaleSelector.Name = "guiScaleSelector";
			this.guiScaleSelector.Size = new System.Drawing.Size(61, 21);
			this.guiScaleSelector.TabIndex = 5;
			this.guiScaleSelector.SelectedIndexChanged += new System.EventHandler(this.guiScaleSelector_SelectedIndexChanged);
			// 
			// rendererInfo
			// 
			this.rendererInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.rendererInfo.AutoSize = true;
			this.rendererInfo.Location = new System.Drawing.Point(12, 125);
			this.rendererInfo.Name = "rendererInfo";
			this.rendererInfo.Size = new System.Drawing.Size(137, 26);
			this.rendererInfo.TabIndex = 6;
			this.rendererInfo.Text = "Rendering hardware:\r\nA line long equipment name";
			// 
			// FormSettings
			// 
			this.AcceptButton = this.confirmButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(200, 189);
			this.Controls.Add(this.rendererInfo);
			this.Controls.Add(this.guiScaleSelector);
			this.Controls.Add(this.guiScaleTitle);
			this.Controls.Add(this.confirmButton);
			this.Controls.Add(this.languageSelector);
			this.Controls.Add(this.languageTitle);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormSettings";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Postavke";
			this.Load += new System.EventHandler(this.FormSettings_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label languageTitle;
		private System.Windows.Forms.ComboBox languageSelector;
		private System.Windows.Forms.Button confirmButton;
		private System.Windows.Forms.Label guiScaleTitle;
		private System.Windows.Forms.ComboBox guiScaleSelector;
		private System.Windows.Forms.Label rendererInfo;
	}
}