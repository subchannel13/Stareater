using System;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers.Views;
using Stareater.Localization;
using Stareater.Utils.Collections;

namespace Stareater.GUI
{
	public sealed partial class FormResearchComplete : Form
	{
		private readonly ResearchCompleteController controller;
		
		public FormResearchComplete()
		{
			InitializeComponent();
		}
		
		public FormResearchComplete(ResearchCompleteController controller) : this()
		{
			this.controller = controller;
			var context = LocalizationManifest.Get.CurrentLanguage["FormResearchComplete"];
			var topic = this.controller.TopicInfo;
			
			this.Text = context["FormTitle"].Text();
			this.Font = SettingsWinforms.Get.FormFont;
			this.acceptButton.Text = context["doneButton"].Text();
			this.priorityTitle.Text = context["priorityTitle"].Text();
			
			this.topicThumbnail.Image = ImageCache.Get[topic.ImagePath];
			this.topicName.Text = topic.Name;
			this.levelLabel.Text = context["Level"].Text(new TextVar("lvl", topic.NextLevel.ToString()).Get);
			this.topicDescription.Text = topic.Description;
			
			this.updateList();
			
			if (topic.Unlocks.Length > 0)
				unlockedList.SelectedIndex = 0;
			
			updateDescription(unlockedList.SelectedItem);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void reorderSelection(int direction)
		{
			if (!unlockedList.HasSelection)
				return;
			
			var unlock = (unlockedList.SelectedItem as DevelopmentItem).Data;
			this.controller.SetPriority(unlock, unlockedList.SelectedIndex + direction);
			this.updateList();
			this.unlockedList.SelectedIndex = this.controller.UnlockPriorities.ToList().IndexOf(unlock);
		}
		
		private void updateDescription(Control topic)
		{
			if (topic == null) {
				techDescription.Text = "";
			} else {
				var selection = topic as DevelopmentItem;
				
				techDescription.Text = selection.Data.Description;
			}
		}
		
		private void updateList()
		{
			var unlocks = this.controller.UnlockPriorities.ToList();
			unlockedList.SuspendLayout();
			
			while (unlockedList.Controls.Count < unlocks.Count) {
				var topicControl = new DevelopmentItem();
				topicControl.MouseEnter += unlock_OnMouseEnter;
				topicControl.MouseLeave += unlock_OnMouseLeave;
				unlockedList.Controls.Add(topicControl);
			}

			for (int i = 0; i < unlocks.Count; i++)
				(unlockedList.Controls[i] as DevelopmentItem).SetData(unlocks[i]);
			
			unlockedList.ResumeLayout();
		}
		
		private void FormResearchComplete_FormClosed(object sender, FormClosedEventArgs e)
		{
			//TODO(v0.6) allow closing only through "confirm" button
			this.controller.Done();
		}

		private void unlock_OnMouseEnter(object sender, EventArgs e)
		{
			updateDescription(sender as Control);
		}
		
		private void unlock_OnMouseLeave(object sender, EventArgs e)
		{
			updateDescription(unlockedList.SelectedItem);
		}
		
		private void acceptButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void reorderUpAction_Click(object sender, EventArgs e)
		{
			reorderSelection(-1);
		}

		private void reorderDownAction_Click(object sender, EventArgs e)
		{
			reorderSelection(1);
		}
	}
}
