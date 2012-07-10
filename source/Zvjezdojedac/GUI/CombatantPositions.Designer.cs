namespace Zvjezdojedac.GUI
{
	partial class CombatantPositions
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
			this.picPosition0 = new System.Windows.Forms.PictureBox();
			this.picPosition1 = new System.Windows.Forms.PictureBox();
			this.picPosition2 = new System.Windows.Forms.PictureBox();
			this.picPosition3 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.picPosition0)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picPosition1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picPosition2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picPosition3)).BeginInit();
			this.SuspendLayout();
			// 
			// picPosition0
			// 
			this.picPosition0.BackColor = System.Drawing.Color.Black;
			this.picPosition0.Location = new System.Drawing.Point(0, 0);
			this.picPosition0.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.picPosition0.Name = "picPosition0";
			this.picPosition0.Size = new System.Drawing.Size(40, 40);
			this.picPosition0.TabIndex = 0;
			this.picPosition0.TabStop = false;
			this.picPosition0.Click += new System.EventHandler(this.picPosition0_Click);
			// 
			// picPosition1
			// 
			this.picPosition1.BackColor = System.Drawing.Color.Black;
			this.picPosition1.Location = new System.Drawing.Point(42, 0);
			this.picPosition1.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.picPosition1.Name = "picPosition1";
			this.picPosition1.Size = new System.Drawing.Size(40, 40);
			this.picPosition1.TabIndex = 1;
			this.picPosition1.TabStop = false;
			this.picPosition1.Click += new System.EventHandler(this.picPosition1_Click);
			// 
			// picPosition2
			// 
			this.picPosition2.BackColor = System.Drawing.Color.Black;
			this.picPosition2.Location = new System.Drawing.Point(84, 0);
			this.picPosition2.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.picPosition2.Name = "picPosition2";
			this.picPosition2.Size = new System.Drawing.Size(40, 40);
			this.picPosition2.TabIndex = 2;
			this.picPosition2.TabStop = false;
			this.picPosition2.Click += new System.EventHandler(this.picPosition2_Click);
			// 
			// picPosition3
			// 
			this.picPosition3.BackColor = System.Drawing.Color.Black;
			this.picPosition3.Location = new System.Drawing.Point(126, 0);
			this.picPosition3.Margin = new System.Windows.Forms.Padding(0);
			this.picPosition3.Name = "picPosition3";
			this.picPosition3.Size = new System.Drawing.Size(40, 40);
			this.picPosition3.TabIndex = 3;
			this.picPosition3.TabStop = false;
			this.picPosition3.Click += new System.EventHandler(this.picPosition3_Click);
			// 
			// CombatantPositions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Red;
			this.Controls.Add(this.picPosition3);
			this.Controls.Add(this.picPosition2);
			this.Controls.Add(this.picPosition1);
			this.Controls.Add(this.picPosition0);
			this.Name = "CombatantPositions";
			this.Size = new System.Drawing.Size(166, 40);
			((System.ComponentModel.ISupportInitialize)(this.picPosition0)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picPosition1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picPosition2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picPosition3)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox picPosition0;
		private System.Windows.Forms.PictureBox picPosition1;
		private System.Windows.Forms.PictureBox picPosition2;
		private System.Windows.Forms.PictureBox picPosition3;
	}
}
