using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Zvjezdojedac.GUI
{
	public class ControlListView : FlowLayoutPanel
	{
		public const int NoneSelected = -1;

		protected int selectedIndex = NoneSelected;
		protected Color lastBackColor;
		protected Color lastForeColor;

		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			e.Control.Click += onItemClick;
		}

		protected override void OnControlRemoved(ControlEventArgs e)
		{
			base.OnControlRemoved(e);
			e.Control.Click -= onItemClick;
		}

		protected virtual void onItemClick(object sender, EventArgs e)
		{
			if (selectedIndex != NoneSelected)
				deselect(selectedIndex);

			Control clickedControl = sender as Control;
			select(Controls.IndexOf(clickedControl));
			if (SelectedIndexChanged != null)
				SelectedIndexChanged(this, new EventArgs());
		}

		private void select(int controlIndex)
		{
			lastBackColor = Controls[controlIndex].BackColor;
			lastForeColor = Controls[controlIndex].ForeColor;
			Controls[controlIndex].BackColor = SystemColors.Highlight;
			Controls[controlIndex].ForeColor = SystemColors.HighlightText;
			selectedIndex = controlIndex;
		}

		private void deselect(int selectedIndex)
		{
			Controls[selectedIndex].BackColor = lastBackColor;
			Controls[selectedIndex].ForeColor = lastForeColor;
			selectedIndex = NoneSelected;
		}

		public event EventHandler SelectedIndexChanged;

		public bool HasSelection
		{
			get { return (selectedIndex != NoneSelected); }
		}

		public int SelectedIndex
		{
			get { return selectedIndex; }
			
			set 
			{
				if (selectedIndex != NoneSelected)
					deselect(selectedIndex);

				select(value);
				if (SelectedIndexChanged != null)
					SelectedIndexChanged(this, new EventArgs());
			}
		}

		public Control SelectedItem
		{
			get { return (selectedIndex != NoneSelected) ? Controls[selectedIndex] : null; }
		}
	}
}
