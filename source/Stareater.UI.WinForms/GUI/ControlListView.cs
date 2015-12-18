using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Stareater.GUI
{
	public class ControlListView : FlowLayoutPanel
	{
		public const int NoneSelected = -1;

		private int selectedIndex = NoneSelected;
		private Color lastBackColor;
		private Color lastForeColor;
		private Control lastSelected = null;

		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			e.Control.Click += onItemClick;
		}

		protected override void OnControlRemoved(ControlEventArgs e)
		{
			base.OnControlRemoved(e);
			e.Control.Click -= onItemClick;

			if (e.Control == lastSelected && Controls.Count > 0) {
				selectedIndex = Math.Min(selectedIndex, Controls.Count - 1);
				select(selectedIndex);
				if (SelectedIndexChanged != null)
					SelectedIndexChanged(this, new EventArgs());
			}
		}

		protected virtual void onItemClick(object sender, EventArgs e)
		{
			if (selectedIndex != NoneSelected)
				deselect();

			var clickedControl = sender as Control;
			select(Controls.IndexOf(clickedControl));
			if (SelectedIndexChanged != null)
				SelectedIndexChanged(this, new EventArgs());
		}

		private void select(int controlIndex)
		{
			lastBackColor = Controls[controlIndex].BackColor;
			lastForeColor = Controls[controlIndex].ForeColor;
			lastSelected = Controls[controlIndex];
			Controls[controlIndex].BackColor = SystemColors.Highlight;
			Controls[controlIndex].ForeColor = SystemColors.HighlightText;
			selectedIndex = controlIndex;
		}

		private void deselect()
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
			get
			{
				return selectedIndex;
			}
			set
			{
				if (selectedIndex != NoneSelected)
					deselect();

				if (value != NoneSelected)
					select(value);
				else
					selectedIndex = NoneSelected;

				if (SelectedIndexChanged != null)
					SelectedIndexChanged(this, new EventArgs());
			}
		}

		public Control SelectedItem
		{
			get
			{
				return (selectedIndex != NoneSelected) ? Controls[selectedIndex] : null;
			}
		}
	}
}
