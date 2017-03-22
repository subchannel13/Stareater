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
		private System.Windows.Forms.Button button1;
		private My_ListView.ControlListView controlListView1;
		private Winforms_Mockups.RelationsPlayerInfo relationsPlayerInfo1;
		private Winforms_Mockups.RelationsPlayerInfo relationsPlayerInfo2;
		private Winforms_Mockups.RelationsPlayerInfo relationsPlayerInfo3;
		private Winforms_Mockups.RelationsPlayerInfo relationsPlayerInfo5;
		private Winforms_Mockups.RelationsPlayerInfo relationsPlayerInfo4;
		
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
			this.button1 = new System.Windows.Forms.Button();
			this.controlListView1 = new My_ListView.ControlListView();
			this.relationsPlayerInfo1 = new Winforms_Mockups.RelationsPlayerInfo();
			this.relationsPlayerInfo2 = new Winforms_Mockups.RelationsPlayerInfo();
			this.relationsPlayerInfo3 = new Winforms_Mockups.RelationsPlayerInfo();
			this.relationsPlayerInfo4 = new Winforms_Mockups.RelationsPlayerInfo();
			this.relationsPlayerInfo5 = new Winforms_Mockups.RelationsPlayerInfo();
			this.controlListView1.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Image = global::Winforms_Mockups.Properties.Resources.message;
			this.button1.Location = new System.Drawing.Point(402, 232);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(93, 43);
			this.button1.TabIndex = 2;
			this.button1.Text = "Audience";
			this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.button1.UseVisualStyleBackColor = true;
			// 
			// controlListView1
			// 
			this.controlListView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left)));
			this.controlListView1.AutoScroll = true;
			this.controlListView1.Controls.Add(this.relationsPlayerInfo1);
			this.controlListView1.Controls.Add(this.relationsPlayerInfo2);
			this.controlListView1.Controls.Add(this.relationsPlayerInfo3);
			this.controlListView1.Controls.Add(this.relationsPlayerInfo5);
			this.controlListView1.Controls.Add(this.relationsPlayerInfo4);
			this.controlListView1.Location = new System.Drawing.Point(12, 12);
			this.controlListView1.Name = "controlListView1";
			this.controlListView1.Size = new System.Drawing.Size(350, 263);
			this.controlListView1.TabIndex = 3;
			// 
			// relationsPlayerInfo1
			// 
			this.relationsPlayerInfo1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.relationsPlayerInfo1.Location = new System.Drawing.Point(3, 3);
			this.relationsPlayerInfo1.Name = "relationsPlayerInfo1";
			this.relationsPlayerInfo1.Size = new System.Drawing.Size(160, 120);
			this.relationsPlayerInfo1.TabIndex = 0;
			// 
			// relationsPlayerInfo2
			// 
			this.relationsPlayerInfo2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.relationsPlayerInfo2.Location = new System.Drawing.Point(169, 3);
			this.relationsPlayerInfo2.Name = "relationsPlayerInfo2";
			this.relationsPlayerInfo2.Size = new System.Drawing.Size(160, 120);
			this.relationsPlayerInfo2.TabIndex = 1;
			// 
			// relationsPlayerInfo3
			// 
			this.relationsPlayerInfo3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.relationsPlayerInfo3.Location = new System.Drawing.Point(3, 129);
			this.relationsPlayerInfo3.Name = "relationsPlayerInfo3";
			this.relationsPlayerInfo3.Size = new System.Drawing.Size(160, 120);
			this.relationsPlayerInfo3.TabIndex = 2;
			// 
			// relationsPlayerInfo4
			// 
			this.relationsPlayerInfo4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.relationsPlayerInfo4.Location = new System.Drawing.Point(3, 255);
			this.relationsPlayerInfo4.Name = "relationsPlayerInfo4";
			this.relationsPlayerInfo4.Size = new System.Drawing.Size(160, 120);
			this.relationsPlayerInfo4.TabIndex = 3;
			// 
			// relationsPlayerInfo5
			// 
			this.relationsPlayerInfo5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.relationsPlayerInfo5.Location = new System.Drawing.Point(169, 129);
			this.relationsPlayerInfo5.Name = "relationsPlayerInfo5";
			this.relationsPlayerInfo5.Size = new System.Drawing.Size(160, 120);
			this.relationsPlayerInfo5.TabIndex = 4;
			// 
			// FormDiplomacyRelations
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(507, 287);
			this.Controls.Add(this.controlListView1);
			this.Controls.Add(this.button1);
			this.Name = "FormDiplomacyRelations";
			this.Text = "FormDiplomacyRelations";
			this.controlListView1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
