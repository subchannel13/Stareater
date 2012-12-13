namespace Stareater.GUI
{
	partial class FormSetupPlayers
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
			this.controllerPicker = new System.Windows.Forms.ComboBox();
			this.controllerLabel = new System.Windows.Forms.Label();
			this.organizationDescription = new System.Windows.Forms.TextBox();
			this.organizationPicker = new System.Windows.Forms.ComboBox();
			this.organizationLabel = new System.Windows.Forms.Label();
			this.nameInput = new System.Windows.Forms.TextBox();
			this.nameLabel = new System.Windows.Forms.Label();
			this.removeButton = new System.Windows.Forms.Button();
			this.addButton = new System.Windows.Forms.Button();
			this.colorsLayout = new Stareater.GUI.ControlListView();
			this.playerViewsLayout = new Stareater.GUI.ControlListView();
			this.SuspendLayout();
			// 
			// controllerPicker
			// 
			this.controllerPicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.controllerPicker.FormattingEnabled = true;
			this.controllerPicker.Location = new System.Drawing.Point(261, 12);
			this.controllerPicker.Name = "controllerPicker";
			this.controllerPicker.Size = new System.Drawing.Size(115, 21);
			this.controllerPicker.TabIndex = 40;
			this.controllerPicker.SelectedIndexChanged += new System.EventHandler(this.controllerPicker_SelectedIndexChanged);
			// 
			// controllerLabel
			// 
			this.controllerLabel.AutoSize = true;
			this.controllerLabel.Location = new System.Drawing.Point(188, 15);
			this.controllerLabel.Name = "controllerLabel";
			this.controllerLabel.Size = new System.Drawing.Size(54, 13);
			this.controllerLabel.TabIndex = 39;
			this.controllerLabel.Text = "Controller:";
			// 
			// organizationDescription
			// 
			this.organizationDescription.Location = new System.Drawing.Point(191, 92);
			this.organizationDescription.Multiline = true;
			this.organizationDescription.Name = "organizationDescription";
			this.organizationDescription.ReadOnly = true;
			this.organizationDescription.Size = new System.Drawing.Size(188, 111);
			this.organizationDescription.TabIndex = 38;
			this.organizationDescription.Text = "North American Space Agency\r\n\r\n+2 on Space program\r\n+1 on Laser\r\n3\r\n4\r\n5";
			// 
			// organizationPicker
			// 
			this.organizationPicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.organizationPicker.FormattingEnabled = true;
			this.organizationPicker.Location = new System.Drawing.Point(261, 65);
			this.organizationPicker.Name = "organizationPicker";
			this.organizationPicker.Size = new System.Drawing.Size(115, 21);
			this.organizationPicker.TabIndex = 37;
			this.organizationPicker.SelectedIndexChanged += new System.EventHandler(this.organizationPicker_SelectedIndexChanged);
			// 
			// organizationLabel
			// 
			this.organizationLabel.AutoSize = true;
			this.organizationLabel.Location = new System.Drawing.Point(188, 68);
			this.organizationLabel.Name = "organizationLabel";
			this.organizationLabel.Size = new System.Drawing.Size(69, 13);
			this.organizationLabel.TabIndex = 36;
			this.organizationLabel.Text = "Organization:";
			// 
			// nameInput
			// 
			this.nameInput.Location = new System.Drawing.Point(261, 39);
			this.nameInput.Name = "nameInput";
			this.nameInput.Size = new System.Drawing.Size(115, 20);
			this.nameInput.TabIndex = 35;
			this.nameInput.TextChanged += new System.EventHandler(this.nameInput_TextChanged);
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Location = new System.Drawing.Point(188, 42);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(38, 13);
			this.nameLabel.TabIndex = 34;
			this.nameLabel.Text = "Name:";
			// 
			// removeButton
			// 
			this.removeButton.Location = new System.Drawing.Point(107, 209);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(75, 23);
			this.removeButton.TabIndex = 33;
			this.removeButton.Text = "Remove";
			this.removeButton.UseVisualStyleBackColor = true;
			// 
			// addButton
			// 
			this.addButton.Location = new System.Drawing.Point(12, 209);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(75, 23);
			this.addButton.TabIndex = 32;
			this.addButton.Text = "Add";
			this.addButton.UseVisualStyleBackColor = true;
			// 
			// colorsLayout
			// 
			this.colorsLayout.Location = new System.Drawing.Point(385, 12);
			this.colorsLayout.Name = "colorsLayout";
			this.colorsLayout.SelectedIndex = -1;
			this.colorsLayout.Size = new System.Drawing.Size(77, 191);
			this.colorsLayout.TabIndex = 42;
			// 
			// playerViewsLayout
			// 
			this.playerViewsLayout.Location = new System.Drawing.Point(12, 12);
			this.playerViewsLayout.Name = "playerViewsLayout";
			this.playerViewsLayout.SelectedIndex = -1;
			this.playerViewsLayout.Size = new System.Drawing.Size(170, 191);
			this.playerViewsLayout.TabIndex = 41;
			this.playerViewsLayout.SelectedIndexChanged += new System.EventHandler(this.playerViewsLayout_SelectedIndexChanged);
			// 
			// FormSetupPlayers
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(480, 256);
			this.Controls.Add(this.colorsLayout);
			this.Controls.Add(this.playerViewsLayout);
			this.Controls.Add(this.controllerPicker);
			this.Controls.Add(this.controllerLabel);
			this.Controls.Add(this.organizationDescription);
			this.Controls.Add(this.organizationPicker);
			this.Controls.Add(this.organizationLabel);
			this.Controls.Add(this.nameInput);
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.addButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormSetupPlayers";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormSetupPlayers";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox controllerPicker;
		private System.Windows.Forms.Label controllerLabel;
		private System.Windows.Forms.TextBox organizationDescription;
		private System.Windows.Forms.ComboBox organizationPicker;
		private System.Windows.Forms.Label organizationLabel;
		private System.Windows.Forms.TextBox nameInput;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button addButton;
		private ControlListView playerViewsLayout;
		private ControlListView colorsLayout;
	}
}