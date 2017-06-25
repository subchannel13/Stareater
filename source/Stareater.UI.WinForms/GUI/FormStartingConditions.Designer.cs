namespace Stareater.GUI
{
	partial class FormStartingConditions
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
			this.coloniesLabel = new System.Windows.Forms.Label();
			this.coloniesSelector = new System.Windows.Forms.NumericUpDown();
			this.infrastructureInput = new System.Windows.Forms.TextBox();
			this.infrastructureLabel = new System.Windows.Forms.Label();
			this.populationInput = new System.Windows.Forms.TextBox();
			this.populationLabel = new System.Windows.Forms.Label();
			this.acceptButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.coloniesSelector)).BeginInit();
			this.SuspendLayout();
			// 
			// coloniesLabel
			// 
			this.coloniesLabel.AutoSize = true;
			this.coloniesLabel.Location = new System.Drawing.Point(12, 14);
			this.coloniesLabel.Name = "coloniesLabel";
			this.coloniesLabel.Size = new System.Drawing.Size(50, 13);
			this.coloniesLabel.TabIndex = 0;
			this.coloniesLabel.Text = "Colonies:";
			// 
			// coloniesSelector
			// 
			this.coloniesSelector.Location = new System.Drawing.Point(90, 12);
			this.coloniesSelector.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.coloniesSelector.Name = "coloniesSelector";
			this.coloniesSelector.Size = new System.Drawing.Size(60, 20);
			this.coloniesSelector.TabIndex = 3;
			this.coloniesSelector.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// infrastructureInput
			// 
			this.infrastructureInput.Location = new System.Drawing.Point(90, 64);
			this.infrastructureInput.Name = "infrastructureInput";
			this.infrastructureInput.Size = new System.Drawing.Size(60, 20);
			this.infrastructureInput.TabIndex = 5;
			this.infrastructureInput.TextChanged += new System.EventHandler(this.infrastructureInput_TextChanged);
			// 
			// infrastructureLabel
			// 
			this.infrastructureLabel.AutoSize = true;
			this.infrastructureLabel.Location = new System.Drawing.Point(12, 67);
			this.infrastructureLabel.Name = "infrastructureLabel";
			this.infrastructureLabel.Size = new System.Drawing.Size(72, 13);
			this.infrastructureLabel.TabIndex = 2;
			this.infrastructureLabel.Text = "Infrastructure:";
			// 
			// populationInput
			// 
			this.populationInput.Location = new System.Drawing.Point(90, 38);
			this.populationInput.Name = "populationInput";
			this.populationInput.Size = new System.Drawing.Size(60, 20);
			this.populationInput.TabIndex = 4;
			this.populationInput.TextChanged += new System.EventHandler(this.populationInput_TextChanged);
			// 
			// populationLabel
			// 
			this.populationLabel.AutoSize = true;
			this.populationLabel.Location = new System.Drawing.Point(12, 41);
			this.populationLabel.Name = "populationLabel";
			this.populationLabel.Size = new System.Drawing.Size(60, 13);
			this.populationLabel.TabIndex = 1;
			this.populationLabel.Text = "Population:";
			// 
			// acceptButton
			// 
			this.acceptButton.Location = new System.Drawing.Point(90, 90);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.Size = new System.Drawing.Size(60, 21);
			this.acceptButton.TabIndex = 6;
			this.acceptButton.Text = "Accept";
			this.acceptButton.UseVisualStyleBackColor = true;
			this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
			// 
			// FormStartingConditions
			// 
			this.AcceptButton = this.acceptButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(167, 120);
			this.Controls.Add(this.acceptButton);
			this.Controls.Add(this.coloniesLabel);
			this.Controls.Add(this.coloniesSelector);
			this.Controls.Add(this.infrastructureInput);
			this.Controls.Add(this.infrastructureLabel);
			this.Controls.Add(this.populationInput);
			this.Controls.Add(this.populationLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormStartingConditions";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormStartingConditions";
			((System.ComponentModel.ISupportInitialize)(this.coloniesSelector)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label coloniesLabel;
		private System.Windows.Forms.NumericUpDown coloniesSelector;
		private System.Windows.Forms.TextBox infrastructureInput;
		private System.Windows.Forms.Label infrastructureLabel;
		private System.Windows.Forms.TextBox populationInput;
		private System.Windows.Forms.Label populationLabel;
		private System.Windows.Forms.Button acceptButton;
	}
}