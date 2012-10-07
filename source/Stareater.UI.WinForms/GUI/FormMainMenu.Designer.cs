namespace Stareater.GUI
{
	partial class FormMainMenu
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
			this.newGameButton = new System.Windows.Forms.Button();
			this.loadGameButton = new System.Windows.Forms.Button();
			this.exitButton = new System.Windows.Forms.Button();
			this.titleLabel = new System.Windows.Forms.Label();
			this.aboutLabel = new System.Windows.Forms.Label();
			this.settingsButton = new System.Windows.Forms.Button();
			this.saveGameButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// newGameButton
			// 
			this.newGameButton.Location = new System.Drawing.Point(109, 64);
			this.newGameButton.Name = "newGameButton";
			this.newGameButton.Size = new System.Drawing.Size(75, 30);
			this.newGameButton.TabIndex = 1;
			this.newGameButton.Text = "Nova igra";
			this.newGameButton.UseVisualStyleBackColor = true;
			this.newGameButton.Click += new System.EventHandler(this.newGameButton_Click);
			// 
			// loadGameButton
			// 
			this.loadGameButton.Location = new System.Drawing.Point(109, 136);
			this.loadGameButton.Name = "loadGameButton";
			this.loadGameButton.Size = new System.Drawing.Size(75, 30);
			this.loadGameButton.TabIndex = 2;
			this.loadGameButton.Text = "Učitaj igru";
			this.loadGameButton.UseVisualStyleBackColor = true;
			this.loadGameButton.Click += new System.EventHandler(this.loadGameButton_Click);
			// 
			// exitButton
			// 
			this.exitButton.Location = new System.Drawing.Point(109, 208);
			this.exitButton.Name = "exitButton";
			this.exitButton.Size = new System.Drawing.Size(75, 30);
			this.exitButton.TabIndex = 4;
			this.exitButton.Text = "Ugasi";
			this.exitButton.UseVisualStyleBackColor = true;
			this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
			// 
			// titleLabel
			// 
			this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.titleLabel.Location = new System.Drawing.Point(12, 9);
			this.titleLabel.Name = "titleLabel";
			this.titleLabel.Size = new System.Drawing.Size(268, 30);
			this.titleLabel.TabIndex = 0;
			this.titleLabel.Text = "Zvjezdojedac";
			this.titleLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// aboutLabel
			// 
			this.aboutLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.aboutLabel.AutoSize = true;
			this.aboutLabel.Location = new System.Drawing.Point(165, 260);
			this.aboutLabel.Name = "aboutLabel";
			this.aboutLabel.Size = new System.Drawing.Size(115, 13);
			this.aboutLabel.TabIndex = 5;
			this.aboutLabel.Text = "Ivan Kravarščan 2012.";
			// 
			// settingsButton
			// 
			this.settingsButton.Location = new System.Drawing.Point(109, 172);
			this.settingsButton.Name = "settingsButton";
			this.settingsButton.Size = new System.Drawing.Size(75, 30);
			this.settingsButton.TabIndex = 3;
			this.settingsButton.Text = "Postavke";
			this.settingsButton.UseVisualStyleBackColor = true;
			this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
			// 
			// saveGameButton
			// 
			this.saveGameButton.Location = new System.Drawing.Point(110, 100);
			this.saveGameButton.Name = "saveGameButton";
			this.saveGameButton.Size = new System.Drawing.Size(75, 30);
			this.saveGameButton.TabIndex = 6;
			this.saveGameButton.Text = "Spremi igru";
			this.saveGameButton.UseVisualStyleBackColor = true;
			this.saveGameButton.Click += new System.EventHandler(this.saveGameButton_Click);
			// 
			// FormMainMenu
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 282);
			this.Controls.Add(this.saveGameButton);
			this.Controls.Add(this.settingsButton);
			this.Controls.Add(this.aboutLabel);
			this.Controls.Add(this.titleLabel);
			this.Controls.Add(this.exitButton);
			this.Controls.Add(this.loadGameButton);
			this.Controls.Add(this.newGameButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormMainMenu";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Zvjezdojedac";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button newGameButton;
		private System.Windows.Forms.Button loadGameButton;
		private System.Windows.Forms.Button exitButton;
		private System.Windows.Forms.Label titleLabel;
		private System.Windows.Forms.Label aboutLabel;
		private System.Windows.Forms.Button settingsButton;
		private System.Windows.Forms.Button saveGameButton;
	}
}

