using System;
using System.Linq;
using System.Windows.Forms;

using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Localization;

namespace Stareater.GUI
{
	public sealed partial class FormBuildingQueue : Form
	{
		private readonly AConstructionSiteController controller;
		
		public FormBuildingQueue()
		{
			InitializeComponent();
		}
		
		public FormBuildingQueue(AConstructionSiteController controller) : this()
		{
			this.controller = controller;
			
			if (controller.IsReadOnly) {
				moveDownButton.Enabled = false;
				moveUpButton.Enabled = false;
				removeButton.Enabled = false;
			}
			
			foreach (var data in controller.ConstructableItems) {
				var itemView = new ConstructableItemView();
				itemView.Data = data;
				itemView.Enabled = controller.CanPick(data);
				
				if (!controller.IsReadOnly)
					itemView.Click += onOption_Click;
				itemView.MouseEnter += onOption_MouseEnter;
				
				optionList.Controls.Add(itemView);
			}
			
			foreach (var data in controller.ConstructionQueue) {
				var queueItemView = new QueuedConstructionView();
				queueItemView.Data = data;
				queueList.Controls.Add(queueItemView);
			}
			
			Context context = SettingsWinforms.Get.Language["FormBuilding"];
			
			this.Text = context["FormTitle"].Text();
			this.Font = SettingsWinforms.Get.FormFont;
			optionsLabel.Text = context["optionsTitle"].Text();
			queueLabel.Text = context["queueTitle"].Text();
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		private void reorderItem(int fromIndex, int toIndex)
		{
			if (toIndex < 0) toIndex = 0;
			if (toIndex >= queueList.Controls.Count) toIndex = queueList.Controls.Count - 1;
			if (fromIndex == toIndex ||
			    (queueList.Controls[fromIndex] as QueuedConstructionView).Data.IsVirtual ||
			    (queueList.Controls[toIndex] as QueuedConstructionView).Data.IsVirtual)
				return;
			
			controller.ReorderQueue(fromIndex, toIndex);
			
			var item = queueList.Controls[fromIndex];
			
			queueList.SelectedIndex = ControlListView.NoneSelected;
			queueList.Controls.SetChildIndex(item, toIndex);
			queueList.SelectedIndex = toIndex;
		}
		
		private void updateDescription(ConstructableItemView itemView)
		{
			thumbnailImage.Image = ImageCache.Get[itemView.Data.ImagePath];
			descriptionLabel.Text = itemView.Data.Description;
		}
		
		private void updateOptions()
		{
			foreach(var optionView in optionList.Controls){
				var itemView = optionView as ConstructableItemView;
				itemView.Enabled = controller.CanPick(itemView.Data);
			}				
		}
		
		private void updateQueue()
		{
			var queueData = controller.ConstructionQueue.ToList();
			
			for (int i = 0; i < queueData.Count; i++) {
				var itemView = queueList.Controls[i] as QueuedConstructionView;
				itemView.Data = queueData[i];
			}
		}
		
		private void onOption_Click(object sender, EventArgs e)
		{
			var itemView = sender as ConstructableItemView;
			
			if (controller.CanPick(itemView.Data)) {
				controller.Enqueue(itemView.Data);
				
				var queueItemView = new QueuedConstructionView();
				queueList.Controls.Add(queueItemView);
			}
			
			updateOptions();
			updateQueue();
		}
		
		private void onOption_MouseEnter(object sender, EventArgs e)
		{
			updateDescription(sender as ConstructableItemView);
		}
		
		private void removeButton_Click(object sender, EventArgs e)
		{
			if (controller.IsReadOnly || !queueList.HasSelection ||
			    (queueList.SelectedItem as QueuedConstructionView).Data.IsVirtual)
				return;
			
			controller.Dequeue(queueList.SelectedIndex);
			queueList.Controls.Remove(queueList.SelectedItem);
			
			updateOptions();
			updateQueue();
		}
		
		private void moveUpButton_Click(object sender, EventArgs e)
		{
			if (controller.IsReadOnly || !queueList.HasSelection)
				return;
			
			reorderItem(queueList.SelectedIndex, queueList.SelectedIndex - 1);
			updateQueue();
		}
		
		private void moveDownButton_Click(object sender, EventArgs e)
		{
			if (controller.IsReadOnly || !queueList.HasSelection)
				return;
			
			reorderItem(queueList.SelectedIndex, queueList.SelectedIndex + 1);
			updateQueue();
		}
	}
}
