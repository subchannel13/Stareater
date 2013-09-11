using System;
using System.Drawing;
using System.Linq;
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
			
			updateList();
		}
		
		private void updateList()
		{
			var topics = controller.DevelopmentTopics().ToArray();

			while (topicList.Controls.Count < topics.Length)
				topicList.Controls.Add(new TechnologyItem());
			while (topicList.Controls.Count > topics.Length)
				topicList.Controls.RemoveAt(topicList.Controls.Count - 1);

			for (int i = 0; i < topics.Length; i++)
				(topicList.Controls[i] as TechnologyItem).SetData(topics[i]);
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
