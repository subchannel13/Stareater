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
			
			foreach (var item in controller.ConstructableItems) {
				var itemView = new ConstructableItemView();
				itemView.Data = item;
				itemView.Click += onOption_Click;
				itemView.MouseEnter += onOption_MouseEnter;
				optionList.Controls.Add(itemView);
			}
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
		
		private void onOption_Click(object sender, EventArgs e)
		{
			var itemView = sender as ConstructableItemView;
			
			if (controller.CanPick(itemView.Data)) {
				controller.Enqueue(itemView.Data);
				
				var queueItemView = new QueuedConstructionView();
				queueItemView.Data = itemView.Data;
				queueList.Controls.Add(queueItemView);
			}
			
			//TODO update whole list
			itemView.Enabled = controller.CanPick(itemView.Data);
		}
		
		private void onOption_MouseEnter(object sender, EventArgs e)
		{
			updateDescription(sender as ConstructableItemView);
		}
	}
}
