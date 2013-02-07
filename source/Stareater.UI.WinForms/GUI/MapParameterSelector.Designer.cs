namespace Stareater.GUI
{
	partial class MapParameterSelector
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.valueSelector = new System.Windows.Forms.ComboBox();
			this.nameLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// valueSelector
			// 
			this.valueSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.valueSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.valueSelector.FormattingEnabled = true;
			this.valueSelector.Location = new System.Drawing.Point(0, 16);
			this.valueSelector.Name = "valueSelector";
			this.valueSelector.Size = new System.Drawing.Size(140, 21);
			this.valueSelector.TabIndex = 21;
			this.valueSelector.SelectedIndexChanged += new System.EventHandler(this.valueSelector_SelectedIndexChanged);
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Location = new System.Drawing.Point(-3, 0);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(35, 13);
			this.nameLabel.TabIndex = 22;
			this.nameLabel.Text = "label1";
			// 
			// MapParameterSelector
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.valueSelector);
			this.Name = "MapParameterSelector";
			this.Size = new System.Drawing.Size(140, 40);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox valueSelector;
		private System.Windows.Forms.Label nameLabel;
	}
}
