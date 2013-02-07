using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stareater.Utils.PluginParameters;

namespace Stareater.GUI
{
	public partial class MapParameterRealRange : UserControl
	{
		RangeParameter<double> parameter;

		public MapParameterRealRange()
		{
			InitializeComponent();
		}

		public void SetData(RangeParameter<double> parameterInfo)
		{
			this.parameter = parameterInfo;

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
		}
	}
}
