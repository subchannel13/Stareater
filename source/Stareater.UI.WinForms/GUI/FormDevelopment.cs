using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Data;
using Stareater.Localization;
using Stareater.Utils.NumberFormatters;

namespace Stareater.GUI
{
	public partial class FormDevelopment : Form
	{
		private GameController controller;
		private IList<TechnologyTopic> topics;
		
		public FormDevelopment()
		{
			InitializeComponent();
		}
		
		public FormDevelopment(GameController controller) : this()
		{
			this.controller = controller;
			this.topics = controller.DevelopmentTopics().ToList();
			updateList();
			updateDescription();
			
			if (controller.IsReadOnly) {
				reorderBottomAction.Enabled = false;
				reorderDownAction.Enabled = false;
				reorderTopAction.Enabled = false;
				reorderUpAction.Enabled = false;
				focusSlider.Enabled = false;
			}
			
			//TODO(v0.5): Get current focus intensity
			
			Context context = SettingsWinforms.Get.Language["FormTech"];
			this.Text = context["FormTitle"].Text();
			
			ThousandsFormatter formatter = new ThousandsFormatter();
			pointsInfo.Text = context["developmentPoints"].Text() + ": " + formatter.Format(controller.DevelopmentPoints);
			focusSlider.Maximum = controller.DevelopmentFocusOptions().Length - 1;
			focusSlider.Value = controller.DevelopmentFocusIndex;
		}
		
		private void updateList()
		{
			topicList.SuspendLayout();
			
			while (topicList.Controls.Count < topics.Count)
				topicList.Controls.Add(new TechnologyItem());
			while (topicList.Controls.Count > topics.Count)
				topicList.Controls.RemoveAt(topicList.Controls.Count - 1);

			for (int i = 0; i < topics.Count; i++)
				(topicList.Controls[i] as TechnologyItem).SetData(topics[i]);
			
			topicList.ResumeLayout();
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
		
		private void reorderTopic(int fromIndex, int toIndex)
		{
			if (toIndex < 0) toIndex = 0;
			if (toIndex >= topics.Count)	toIndex = topics.Count - 1;
			if (fromIndex == toIndex)
				return;
			
			var item = topics[fromIndex];
			
			topics.RemoveAt(fromIndex);
			
			if (toIndex < topics.Count)
				topics.Insert(toIndex, item);
			else
				topics.Add(item);
			
			updateList();
			topicList.SelectedIndex = toIndex;
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
		
		private void formDevelopment_FormClosed(object sender, FormClosedEventArgs e)
		{
			controller.ReorderDevelopmentTopics(topics.Select(x => x.IdCode));
		}
		
		private void topicList_SelectedIndexChanged(object sender, EventArgs e)
		{
			updateDescription();
		}
		
		private void reorderTopAction_Click(object sender, EventArgs e)
		{
			if (!topicList.HasSelection)
				return;
			
			reorderTopic(topicList.SelectedIndex, 0);
		}
		
		private void reorderUpAction_Click(object sender, EventArgs e)
		{
			if (!topicList.HasSelection)
				return;
			
			reorderTopic(topicList.SelectedIndex, topicList.SelectedIndex - 1);
		}
		
		private void reorderDownAction_Click(object sender, EventArgs e)
		{
			if (!topicList.HasSelection)
				return;
			
			reorderTopic(topicList.SelectedIndex, topicList.SelectedIndex + 1);
		}
		
		private void reorderBottomAction_Click(object sender, EventArgs e)
		{
			if (!topicList.HasSelection)
				return;
			
			reorderTopic(topicList.SelectedIndex, topics.Count);
		}
		
		private void focusSlider_Scroll(object sender, EventArgs e)
		{
			controller.DevelopmentFocusIndex = focusSlider.Value;
		}
	}
}
