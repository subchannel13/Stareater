using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Galaxy;
using Stareater.Galaxy.Builders;
using Stareater.GuiUtils;
using Stareater.Localization;
using Stareater.Utils;
using Stareater.Utils.PluginParameters;

namespace Stareater.GUI
{
	public partial class FormSetupMap : Form
	{
		private const int PreviewPadding = 10;
		private const float MinR = 2;
		private const float MaxR = 15;

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
			shapeSelector.Visible = MapAssets.StarPositioners.Count > 1;

			selectedIndex = 0;
			foreach (var wormholeFactory in MapAssets.StarConnectors) {
				if (controller.StarConnector == wormholeFactory)
					selectedIndex = wormholeSelector.Items.Count;
				wormholeSelector.Items.Add(new Tag<IStarConnector>(wormholeFactory, wormholeFactory.Name));
			}
			wormholeSelector.SelectedIndex = selectedIndex;
			wormholeSelector.Visible = MapAssets.StarConnectors.Count > 1;

			selectedIndex = 0;
			foreach (var populatorFactory in MapAssets.StarPopulators) {
				if (controller.StarConnector == populatorFactory)
					selectedIndex = populatorSelector.Items.Count;
				populatorSelector.Items.Add(new Tag<IStarPopulator>(populatorFactory, populatorFactory.Name));
			}
			populatorSelector.SelectedIndex = selectedIndex;
			populatorSelector.Visible = MapAssets.StarPopulators.Count > 1;
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
			var map = this.controller.GeneratePreview(random);

			var starPositions = map.Systems.Select(x => x.Position);
			var minCorner = starPositions.Aggregate((a, b) => new Vector2D(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y)));
			var size = starPositions.
				Select(x => x - minCorner).
				Aggregate(
					(a, b) => new Vector2D(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y))
				);
			var scale = Math.Min(
				(this.mapPreview.Width - 2 * PreviewPadding) / size.X, 
				(this.mapPreview.Height - 2 * PreviewPadding) / size.Y
			);
			
			var preview = new Bitmap(this.mapPreview.Width, this.mapPreview.Height);
			var starColorBottom = new SolidBrush(Color.Gray);
			var starColorTop = new SolidBrush(Color.White);
			var homeSystemColorBottom = new SolidBrush(Color.Brown);
			var homeSystemColorTop = new SolidBrush(Color.Gold);
			var starlaneColor = new Pen(Color.Blue);
			
			using(var g = Graphics.FromImage(preview))
			{
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				
				foreach(var lane in map.Starlanes)
					g.DrawLine(
						starlaneColor,
						(float)((lane.Endpoints.First.Position.X - minCorner.X) * scale) + PreviewPadding,
						(float)((lane.Endpoints.First.Position.Y - minCorner.Y) * scale) + PreviewPadding,
						(float)((lane.Endpoints.Second.Position.X - minCorner.X) * scale) + PreviewPadding,
						(float)((lane.Endpoints.Second.Position.Y - minCorner.Y) * scale) + PreviewPadding
					);

				var scoreOffset = Math.Min(this.controller.WorstSystemScore, map.Systems.Min(x => x.StartingScore));
				var scoreRange = Math.Max(this.controller.BestSystemScore, map.Systems.Max(x => x.PotentialScore)) - scoreOffset;

				foreach (var system in map.Systems)
				{
					var position = (system.Position - minCorner) * scale + new Vector2D(PreviewPadding, PreviewPadding);
					var startingScoreR = scoreRadius(system.StartingScore, scoreOffset, scoreRange);
					var potentialScoreR = scoreRadius(system.PotentialScore, scoreOffset, scoreRange);

					var brushTop = system.IsHomeSystem ? homeSystemColorTop : starColorTop;
					var brushBottom = system.IsHomeSystem ? homeSystemColorBottom : starColorBottom;

					g.FillEllipse(
						brushBottom,
						(float)position.X - potentialScoreR / 2, (float)position.Y - potentialScoreR / 2,
						potentialScoreR, potentialScoreR
					);

					g.FillEllipse(
						brushTop,
						(float)position.X - startingScoreR / 2, (float)position.Y - startingScoreR / 2,
						startingScoreR, startingScoreR
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

		private float scoreRadius(double score, double offset, double range)
		{
			return (float)Methods.Lerp((score - offset) / range, MinR, MaxR);
		}
	}
}
