using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
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
			updateDescription();
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
		
		private void updateDescription()
		{
			if (topicList.SelectedItem == null) {
				techImage.Image = null;
				techName.Text = "";
				techDescription.Text = "";
				techLevel.Text = "";
			} else {
				var selection = topicList.SelectedItem as TechnologyItem;
				
				techImage.Image = ImageCache.Get[selection.Data.ImagePath];
				techName.Text = selection.Data.Name;
				techDescription.Text = selection.Data.Description;
				techLevel.Text = selection.TopicLevelText;
			}
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void formDevelopment_Load(object sender, EventArgs e)
		{
			if (topicList.Controls.Count > 0)
				topicList.SelectedIndex = 0;
		}
		
		private void topicList_SelectedIndexChanged(object sender, EventArgs e)
		{
			updateDescription();
		}
	}
}
