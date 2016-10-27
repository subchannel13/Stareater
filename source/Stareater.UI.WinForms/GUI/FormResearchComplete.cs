using System;
using System.Windows.Forms;
using Stareater.Controllers.Views;

namespace Stareater.GUI
{
	public partial class FormResearchComplete : Form
	{
		private readonly ResearchCompleteController controller;
		
		public FormResearchComplete()
		{
			InitializeComponent();
		}
		
		public FormResearchComplete(ResearchCompleteController controller) : this()
		{
			this.controller = controller;
		}

		private void FormResearchComplete_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.controller.Done();
		}

		private void acceptButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void reorderUpAction_Click(object sender, EventArgs e)
		{
			//TODO(v0.6)
		}

		private void reorderDownAction_Click(object sender, EventArgs e)
		{
			//TODO(v0.6)
		}
	}
}
