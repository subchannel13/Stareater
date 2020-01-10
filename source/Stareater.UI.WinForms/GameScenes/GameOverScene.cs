using System.Drawing;
using System.Linq;
using OpenTK;
using Stareater.Localization;
using Stareater.GraphicsEngine;
using Stareater.Controllers;
using Stareater.Utils.NumberFormatters;
using System.Collections.Generic;
using Stareater.GraphicsEngine.GuiElements;

namespace Stareater.GameScenes
{
	class GameOverScene : AScene
	{
		private const float DefaultViewSize = 7;
		
		private const float FarZ = 1;
		private const float Layers = 4.0f;

		private readonly GuiText title;
		private readonly List<GuiText> tableCells = new List<GuiText>();

		public GameOverScene()
		{
			this.title = new GuiText
			{
				Margins = new Vector2(0, 20),
				TextColor = Color.Red,
				TextHeight = 64,
				Text = LocalizationManifest.Get.CurrentLanguage["FormMain"]["GameOver"].Text()
			};
			this.title.Position.WrapContent().Then.ParentRelative(0, 1).UseMargins();
			this.AddElement(this.title);
		}

		#region AScene implemented
		protected override float guiLayerThickness => 1 / Layers;

		protected override Matrix4 calculatePerspective()
		{
			var aspect = canvasSize.X / canvasSize.Y;

			return calcOrthogonalPerspective(aspect * DefaultViewSize, DefaultViewSize, FarZ, new Vector2());
		}		
		#endregion

		public void SetResults(ResultsController controller)
		{
			foreach (var cellText in this.tableCells)
				this.RemoveElement(cellText);

			//TODO(later) add table header

			var scores = controller.Scores.OrderByDescending(x => x.VictoryPoints).ToList();
			var formatter = new DecimalsFormatter(0, 0);

			for(int i = 0; i < scores.Count; i++)
			{
				var score = new GuiText
				{
					TextColor = Color.White,
					TextHeight = 30,
					Text = formatter.Format(scores[i].VictoryPoints)
				};
				score.Position.WrapContent().Then.RelativeTo(this.title, 0, -1, 1, 1).Then.Offset(-30, -20 + -40 * i);
				this.AddElement(score);
				this.tableCells.Add(score);

				var name = new GuiText
				{
					TextColor = Color.White,
					TextHeight = 30,
					Text = scores[i].Player.Name
				};
				name.Position.WrapContent().Then.RelativeTo(this.title, 0, -1, -1, 1).Then.Offset(30, -20 + -40 * i);
				this.AddElement(name);
				this.tableCells.Add(name);
			}
		}
	}
}
