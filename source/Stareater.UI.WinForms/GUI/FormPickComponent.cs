using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Stareater.AppData;

namespace Stareater.GUI
{
	public partial class FormPickComponent : Form
	{
		public IShipComponent Choice { get; private set; }
		
		private readonly Dictionary<object, IShipComponent> buttonResult = new Dictionary<object, IShipComponent>();
		
		public FormPickComponent()
		{
			InitializeComponent();
		}
		
		public FormPickComponent(IEnumerable<IShipComponent> components) : this()
		{
			this.Choice = null;
			
			foreach(var component in components)
			{
				var button = new Button();
				if (component.ImagePath != null)
					button.Image = ImageCache.Get[component.ImagePath]; //TODO(v0.5) shirnk image
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
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void onSelect(object sender, EventArgs e)
		{
			this.Choice = buttonResult[sender];
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
