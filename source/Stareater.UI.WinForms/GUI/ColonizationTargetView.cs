using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Utils.Collections;
using Stareater.Utils.NumberFormatters;
using Stareater.GuiUtils;

namespace Stareater.GUI
{
	public partial class ColonizationTargetView : UserControl
	{
		private readonly ColonizationController controller;
		
		public ColonizationTargetView()
		{
			InitializeComponent();
		}
		
		public ColonizationTargetView(ColonizationController controller) : this()
		{
			this.controller = controller;
			var context = SettingsWinforms.Get.Language["FormColonization"];
			
			var infoFormatter = new ThousandsFormatter(controller.Population, controller.PopulationMax);
			var infoVars = new TextVar("pop", infoFormatter.Format(controller.Population)).
				And("max", infoFormatter.Format(controller.PopulationMax));
			
			this.targetName.Text = LocalizationMethods.PlanetName(controller.PlanetBody);
			this.targetInfo.Text = context["population"].Text(infoVars.Get);
			
			updateView();
			
		}

		void updateView()
		{
			var sources = controller.Sources().ToArray();
			
			while(sourceList.Controls.Count < sources.Length)
			{
				var itemView = new ColonizationSourceView(controller);
				itemView.OnStateChange += onSourceChange;
				sourceList.Controls.Add(itemView);
			}
			while(sourceList.Controls.Count > sources.Length)
			{
				var itemView = sourceList.Controls[sourceList.Controls.Count - 1] as ColonizationSourceView;
				itemView.OnStateChange -= onSourceChange;
				sourceList.Controls.RemoveAt(sourceList.Controls.Count - 1);
			}
			
			for(int i = 0; i < sources.Length; i++)
	        {
				var itemView = sourceList.Controls[i] as ColonizationSourceView;
				itemView.Data = sources[i];
	        }
		}
		
		private void onSourceChange()
		{
			updateView();
		}
		
		private void addButton_Click(object sender, EventArgs e)
		{
			if (this.controller.AvailableSources().Any())
				using(var form = new FormPickColonizationSource(controller))
					if (form.ShowDialog() == DialogResult.OK)
					{
						controller.StartColonization(form.SelectedSource);
						updateView();
					}
		}
	}
}
