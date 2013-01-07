namespace Stareater.GUI
{
	partial class FormNewGame
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
			this.setupPlayersButton = new System.Windows.Forms.Button();
			this.startButton = new System.Windows.Forms.Button();
			this.startingDescription = new System.Windows.Forms.TextBox();
			this.setupMapButton = new System.Windows.Forms.Button();
			this.mapDescription = new System.Windows.Forms.TextBox();
			this.playerViewsLayout = new System.Windows.Forms.FlowLayoutPanel();
			this.setupStartSelector = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// setupPlayersButton
			// 
			this.setupPlayersButton.Location = new System.Drawing.Point(12, 12);
			this.setupPlayersButton.Name = "setupPlayersButton";
			this.setupPlayersButton.Size = new System.Drawing.Size(170, 23);
			this.setupPlayersButton.TabIndex = 0;
			this.setupPlayersButton.Text = "Setup players";
			this.setupPlayersButton.UseVisualStyleBackColor = true;
			this.setupPlayersButton.Click += new System.EventHandler(this.setupPlayersButton_Click);
			// 
			// startButton
			// 
			this.startButton.Location = new System.Drawing.Point(283, 244);
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size(75, 23);
			this.startButton.TabIndex = 6;
			this.startButton.Text = "Start";
			this.startButton.UseVisualStyleBackColor = true;
			// 
			// startingDescription
			// 
			this.startingDescription.Location = new System.Drawing.Point(188, 155);
			this.startingDescription.Multiline = true;
			this.startingDescription.Name = "startingDescription";
			this.startingDescription.ReadOnly = true;
			this.startingDescription.Size = new System.Drawing.Size(170, 81);
			this.startingDescription.TabIndex = 5;
			// 
			// setupMapButton
			// 
			this.setupMapButton.Location = new System.Drawing.Point(188, 12);
			this.setupMapButton.Name = "setupMapButton";
			this.setupMapButton.Size = new System.Drawing.Size(170, 23);
			this.setupMapButton.TabIndex = 2;
			this.setupMapButton.Text = "Map";
			this.setupMapButton.UseVisualStyleBackColor = true;
			// 
			// mapDescription
			// 
			this.mapDescription.Location = new System.Drawing.Point(188, 41);
			this.mapDescription.Multiline = true;
			this.mapDescription.Name = "mapDescription";
			this.mapDescription.ReadOnly = true;
			this.mapDescription.Size = new System.Drawing.Size(170, 81);
			this.mapDescription.TabIndex = 3;
			// 
			// playerViewsLayout
			// 
			this.playerViewsLayout.Location = new System.Drawing.Point(12, 41);
			this.playerViewsLayout.Name = "playerViewsLayout";
			this.playerViewsLayout.Size = new System.Drawing.Size(170, 195);
			this.playerViewsLayout.TabIndex = 1;
			// 
			// setupStartSelector
			// 
			this.setupStartSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.setupStartSelector.FormattingEnabled = true;
			this.setupStartSelector.Location = new System.Drawing.Point(188, 128);
			this.setupStartSelector.Name = "setupStartSelector";
			this.setupStartSelector.Size = new System.Drawing.Size(170, 21);
			this.setupStartSelector.TabIndex = 7;
			this.setupStartSelector.SelectedIndexChanged += new System.EventHandler(this.setupStartSelector_SelectedIndexChanged);
			// 
			// FormNewGame
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(372, 282);
			this.Controls.Add(this.setupStartSelector);
			this.Controls.Add(this.playerViewsLayout);
			this.Controls.Add(this.setupPlayersButton);
			this.Controls.Add(this.startButton);
			this.Controls.Add(this.startingDescription);
			this.Controls.Add(this.setupMapButton);
			this.Controls.Add(this.mapDescription);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormNewGame";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Nova igra";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button setupPlayersButton;
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.TextBox startingDescription;
		private System.Windows.Forms.Button setupMapButton;
		private System.Windows.Forms.TextBox mapDescription;
		private System.Windows.Forms.FlowLayoutPanel playerViewsLayout;
		private System.Windows.Forms.ComboBox setupStartSelector;

	}
}