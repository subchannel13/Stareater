/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 24.9.2015.
 * Time: 15:43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class FormColonization
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TableLayoutPanel projectList;
		
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
			this.projectList = new System.Windows.Forms.TableLayoutPanel();
			this.selectColonizerAction = new System.Windows.Forms.Button();
			this.colonizerDesignText = new System.Windows.Forms.Label();
			this.capacityInput = new System.Windows.Forms.TextBox();
			this.capacityText = new System.Windows.Forms.Label();
			this.shipyardList = new System.Windows.Forms.TableLayoutPanel();
			this.shipyardListTitle = new System.Windows.Forms.Label();
			this.projectListTitle = new System.Windows.Forms.Label();
			this.addSourceAction = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// projectList
			// 
			this.projectList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.projectList.AutoScroll = true;
			this.projectList.ColumnCount = 1;
			this.projectList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.projectList.Location = new System.Drawing.Point(12, 73);
			this.projectList.Name = "projectList";
			this.projectList.Padding = new System.Windows.Forms.Padding(0, 0, 25, 0);
			this.projectList.RowCount = 1;
			this.projectList.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.projectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 276F));
			this.projectList.Size = new System.Drawing.Size(281, 276);
			this.projectList.TabIndex = 0;
			// 
			// selectColonizerAction
			// 
			this.selectColonizerAction.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.selectColonizerAction.Location = new System.Drawing.Point(148, 9);
			this.selectColonizerAction.Name = "selectColonizerAction";
			this.selectColonizerAction.Size = new System.Drawing.Size(124, 36);
			this.selectColonizerAction.TabIndex = 1;
			this.selectColonizerAction.Text = "button1";
			this.selectColonizerAction.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.selectColonizerAction.UseVisualStyleBackColor = true;
			this.selectColonizerAction.Click += new System.EventHandler(this.selectColonizerAction_Click);
			// 
			// colonizerDesignText
			// 
			this.colonizerDesignText.Location = new System.Drawing.Point(12, 9);
			this.colonizerDesignText.Name = "colonizerDesignText";
			this.colonizerDesignText.Size = new System.Drawing.Size(130, 36);
			this.colonizerDesignText.TabIndex = 0;
			this.colonizerDesignText.Text = "Colony ship design:";
			this.colonizerDesignText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// capacityInput
			// 
			this.capacityInput.Location = new System.Drawing.Point(421, 18);
			this.capacityInput.Name = "capacityInput";
			this.capacityInput.Size = new System.Drawing.Size(80, 20);
			this.capacityInput.TabIndex = 7;
			this.capacityInput.Text = "820 k";
			// 
			// capacityText
			// 
			this.capacityText.Location = new System.Drawing.Point(295, 9);
			this.capacityText.Margin = new System.Windows.Forms.Padding(20, 0, 3, 0);
			this.capacityText.Name = "capacityText";
			this.capacityText.Size = new System.Drawing.Size(120, 36);
			this.capacityText.TabIndex = 6;
			this.capacityText.Text = "Target transport capacity:";
			this.capacityText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// shipyardList
			// 
			this.shipyardList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.shipyardList.AutoScroll = true;
			this.shipyardList.ColumnCount = 1;
			this.shipyardList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.shipyardList.Location = new System.Drawing.Point(319, 73);
			this.shipyardList.Name = "shipyardList";
			this.shipyardList.Padding = new System.Windows.Forms.Padding(0, 0, 25, 0);
			this.shipyardList.RowCount = 1;
			this.shipyardList.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.shipyardList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 276F));
			this.shipyardList.Size = new System.Drawing.Size(223, 230);
			this.shipyardList.TabIndex = 1;
			// 
			// shipyardListTitle
			// 
			this.shipyardListTitle.AutoSize = true;
			this.shipyardListTitle.Location = new System.Drawing.Point(316, 57);
			this.shipyardListTitle.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
			this.shipyardListTitle.Name = "shipyardListTitle";
			this.shipyardListTitle.Size = new System.Drawing.Size(92, 13);
			this.shipyardListTitle.TabIndex = 9;
			this.shipyardListTitle.Text = "Colony ship yards:";
			// 
			// projectListTitle
			// 
			this.projectListTitle.AutoSize = true;
			this.projectListTitle.Location = new System.Drawing.Point(12, 57);
			this.projectListTitle.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
			this.projectListTitle.Name = "projectListTitle";
			this.projectListTitle.Size = new System.Drawing.Size(99, 13);
			this.projectListTitle.TabIndex = 8;
			this.projectListTitle.Text = "Planets to colonize:";
			// 
			// addSourceAction
			// 
			this.addSourceAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.addSourceAction.Image = global::Stareater.Properties.Resources.add;
			this.addSourceAction.Location = new System.Drawing.Point(319, 309);
			this.addSourceAction.Name = "addSourceAction";
			this.addSourceAction.Size = new System.Drawing.Size(40, 40);
			this.addSourceAction.TabIndex = 10;
			this.addSourceAction.UseVisualStyleBackColor = true;
			this.addSourceAction.Click += new System.EventHandler(this.addSourceAction_Click);
			// 
			// FormColonization
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(554, 361);
			this.Controls.Add(this.addSourceAction);
			this.Controls.Add(this.shipyardListTitle);
			this.Controls.Add(this.projectListTitle);
			this.Controls.Add(this.shipyardList);
			this.Controls.Add(this.capacityInput);
			this.Controls.Add(this.capacityText);
			this.Controls.Add(this.selectColonizerAction);
			this.Controls.Add(this.projectList);
			this.Controls.Add(this.colonizerDesignText);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.Name = "FormColonization";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormColonization";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private System.Windows.Forms.Button selectColonizerAction;
		private System.Windows.Forms.Label colonizerDesignText;
		private System.Windows.Forms.TextBox capacityInput;
		private System.Windows.Forms.Label capacityText;
		private System.Windows.Forms.TableLayoutPanel shipyardList;
		private System.Windows.Forms.Label shipyardListTitle;
		private System.Windows.Forms.Label projectListTitle;
		private System.Windows.Forms.Button addSourceAction;
	}
}
