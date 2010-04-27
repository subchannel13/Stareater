namespace Zvjezdojedac_editori
{
	partial class FormPreduvjeti
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
			this.lstvPreduvjeti = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.label1 = new System.Windows.Forms.Label();
			this.cbTehno = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtNivo = new System.Windows.Forms.TextBox();
			this.lblNivoGreska = new System.Windows.Forms.Label();
			this.btnDodaj = new System.Windows.Forms.Button();
			this.btnUkloni = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lstvPreduvjeti
			// 
			this.lstvPreduvjeti.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.lstvPreduvjeti.FullRowSelect = true;
			this.lstvPreduvjeti.GridLines = true;
			this.lstvPreduvjeti.HideSelection = false;
			this.lstvPreduvjeti.Location = new System.Drawing.Point(12, 12);
			this.lstvPreduvjeti.MultiSelect = false;
			this.lstvPreduvjeti.Name = "lstvPreduvjeti";
			this.lstvPreduvjeti.Size = new System.Drawing.Size(290, 96);
			this.lstvPreduvjeti.TabIndex = 0;
			this.lstvPreduvjeti.UseCompatibleStateImageBehavior = false;
			this.lstvPreduvjeti.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Tehnologija";
			this.columnHeader1.Width = 123;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Nivo";
			this.columnHeader2.Width = 149;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 115);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(65, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "&Tehnologija:";
			// 
			// cbTehno
			// 
			this.cbTehno.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbTehno.FormattingEnabled = true;
			this.cbTehno.Location = new System.Drawing.Point(12, 131);
			this.cbTehno.Name = "cbTehno";
			this.cbTehno.Size = new System.Drawing.Size(126, 21);
			this.cbTehno.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 159);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "&Nivo:";
			// 
			// txtNivo
			// 
			this.txtNivo.Location = new System.Drawing.Point(12, 175);
			this.txtNivo.Name = "txtNivo";
			this.txtNivo.Size = new System.Drawing.Size(270, 20);
			this.txtNivo.TabIndex = 5;
			// 
			// lblNivoGreska
			// 
			this.lblNivoGreska.AutoSize = true;
			this.lblNivoGreska.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.lblNivoGreska.ForeColor = System.Drawing.Color.Red;
			this.lblNivoGreska.Location = new System.Drawing.Point(288, 178);
			this.lblNivoGreska.Name = "lblNivoGreska";
			this.lblNivoGreska.Size = new System.Drawing.Size(16, 13);
			this.lblNivoGreska.TabIndex = 4;
			this.lblNivoGreska.Text = "*!";
			// 
			// btnDodaj
			// 
			this.btnDodaj.Location = new System.Drawing.Point(227, 114);
			this.btnDodaj.Name = "btnDodaj";
			this.btnDodaj.Size = new System.Drawing.Size(75, 23);
			this.btnDodaj.TabIndex = 6;
			this.btnDodaj.Text = "&Dodaj";
			this.btnDodaj.UseVisualStyleBackColor = true;
			// 
			// btnUkloni
			// 
			this.btnUkloni.Location = new System.Drawing.Point(227, 143);
			this.btnUkloni.Name = "btnUkloni";
			this.btnUkloni.Size = new System.Drawing.Size(75, 23);
			this.btnUkloni.TabIndex = 7;
			this.btnUkloni.Text = "&Ukloni";
			this.btnUkloni.UseVisualStyleBackColor = true;
			this.btnUkloni.Click += new System.EventHandler(this.btnUkloni_Click);
			// 
			// btnOk
			// 
			this.btnOk.Location = new System.Drawing.Point(12, 227);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 8;
			this.btnOk.Text = "Prihvati";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(229, 227);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 9;
			this.btnCancel.Text = "Odustani";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// FormPreduvjeti
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(314, 262);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnUkloni);
			this.Controls.Add(this.btnDodaj);
			this.Controls.Add(this.lblNivoGreska);
			this.Controls.Add(this.txtNivo);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cbTehno);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lstvPreduvjeti);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormPreduvjeti";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Preduvjeti";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView lstvPreduvjeti;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbTehno;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtNivo;
		private System.Windows.Forms.Label lblNivoGreska;
		private System.Windows.Forms.Button btnDodaj;
		private System.Windows.Forms.Button btnUkloni;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
	}
}