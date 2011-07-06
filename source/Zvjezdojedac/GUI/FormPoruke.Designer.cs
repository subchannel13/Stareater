namespace Zvjezdojedac.GUI
{
	partial class FormPoruke
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPoruke));
			this.lstvPoruke = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chTipBrod = new System.Windows.Forms.CheckBox();
			this.chTipKolonija = new System.Windows.Forms.CheckBox();
			this.chTipTehnologije = new System.Windows.Forms.CheckBox();
			this.chTipZgrade = new System.Windows.Forms.CheckBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lstvPoruke
			// 
			this.lstvPoruke.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this.lstvPoruke.FullRowSelect = true;
			this.lstvPoruke.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lstvPoruke.Location = new System.Drawing.Point(12, 12);
			this.lstvPoruke.MultiSelect = false;
			this.lstvPoruke.Name = "lstvPoruke";
			this.lstvPoruke.Size = new System.Drawing.Size(340, 166);
			this.lstvPoruke.TabIndex = 0;
			this.lstvPoruke.UseCompatibleStateImageBehavior = false;
			this.lstvPoruke.View = System.Windows.Forms.View.List;
			this.lstvPoruke.ItemActivate += new System.EventHandler(this.lstvPoruke_ItemActivate);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Naslov";
			this.columnHeader1.Width = 600;
			// 
			// chTipBrod
			// 
			this.chTipBrod.AutoSize = true;
			this.chTipBrod.Location = new System.Drawing.Point(12, 184);
			this.chTipBrod.Name = "chTipBrod";
			this.chTipBrod.Size = new System.Drawing.Size(86, 17);
			this.chTipBrod.TabIndex = 1;
			this.chTipBrod.Text = "Novi brodovi";
			this.chTipBrod.UseVisualStyleBackColor = true;
			this.chTipBrod.CheckedChanged += new System.EventHandler(this.chTipBrod_CheckedChanged);
			// 
			// chTipKolonija
			// 
			this.chTipKolonija.AutoSize = true;
			this.chTipKolonija.Location = new System.Drawing.Point(12, 207);
			this.chTipKolonija.Name = "chTipKolonija";
			this.chTipKolonija.Size = new System.Drawing.Size(91, 17);
			this.chTipKolonija.TabIndex = 2;
			this.chTipKolonija.Text = "Nove kolonije";
			this.chTipKolonija.UseVisualStyleBackColor = true;
			this.chTipKolonija.CheckedChanged += new System.EventHandler(this.chTipKolonija_CheckedChanged);
			// 
			// chTipTehnologije
			// 
			this.chTipTehnologije.AutoSize = true;
			this.chTipTehnologije.Location = new System.Drawing.Point(12, 230);
			this.chTipTehnologije.Name = "chTipTehnologije";
			this.chTipTehnologije.Size = new System.Drawing.Size(106, 17);
			this.chTipTehnologije.TabIndex = 3;
			this.chTipTehnologije.Text = "Nove tehnologije";
			this.chTipTehnologije.UseVisualStyleBackColor = true;
			this.chTipTehnologije.CheckedChanged += new System.EventHandler(this.chTipTehnologije_CheckedChanged);
			// 
			// chTipZgrade
			// 
			this.chTipZgrade.AutoSize = true;
			this.chTipZgrade.Location = new System.Drawing.Point(11, 253);
			this.chTipZgrade.Name = "chTipZgrade";
			this.chTipZgrade.Size = new System.Drawing.Size(87, 17);
			this.chTipZgrade.TabIndex = 4;
			this.chTipZgrade.Text = "Nove zgrade";
			this.chTipZgrade.UseVisualStyleBackColor = true;
			this.chTipZgrade.CheckedChanged += new System.EventHandler(this.chTipZgrade_CheckedChanged);
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnOk.Location = new System.Drawing.Point(277, 279);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 5;
			this.btnOk.Text = "Zatvori";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// FormPoruke
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnOk;
			this.ClientSize = new System.Drawing.Size(364, 314);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.chTipZgrade);
			this.Controls.Add(this.chTipTehnologije);
			this.Controls.Add(this.chTipKolonija);
			this.Controls.Add(this.chTipBrod);
			this.Controls.Add(this.lstvPoruke);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormPoruke";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Poruke";
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormPoruke_KeyPress);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView lstvPoruke;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.CheckBox chTipBrod;
		private System.Windows.Forms.CheckBox chTipKolonija;
		private System.Windows.Forms.CheckBox chTipTehnologije;
		private System.Windows.Forms.CheckBox chTipZgrade;
		private System.Windows.Forms.Button btnOk;

	}
}