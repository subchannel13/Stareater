namespace Prototip
{
	partial class frmPoruke
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPoruke));
			this.lstvPoruke = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
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
			// frmPoruke
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(364, 314);
			this.Controls.Add(this.lstvPoruke);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmPoruke";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Poruke";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView lstvPoruke;
		private System.Windows.Forms.ColumnHeader columnHeader1;

	}
}