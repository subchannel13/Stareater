using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stareater.Controllers;
using Stareater.Localization;
using Stareater.AppData;
using Stareater.Maps;

namespace Stareater.GUI
{
	public partial class FormSetupMap : Form
	{
		private NewGameController controller;

		public FormSetupMap()
		{
			InitializeComponent();

			setLanguage();
		}

		public void Initialize(NewGameController controller)
		{
			this.controller = controller;

			int selectedIndex = 0;
			foreach (var mapFactory in MapAssets.MapFactories) {
				if (controller.MapFactory == mapFactory)
					selectedIndex = shapeSelector.Items.Count;

				shapeSelector.Items.Add(new Tag<IMapFactory>(mapFactory, mapFactory.Name));
			}
			shapeSelector.SelectedIndex = selectedIndex;
		}

		private void setLanguage()
		{
			Context context = SettingsWinforms.Get.Language["FormSetupMap"];

			this.Font = SettingsWinforms.Get.FormFont;
			this.Text = context["FormTitle"];

			acceptButton.Text = context["acceptButton"];
		}

		private void acceptButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void shapeSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (shapeSelector.SelectedItem == null)
				return;

			parametersPanel.Controls.Clear();
			foreach (var parameterInfo in controller.MapFactory.Parameters()) {
				var parameterSelector = new MapParameterSelector();
				parameterSelector.SetData(parameterInfo);
				parametersPanel.Controls.Add(parameterSelector);
			}
		}
	}
}
