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
			this.fpsTitle = new System.Windows.Forms.Label();
			this.batteryFpsTitle = new System.Windows.Forms.Label();
			this.fpsInput = new System.Windows.Forms.NumericUpDown();
			this.batteryFpsInput = new System.Windows.Forms.NumericUpDown();
			this.unlimitedFpsCheck = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.fpsInput)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.batteryFpsInput)).BeginInit();
			this.SuspendLayout();
			// 
			// languageTitle
			// 
			this.languageTitle.AutoSize = true;
			this.languageTitle.Location = new System.Drawing.Point(12, 9);
			this.languageTitle.Name = "languageTitle";
			this.languageTitle.Size = new System.Drawing.Size(34, 13);
			this.languageTitle.TabIndex = 0;
			this.languageTitle.Text = "Jezik:";
			// 
			// languageSelector
			// 
			this.languageSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.languageSelector.FormattingEnabled = true;
			this.languageSelector.Location = new System.Drawing.Point(12, 25);
			this.languageSelector.Name = "languageSelector";
			this.languageSelector.Size = new System.Drawing.Size(176, 21);
			this.languageSelector.TabIndex = 1;
			this.languageSelector.SelectedIndexChanged += new System.EventHandler(this.languageSelector_SelectedIndexChanged);
			// 
			// confirmButton
			// 
			this.confirmButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.confirmButton.Location = new System.Drawing.Point(317, 154);
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
			this.guiScaleTitle.Location = new System.Drawing.Point(12, 58);
			this.guiScaleTitle.Name = "guiScaleTitle";
			this.guiScaleTitle.Size = new System.Drawing.Size(83, 13);
			this.guiScaleTitle.TabIndex = 4;
			this.guiScaleTitle.Text = "Velicina sučelja:";
			// 
			// guiScaleSelector
			// 
			this.guiScaleSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.guiScaleSelector.FormattingEnabled = true;
			this.guiScaleSelector.Location = new System.Drawing.Point(127, 55);
			this.guiScaleSelector.Name = "guiScaleSelector";
			this.guiScaleSelector.Size = new System.Drawing.Size(61, 21);
			this.guiScaleSelector.TabIndex = 5;
			this.guiScaleSelector.SelectedIndexChanged += new System.EventHandler(this.guiScaleSelector_SelectedIndexChanged);
			// 
			// rendererInfo
			// 
			this.rendererInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.rendererInfo.AutoSize = true;
			this.rendererInfo.Location = new System.Drawing.Point(214, 125);
			this.rendererInfo.Name = "rendererInfo";
			this.rendererInfo.Size = new System.Drawing.Size(137, 26);
			this.rendererInfo.TabIndex = 6;
			this.rendererInfo.Text = "Rendering hardware:\r\nA line long equipment name";
			// 
			// fpsTitle
			// 
			this.fpsTitle.AutoSize = true;
			this.fpsTitle.Location = new System.Drawing.Point(214, 9);
			this.fpsTitle.Name = "fpsTitle";
			this.fpsTitle.Size = new System.Drawing.Size(30, 13);
			this.fpsTitle.TabIndex = 7;
			this.fpsTitle.Text = "FPS:";
			// 
			// batteryFpsTitle
			// 
			this.batteryFpsTitle.AutoSize = true;
			this.batteryFpsTitle.Location = new System.Drawing.Point(214, 58);
			this.batteryFpsTitle.Name = "batteryFpsTitle";
			this.batteryFpsTitle.Size = new System.Drawing.Size(66, 13);
			this.batteryFpsTitle.TabIndex = 9;
			this.batteryFpsTitle.Text = "Battery FPS:";
			// 
			// fpsInput
			// 
			this.fpsInput.Increment = new decimal(new int[] {
			15,
			0,
			0,
			0});
			this.fpsInput.Location = new System.Drawing.Point(306, 7);
			this.fpsInput.Maximum = new decimal(new int[] {
			300,
			0,
			0,
			0});
			this.fpsInput.Minimum = new decimal(new int[] {
			30,
			0,
			0,
			0});
			this.fpsInput.Name = "fpsInput";
			this.fpsInput.Size = new System.Drawing.Size(45, 20);
			this.fpsInput.TabIndex = 11;
			this.fpsInput.Value = new decimal(new int[] {
			30,
			0,
			0,
			0});
			// 
			// batteryFpsInput
			// 
			this.batteryFpsInput.Increment = new decimal(new int[] {
			15,
			0,
			0,
			0});
			this.batteryFpsInput.Location = new System.Drawing.Point(306, 56);
			this.batteryFpsInput.Maximum = new decimal(new int[] {
			300,
			0,
			0,
			0});
			this.batteryFpsInput.Minimum = new decimal(new int[] {
			30,
			0,
			0,
			0});
			this.batteryFpsInput.Name = "batteryFpsInput";
			this.batteryFpsInput.Size = new System.Drawing.Size(45, 20);
			this.batteryFpsInput.TabIndex = 12;
			this.batteryFpsInput.Value = new decimal(new int[] {
			30,
			0,
			0,
			0});
			// 
			// unlimitedFpsCheck
			// 
			this.unlimitedFpsCheck.AutoSize = true;
			this.unlimitedFpsCheck.Location = new System.Drawing.Point(214, 33);
			this.unlimitedFpsCheck.Name = "unlimitedFpsCheck";
			this.unlimitedFpsCheck.Size = new System.Drawing.Size(116, 17);
			this.unlimitedFpsCheck.TabIndex = 13;
			this.unlimitedFpsCheck.Text = "Unlimited framerate";
			this.unlimitedFpsCheck.UseVisualStyleBackColor = true;
			this.unlimitedFpsCheck.CheckedChanged += new System.EventHandler(this.UnlimitedFpsCheckCheckedChanged);
			// 
			// FormSettings
			// 
			this.AcceptButton = this.confirmButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(404, 189);
			this.Controls.Add(this.unlimitedFpsCheck);
			this.Controls.Add(this.batteryFpsInput);
			this.Controls.Add(this.fpsInput);
			this.Controls.Add(this.batteryFpsTitle);
			this.Controls.Add(this.fpsTitle);
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
			((System.ComponentModel.ISupportInitialize)(this.fpsInput)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.batteryFpsInput)).EndInit();
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
		private System.Windows.Forms.Label fpsTitle;
		private System.Windows.Forms.Label batteryFpsTitle;
		private System.Windows.Forms.NumericUpDown fpsInput;
		private System.Windows.Forms.NumericUpDown batteryFpsInput;
		private System.Windows.Forms.CheckBox unlimitedFpsCheck;
	}
}