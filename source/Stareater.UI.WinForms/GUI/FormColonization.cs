using System;
using System.Drawing;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;

namespace Stareater.GUI
{
	public partial class FormColonization : Form
	{
		private readonly GameController controller;
		
		public FormColonization()
		{
			InitializeComponent();
		}
		
		public FormColonization(GameController controller) : this()
		{
			this.controller = controller;
			
			foreach (var data in controller.ColonizationProjects()) {
				var itemView = new ColonizationTargetView(data, controller);
				projectList.Controls.Add(itemView);
			}
			
			var context = SettingsWinforms.Get.Language["FormColonization"];
			this.Text = context["title"].Text();
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
