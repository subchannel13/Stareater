using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.GUI.ShipDesigns;

namespace Stareater.GUI
{
	public sealed partial class FormPickComponent : Form
	{
		public IShipComponentType Choice { get; private set; }
		
		private readonly Dictionary<object, IShipComponentType> buttonResult = new Dictionary<object, IShipComponentType>();
		
		public FormPickComponent()
		{
			InitializeComponent();
		}
		
		public FormPickComponent(string title, IEnumerable<IShipComponentType> components1, string group2Title = null, IEnumerable<IShipComponentType> components2 = null) : this()
		{
			this.Text = title;
			this.Choice = null;
			
			populateWhith(components1);
			
			if (components1.Any() && components2 != null && components2.Any())
			{
				this.componentPanel.SetFlowBreak(this.componentPanel.Controls[this.componentPanel.Controls.Count - 1], true);
				
				var groupTitle = new Label();
				var dummy = new Label();
				this.componentPanel.Controls.Add(groupTitle);
				this.componentPanel.Controls.Add(dummy);
				
				groupTitle.AutoSize = true;
				this.componentPanel.SetFlowBreak(groupTitle, true);
				groupTitle.Margin = new Padding(3, 6, 3, 6);
				groupTitle.Name = "groupTitle";
				groupTitle.Size = new System.Drawing.Size(149, 13);
				groupTitle.Text = group2Title;
				
				dummy.Margin = new Padding(0);
				dummy.Name = "dummy";
				dummy.Size = new System.Drawing.Size(0, 0);
			}
			
			if (components2 != null)
				populateWhith(components2);
			
			this.Font = SettingsWinforms.Get.FormFont;
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void populateWhith(IEnumerable<IShipComponentType> options)
		{
			foreach(var component in options)
			{
				var button = new Button();
				if (component.Image != null)
					button.Image = component.Image; //TODO(v0.6) shirnk image
				button.Size = new System.Drawing.Size(80, 80);
				button.Text = component.Name;
				button.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
				button.TextImageRelation = TextImageRelation.ImageAboveText;
				button.UseVisualStyleBackColor = true;
				button.Click += onSelect;
				
				buttonResult.Add(button, component);
				this.componentPanel.Controls.Add(button);
			}
		}
		
		private void onSelect(object sender, EventArgs e)
		{
			buttonResult[sender].Dispatch();
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
