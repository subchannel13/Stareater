using System;
using System.Linq;
using System.Windows.Forms;
using Stareater.Utils.PluginParameters;

namespace Stareater.GUI
{
	public partial class MapParameterRealRange : UserControl
	{
		private ContinuousRangeParameter parameter;
		private Action changeListener;

		public MapParameterRealRange()
		{
			InitializeComponent();
		}

		public void SetData(ContinuousRangeParameter parameterInfo, Action changeListener)
		{
			this.parameter = parameterInfo;
			this.changeListener = changeListener;

			nameLabel.Text = parameterInfo.Name;
			valueLabel.Text = parameterInfo.ValueDescription;

			double ratio = parameterInfo.Value / (parameterInfo.Maximum - parameterInfo.Minimum) - parameterInfo.Minimum;
			valueSlider.Value = (int)(ratio * sliderMax);
		}

		private double sliderMax
		{
			get { return valueSlider.Maximum - valueSlider.LargeChange + 1; }
		}

		private void valueSlider_Scroll(object sender, ScrollEventArgs e)
		{
			double ratio = e.NewValue / sliderMax;
			parameter.Value = parameter.Minimum + ratio * (parameter.Maximum - parameter.Minimum);

			valueLabel.Text = parameter.ValueDescription;
			changeListener();
		}
	}
}
