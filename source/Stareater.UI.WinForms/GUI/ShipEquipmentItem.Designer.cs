namespace Stareater.GUI
{
	partial class ShipEquipmentItem
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.amountLabel = new System.Windows.Forms.Label();
			this.thumbnail = new System.Windows.Forms.PictureBox();
			this.nameLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail)).BeginInit();
			this.SuspendLayout();
			// 
			// amountLabel
			// 
			this.amountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.amountLabel.Location = new System.Drawing.Point(0, 0);
			this.amountLabel.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.amountLabel.Name = "amountLabel";
			this.amountLabel.Size = new System.Drawing.Size(50, 30);
			this.amountLabel.TabIndex = 0;
			this.amountLabel.Text = "x.xx X";
			this.amountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.amountLabel.Click += new System.EventHandler(this.amountLabel_Click);
			// 
			// thumbnail
			// 
			this.thumbnail.Location = new System.Drawing.Point(53, 0);
			this.thumbnail.Name = "thumbnail";
			this.thumbnail.Size = new System.Drawing.Size(30, 30);
			this.thumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.thumbnail.TabIndex = 1;
			this.thumbnail.TabStop = false;
			this.thumbnail.Click += new System.EventHandler(this.thumbnail_Click);
			// 
			// nameLabel
			// 
			this.nameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.nameLabel.Location = new System.Drawing.Point(86, 0);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(150, 30);
			this.nameLabel.TabIndex = 1;
			this.nameLabel.Text = "Name";
			this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.nameLabel.Click += new System.EventHandler(this.nameLabel_Click);
			// 
			// ShipEquipmentItem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.thumbnail);
			this.Controls.Add(this.amountLabel);
			this.Name = "ShipEquipmentItem";
			this.Size = new System.Drawing.Size(375, 30);
			((System.ComponentModel.ISupportInitialize)(this.thumbnail)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label amountLabel;
		private System.Windows.Forms.PictureBox thumbnail;
		private System.Windows.Forms.Label nameLabel;
	}
}
