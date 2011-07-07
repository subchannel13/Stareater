namespace Zvjezdojedac.GUI
{
	partial class FormPostavke
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
			this.lblJezik = new System.Windows.Forms.Label();
			this.cbJezik = new System.Windows.Forms.ComboBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.lblVelicina = new System.Windows.Forms.Label();
			this.cbVelicina = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// lblJezik
			// 
			this.lblJezik.AutoSize = true;
			this.lblJezik.Location = new System.Drawing.Point(12, 23);
			this.lblJezik.Name = "lblJezik";
			this.lblJezik.Size = new System.Drawing.Size(34, 13);
			this.lblJezik.TabIndex = 0;
			this.lblJezik.Text = "Jezik:";
			// 
			// cbJezik
			// 
			this.cbJezik.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbJezik.FormattingEnabled = true;
			this.cbJezik.Location = new System.Drawing.Point(12, 39);
			this.cbJezik.Name = "cbJezik";
			this.cbJezik.Size = new System.Drawing.Size(176, 21);
			this.cbJezik.TabIndex = 1;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(113, 111);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 3;
			this.btnOk.Text = "U redu";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// lblVelicina
			// 
			this.lblVelicina.AutoSize = true;
			this.lblVelicina.Location = new System.Drawing.Point(12, 72);
			this.lblVelicina.Name = "lblVelicina";
			this.lblVelicina.Size = new System.Drawing.Size(83, 13);
			this.lblVelicina.TabIndex = 4;
			this.lblVelicina.Text = "Velicina sučelja:";
			// 
			// cbVelicina
			// 
			this.cbVelicina.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbVelicina.FormattingEnabled = true;
			this.cbVelicina.Location = new System.Drawing.Point(127, 69);
			this.cbVelicina.Name = "cbVelicina";
			this.cbVelicina.Size = new System.Drawing.Size(61, 21);
			this.cbVelicina.TabIndex = 5;
			this.cbVelicina.SelectedIndexChanged += new System.EventHandler(this.cbVelicina_SelectedIndexChanged);
			// 
			// FormPostavke
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(200, 146);
			this.Controls.Add(this.cbVelicina);
			this.Controls.Add(this.lblVelicina);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.cbJezik);
			this.Controls.Add(this.lblJezik);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormPostavke";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Postavke";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblJezik;
		private System.Windows.Forms.ComboBox cbJezik;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Label lblVelicina;
		private System.Windows.Forms.ComboBox cbVelicina;
	}
}