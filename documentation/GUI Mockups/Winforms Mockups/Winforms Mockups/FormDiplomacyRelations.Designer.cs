/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 21.3.2017.
 * Time: 14:59
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Winforms_Mockups
{
	partial class FormDiplomacyRelations
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private Winforms_Mockups.RelationsPlayerInfo relationsPlayerInfo1;
		private Winforms_Mockups.RelationsPlayerInfo relationsPlayerInfo2;
		private System.Windows.Forms.Button button1;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.relationsPlayerInfo1 = new Winforms_Mockups.RelationsPlayerInfo();
			this.relationsPlayerInfo2 = new Winforms_Mockups.RelationsPlayerInfo();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// relationsPlayerInfo1
			// 
			this.relationsPlayerInfo1.Location = new System.Drawing.Point(12, 12);
			this.relationsPlayerInfo1.Name = "relationsPlayerInfo1";
			this.relationsPlayerInfo1.Size = new System.Drawing.Size(150, 75);
			this.relationsPlayerInfo1.TabIndex = 0;
			// 
			// relationsPlayerInfo2
			// 
			this.relationsPlayerInfo2.Location = new System.Drawing.Point(12, 93);
			this.relationsPlayerInfo2.Name = "relationsPlayerInfo2";
			this.relationsPlayerInfo2.Size = new System.Drawing.Size(150, 75);
			this.relationsPlayerInfo2.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.Image = global::Winforms_Mockups.Properties.Resources.message;
			this.button1.Location = new System.Drawing.Point(179, 207);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(93, 43);
			this.button1.TabIndex = 2;
			this.button1.Text = "Audience";
			this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.button1.UseVisualStyleBackColor = true;
			// 
			// FormDiplomacyRelations
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.relationsPlayerInfo2);
			this.Controls.Add(this.relationsPlayerInfo1);
			this.Name = "FormDiplomacyRelations";
			this.Text = "FormDiplomacyRelations";
			this.ResumeLayout(false);

		}
	}
}
