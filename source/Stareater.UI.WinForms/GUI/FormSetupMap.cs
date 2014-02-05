using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Galaxy;
using Stareater.Galaxy.Builders;
using Stareater.GuiUtils;
using Stareater.Localization;
using Stareater.Utils.PluginParameters;

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
			foreach (var mapFactory in MapAssets.StarPositioners) {
				if (controller.StarPositioner == mapFactory)
					selectedIndex = shapeSelector.Items.Count;
				shapeSelector.Items.Add(new Tag<IStarPositioner>(mapFactory, mapFactory.Name));
			}
			shapeSelector.SelectedIndex = selectedIndex;
			shapeSelector.Visible = MapAssets.StarPositioners.Length > 1;

			selectedIndex = 0;
			foreach (var wormholeFactory in MapAssets.StarConnectors) {
				if (controller.StarConnector == wormholeFactory)
					selectedIndex = wormholeSelector.Items.Count;
				wormholeSelector.Items.Add(new Tag<IStarConnector>(wormholeFactory, wormholeFactory.Name));
			}
			wormholeSelector.SelectedIndex = selectedIndex;
			wormholeSelector.Visible = MapAssets.StarConnectors.Length > 1;

			selectedIndex = 0;
			foreach (var populatorFactory in MapAssets.StarPopulators) {
				if (controller.StarConnector == populatorFactory)
					selectedIndex = populatorSelector.Items.Count;
				populatorSelector.Items.Add(new Tag<IStarPopulator>(populatorFactory, populatorFactory.Name));
			}
			populatorSelector.SelectedIndex = selectedIndex;
			populatorSelector.Visible = MapAssets.StarPopulators.Length > 1;
		}

		private void setLanguage()
		{
			Context context = SettingsWinforms.Get.Language["FormSetupMap"];

			this.Font = SettingsWinforms.Get.FormFont;
			this.Text = context["FormTitle"].Text();

			acceptButton.Text = context["acceptButton"].Text();
		}

		private void populateParameters()
		{
			parametersPanel.Controls.Clear();
			extractParameters(controller.StarPositioner.Parameters);
			extractParameters(controller.StarConnector.Parameters);
			extractParameters(controller.StarPopulator.Parameters);
			
		}

		private void extractParameters(ParameterList parameters)
		{
			Dictionary<int, Control> parameterGuis = new Dictionary<int, Control>();
			foreach (var parameterInfo in parameters.Selectors) {
				var parameterControl = new MapParameterSelector();
				parameterControl.SetData(parameterInfo);
				parameterGuis.Add(parameterGuis.Count, parameterControl);
			}
			foreach (var parameterInfo in parameters.RealRanges) {
				var parameterControl = new MapParameterRealRange();
				parameterControl.SetData(parameterInfo);
				parameterGuis.Add(parameterGuis.Count, parameterControl);
			}
			for(int i = 0; i < parameterGuis.Count; i++)
				parametersPanel.Controls.Add(parameterGuis[i]);
		}

		private void acceptButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void shapeSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (shapeSelector.SelectedItem == null)
				return;

			populateParameters();
		}
		
		//TODO: Map preview
	}
}
