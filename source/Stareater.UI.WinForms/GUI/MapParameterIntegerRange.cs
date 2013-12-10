using System;
using System.Linq;
using System.Windows.Forms;

using Stareater.Utils.PluginParameters;

namespace Stareater.GUI
{
	public partial class MapParameterIntegerRange : UserControl
	{
		RangeParameter<int> parameter;

		public MapParameterIntegerRange()
		{
			InitializeComponent();
		}

		public void SetData(RangeParameter<int> parameterInfo)
		{
			this.parameter = parameterInfo;

			nameLabel.Text = parameterInfo.Name;
			valueLabel.Text = parameterInfo.ValueDescription;

			valueSlider.Minimum = parameterInfo.Minimum;
			valueSlider.Maximum = parameterInfo.Maximum;
			valueSlider.Value = parameterInfo.Value;
		}

		private void valueSlider_Scroll(object sender, ScrollEventArgs e)
		{
			parameter.Value = e.NewValue;
			valueLabel.Text = parameter.ValueDescription;
		}
	}
}
