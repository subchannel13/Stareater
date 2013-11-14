using System;
using System.Drawing;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;

namespace Stareater.GUI
{
	public partial class FormBuildingQueue : Form
	{
		private AConstructionSiteController controller;
		
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
		}
		
		private void reorderItem(int fromIndex, int toIndex)
		{
			if (toIndex < 0) toIndex = 0;
			if (toIndex >= queueList.Controls.Count)	toIndex = queueList.Controls.Count - 1;
			if (fromIndex == toIndex)
				return;
			
			controller.ReorderQueue(fromIndex, toIndex);
			
			var item = queueList.Controls[fromIndex];
			
			queueList.SelectedIndex = ControlListView.NoneSelected;
			queueList.Controls.SetChildIndex(item, toIndex);
			queueList.SelectedIndex = toIndex;
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
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
		
		private void onOption_Click(object sender, EventArgs e)
		{
			var itemView = sender as ConstructableItemView;
			
			if (controller.CanPick(itemView.Data)) {
				controller.Enqueue(itemView.Data);
				
				var queueItemView = new QueuedConstructionView();
				queueItemView.Data = itemView.Data;
				queueList.Controls.Add(queueItemView);
			}
			
			updateOptions();
		}
		
		private void onOption_MouseEnter(object sender, EventArgs e)
		{
			updateDescription(sender as ConstructableItemView);
		}
		
		private void removeButton_Click(object sender, EventArgs e)
		{
			if (!queueList.HasSelection)
				return;
			
			controller.Dequeue(queueList.SelectedIndex);
			queueList.Controls.Remove(queueList.SelectedItem);
			
			updateOptions();
		}
		
		private void moveUpButton_Click(object sender, EventArgs e)
		{
			if (!queueList.HasSelection)
				return;
			
			reorderItem(queueList.SelectedIndex, queueList.SelectedIndex - 1);
		}
		
		private void moveDownButton_Click(object sender, EventArgs e)
		{
			reorderItem(queueList.SelectedIndex, queueList.SelectedIndex + 1);
		}
	}
}
