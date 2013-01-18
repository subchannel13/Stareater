using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stareater.Maps;

namespace Stareater.GUI
{
	public partial class MapParameterSelector : UserControl
	{
		public MapParameterSelector()
		{
			InitializeComponent();
		}

		public void SetData(MapFactoryParameterInfo parameterInfo)
		{
			nameLabel.Text = parameterInfo.Name;

			foreach (var valueInfo in parameterInfo)
				valueSelector.Items.Add(new Tag<int>(valueInfo.Key, valueInfo.Value));

			valueSelector.SelectedIndex = 0;
		}
	}
}
