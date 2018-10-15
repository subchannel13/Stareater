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
			this.designPanel = new System.Windows.Forms.Panel();
			this.selectColonizerAction = new System.Windows.Forms.Button();
			this.colonizerDesignText = new System.Windows.Forms.Label();
			this.designPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// projectList
			// 
			this.projectList.AutoScroll = true;
			this.projectList.ColumnCount = 1;
			this.projectList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.projectList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.projectList.Location = new System.Drawing.Point(0, 54);
			this.projectList.Name = "projectList";
			this.projectList.Padding = new System.Windows.Forms.Padding(0, 0, 25, 0);
			this.projectList.RowCount = 1;
			this.projectList.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.projectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 208F));
			this.projectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 208F));
			this.projectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 208F));
			this.projectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 208F));
			this.projectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 208F));
			this.projectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 208F));
			this.projectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 208F));
			this.projectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 208F));
			this.projectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 208F));
			this.projectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 208F));
			this.projectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 208F));
			this.projectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 208F));
			this.projectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 208F));
			this.projectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 208F));
			this.projectList.Size = new System.Drawing.Size(284, 208);
			this.projectList.TabIndex = 0;
			// 
			// designPanel
			// 
			this.designPanel.Controls.Add(this.selectColonizerAction);
			this.designPanel.Controls.Add(this.colonizerDesignText);
			this.designPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.designPanel.Location = new System.Drawing.Point(0, 0);
			this.designPanel.Name = "designPanel";
			this.designPanel.Size = new System.Drawing.Size(284, 54);
			this.designPanel.TabIndex = 1;
			// 
			// selectColonizerAction
			// 
			this.selectColonizerAction.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.selectColonizerAction.Location = new System.Drawing.Point(148, 9);
			this.selectColonizerAction.Name = "selectColonizerAction";
			this.selectColonizerAction.Size = new System.Drawing.Size(124, 30);
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
			this.colonizerDesignText.Size = new System.Drawing.Size(130, 33);
			this.colonizerDesignText.TabIndex = 0;
			this.colonizerDesignText.Text = "Colony ship design:";
			this.colonizerDesignText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormColonization
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.projectList);
			this.Controls.Add(this.designPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.Name = "FormColonization";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormColonization";
			this.designPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		private System.Windows.Forms.Panel designPanel;
		private System.Windows.Forms.Button selectColonizerAction;
		private System.Windows.Forms.Label colonizerDesignText;
	}
}
