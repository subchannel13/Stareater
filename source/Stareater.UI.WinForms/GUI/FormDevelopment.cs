using System;
using System.Drawing;
using System.Windows.Forms;
using Stareater.Controllers;

namespace Stareater.GUI
{
	public partial class FormDevelopment : Form
	{
		private GameController controller;
		
		public FormDevelopment()
		{
			InitializeComponent();
		}
		
		public FormDevelopment(GameController controller) : this()
		{
			this.controller = controller;
			
			foreach(var topic in controller.DevelopmentTopics())
				topicList.Controls.Add(new TechnologyItem(topic));
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
