namespace Prototip
{
	partial class FormFlotaPokret
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
			this.lblPolaznaZvijezda = new System.Windows.Forms.Label();
			this.lblOdrediste = new System.Windows.Forms.Label();
			this.cbOdrediste = new System.Windows.Forms.ComboBox();
			this.btnPosalji = new System.Windows.Forms.Button();
			this.lblPridruzi = new System.Windows.Forms.Label();
			this.cbPridruzi = new System.Windows.Forms.ComboBox();
			this.lblNazivFlote = new System.Windows.Forms.Label();
			this.hscbKolicina = new System.Windows.Forms.HScrollBar();
			this.lblKolicina = new System.Windows.Forms.Label();
			this.txtKolicina = new System.Windows.Forms.TextBox();
			this.lblBrPoteza = new System.Windows.Forms.Label();
			this.lstBrodovi = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// lblPolaznaZvijezda
			// 
			this.lblPolaznaZvijezda.AutoSize = true;
			this.lblPolaznaZvijezda.Location = new System.Drawing.Point(12, 28);
			this.lblPolaznaZvijezda.Name = "lblPolaznaZvijezda";
			this.lblPolaznaZvijezda.Size = new System.Drawing.Size(62, 13);
			this.lblPolaznaZvijezda.TabIndex = 0;
			this.lblPolaznaZvijezda.Text = "Od zvijezde";
			// 
			// lblOdrediste
			// 
			this.lblOdrediste.AutoSize = true;
			this.lblOdrediste.Location = new System.Drawing.Point(169, 9);
			this.lblOdrediste.Name = "lblOdrediste";
			this.lblOdrediste.Size = new System.Drawing.Size(55, 13);
			this.lblOdrediste.TabIndex = 1;
			this.lblOdrediste.Text = "Odrediste:";
			// 
			// cbOdrediste
			// 
			this.cbOdrediste.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbOdrediste.FormattingEnabled = true;
			this.cbOdrediste.Location = new System.Drawing.Point(172, 25);
			this.cbOdrediste.Name = "cbOdrediste";
			this.cbOdrediste.Size = new System.Drawing.Size(122, 21);
			this.cbOdrediste.TabIndex = 2;
			this.cbOdrediste.SelectedIndexChanged += new System.EventHandler(this.cbOdrediste_SelectedIndexChanged);
			// 
			// btnPosalji
			// 
			this.btnPosalji.Location = new System.Drawing.Point(222, 212);
			this.btnPosalji.Name = "btnPosalji";
			this.btnPosalji.Size = new System.Drawing.Size(75, 23);
			this.btnPosalji.TabIndex = 4;
			this.btnPosalji.Text = "Pošalji";
			this.btnPosalji.UseVisualStyleBackColor = true;
			// 
			// lblPridruzi
			// 
			this.lblPridruzi.AutoSize = true;
			this.lblPridruzi.Location = new System.Drawing.Point(12, 198);
			this.lblPridruzi.Name = "lblPridruzi";
			this.lblPridruzi.Size = new System.Drawing.Size(63, 13);
			this.lblPridruzi.TabIndex = 5;
			this.lblPridruzi.Text = "Pridruzi floti:";
			// 
			// cbPridruzi
			// 
			this.cbPridruzi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPridruzi.FormattingEnabled = true;
			this.cbPridruzi.Location = new System.Drawing.Point(12, 214);
			this.cbPridruzi.Name = "cbPridruzi";
			this.cbPridruzi.Size = new System.Drawing.Size(121, 21);
			this.cbPridruzi.TabIndex = 6;
			// 
			// lblNazivFlote
			// 
			this.lblNazivFlote.AutoSize = true;
			this.lblNazivFlote.Location = new System.Drawing.Point(12, 9);
			this.lblNazivFlote.Name = "lblNazivFlote";
			this.lblNazivFlote.Size = new System.Drawing.Size(48, 13);
			this.lblNazivFlote.TabIndex = 7;
			this.lblNazivFlote.Text = "Flota xxx";
			// 
			// hscbKolicina
			// 
			this.hscbKolicina.LargeChange = 1;
			this.hscbKolicina.Location = new System.Drawing.Point(172, 146);
			this.hscbKolicina.Name = "hscbKolicina";
			this.hscbKolicina.Size = new System.Drawing.Size(125, 17);
			this.hscbKolicina.TabIndex = 8;
			this.hscbKolicina.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hscbKolicina_Scroll);
			// 
			// lblKolicina
			// 
			this.lblKolicina.AutoSize = true;
			this.lblKolicina.Location = new System.Drawing.Point(175, 133);
			this.lblKolicina.Name = "lblKolicina";
			this.lblKolicina.Size = new System.Drawing.Size(47, 13);
			this.lblKolicina.TabIndex = 9;
			this.lblKolicina.Text = "Kolicina:";
			// 
			// txtKolicina
			// 
			this.txtKolicina.Location = new System.Drawing.Point(218, 166);
			this.txtKolicina.Name = "txtKolicina";
			this.txtKolicina.Size = new System.Drawing.Size(79, 20);
			this.txtKolicina.TabIndex = 10;
			this.txtKolicina.TextChanged += new System.EventHandler(this.txtKolicina_TextChanged);
			// 
			// lblBrPoteza
			// 
			this.lblBrPoteza.AutoSize = true;
			this.lblBrPoteza.Location = new System.Drawing.Point(175, 54);
			this.lblBrPoteza.Name = "lblBrPoteza";
			this.lblBrPoteza.Size = new System.Drawing.Size(52, 13);
			this.lblBrPoteza.TabIndex = 11;
			this.lblBrPoteza.Text = "xx poteza";
			// 
			// lstBrodovi
			// 
			this.lstBrodovi.FormattingEnabled = true;
			this.lstBrodovi.Location = new System.Drawing.Point(12, 44);
			this.lstBrodovi.Name = "lstBrodovi";
			this.lstBrodovi.Size = new System.Drawing.Size(154, 147);
			this.lstBrodovi.TabIndex = 12;
			this.lstBrodovi.SelectedIndexChanged += new System.EventHandler(this.lstBrodovi_SelectedIndexChanged);
			// 
			// FormFlotaPokret
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(306, 255);
			this.Controls.Add(this.lstBrodovi);
			this.Controls.Add(this.lblBrPoteza);
			this.Controls.Add(this.txtKolicina);
			this.Controls.Add(this.lblKolicina);
			this.Controls.Add(this.hscbKolicina);
			this.Controls.Add(this.lblNazivFlote);
			this.Controls.Add(this.cbPridruzi);
			this.Controls.Add(this.lblPridruzi);
			this.Controls.Add(this.btnPosalji);
			this.Controls.Add(this.cbOdrediste);
			this.Controls.Add(this.lblOdrediste);
			this.Controls.Add(this.lblPolaznaZvijezda);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormFlotaPokret";
			this.Text = "Usmjeravanje flote";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFlotaPokret_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblPolaznaZvijezda;
		private System.Windows.Forms.Label lblOdrediste;
		private System.Windows.Forms.ComboBox cbOdrediste;
		private System.Windows.Forms.Button btnPosalji;
		private System.Windows.Forms.Label lblPridruzi;
		private System.Windows.Forms.ComboBox cbPridruzi;
		private System.Windows.Forms.Label lblNazivFlote;
		private System.Windows.Forms.HScrollBar hscbKolicina;
		private System.Windows.Forms.Label lblKolicina;
		private System.Windows.Forms.TextBox txtKolicina;
		private System.Windows.Forms.Label lblBrPoteza;
		private System.Windows.Forms.ListBox lstBrodovi;
	}
}