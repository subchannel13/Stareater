using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Stareater.GuiUtils;
using Stareater.Utils.PluginParameters;

namespace Stareater.GUI
{
	public partial class MapParameterSelector : UserControl
	{
		private SelectorParameter parameter;
		private Action changeListener;

		public MapParameterSelector()
		{
			InitializeComponent();
		}

		public void SetData(SelectorParameter parameterInfo, Action changeListener)
		{
			this.parameter = parameterInfo;
			this.changeListener = changeListener;
			this.nameLabel.Text = parameterInfo.Name;

			foreach (var valueInfo in parameterInfo)
				valueSelector.Items.Add(new Tag<int>(valueInfo.Key, valueInfo.Value));

			valueSelector.SelectedIndex = parameterInfo.Value;
		}

		private void valueSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			parameter.Value = valueSelector.SelectedIndex;
			changeListener();
		}
	}
}
