using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using NGenerics.DataStructures.Mathematical;
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
		private const int PreviewPadding = 10;
		
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
			Context context = LocalizationManifest.Get.CurrentLanguage["FormSetupMap"];

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
		
		private void makePreview()
		{
			var random = new Random();
			var stars = this.controller.StarPositioner.Generate(random, this.controller.PlayerList.Count);
			var starlanes = this.controller.StarConnector.Generate(random, stars);
			
			var minCorner = stars.Stars.Aggregate((a, b) => new Vector2D(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y)));
			var size = stars.Stars.
				Select(x => x - minCorner).
				Aggregate(
					(a, b) => new Vector2D(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y))
				);
			var scale = Math.Min(
				(this.mapPreview.Width - 2 * PreviewPadding) / size.X, 
				(this.mapPreview.Height - 2 * PreviewPadding) / size.Y
			);
			
			var preview = new Bitmap(this.mapPreview.Width, this.mapPreview.Height);
			var starColor = new SolidBrush(Color.White);
			var homeSystemColor = new SolidBrush(Color.Gold);
			var starlaneColor = new Pen(Color.Blue);
			
			using(var g = Graphics.FromImage(preview))
			{
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				
				foreach(var lane in starlanes)
					g.DrawLine(
						starlaneColor,
						(float)((stars.Stars[lane.FromIndex].X - minCorner.X) * scale) + PreviewPadding,
						(float)((stars.Stars[lane.FromIndex].Y - minCorner.Y) * scale) + PreviewPadding,
						(float)((stars.Stars[lane.ToIndex].X - minCorner.X) * scale) + PreviewPadding,
						(float)((stars.Stars[lane.ToIndex].Y - minCorner.Y) * scale) + PreviewPadding
					);
					
				for(int i = 0; i < stars.Stars.Length; i++)
				{
					var position = (stars.Stars[i] - minCorner) * scale + new Vector2D(PreviewPadding, PreviewPadding);
					var isHomeSystem = stars.HomeSystems.Contains(i);
					
					g.FillEllipse(
						isHomeSystem ? homeSystemColor : starColor, 
						(float)position.X - (isHomeSystem ? 2 : 1),
						(float)position.Y - (isHomeSystem ? 2 : 1), 
						(isHomeSystem ? 5 : 3), 
						(isHomeSystem ? 5 : 3)
					);
				}
			}
			
			if (this.mapPreview.Image != null)
				this.mapPreview.Image.Dispose();
			this.mapPreview.Image = preview;
		}

		private void extractParameters(IEnumerable<AParameterBase> parameters)
		{
			var converter = new ParametarUiVisitor(parameters, this.makePreview);
			foreach(var control in converter.MakeUi())
				parametersPanel.Controls.Add(control);
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
	}
}
