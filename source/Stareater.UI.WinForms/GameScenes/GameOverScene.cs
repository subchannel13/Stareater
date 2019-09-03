using System.Drawing;
using System.Linq;
using OpenTK;
using Stareater.Localization;
using Stareater.GraphicsEngine;
using Stareater.Controllers;
using Stareater.Utils.NumberFormatters;
using Stareater.GLData;
using System.Collections.Generic;

namespace Stareater.GameScenes
{
	class GameOverScene : AScene
	{
		private const float DefaultViewSize = 7;
		
		private const float FarZ = 1;
		private const float Layers = 4.0f;

		private const float TextZ = 2 / Layers;

		private const float TextSize = 0.4f;
		
		private SceneObject headerText = null;
		private IEnumerable<SceneObject> tableRows = null;
		private float pixelSize = 1;

		#region AScene implemented
		protected override float guiLayerThickness => 1 / Layers;

		public override void Activate()
		{
			this.UpdateScene(
				ref this.headerText,
				new SceneObjectBuilder().
					PixelSize(this.pixelSize).
					StartText(
						LocalizationManifest.Get.CurrentLanguage["FormMain"]["GameOver"].Text(),
						-0.5f, TextZ, 1/Layers,
						TextRenderUtil.Get.TextureId, Color.Red
					).
					Translate(0, 2).
					Build()
			);
		}

		protected override Matrix4 calculatePerspective()
		{
			var aspect = canvasSize.X / canvasSize.Y;
			this.pixelSize = DefaultViewSize / canvasSize.Y;
			return calcOrthogonalPerspective(aspect * DefaultViewSize, DefaultViewSize, FarZ, new Vector2());
		}		
		#endregion

		public void SetResults(ResultsController controller)
		{
			var scores = controller.Scores.OrderByDescending(x => x.VictoryPoints).ToList();
			var formatter = new DecimalsFormatter(0, 0);

			this.UpdateScene(
				ref this.tableRows,
				scores.Select((score, i) =>
					new SceneObjectBuilder().
						PixelSize(this.pixelSize).
						StartText(
							formatter.Format(score.VictoryPoints),
							-1, TextZ, 1 / Layers, 
							TextRenderUtil.Get.TextureId, Color.White
						).
						Scale(TextSize).
						Translate(-0.2, -0.5 * i + 0.8).
						StartText(
							score.Player.Name,
							0, TextZ, 1 / Layers,
							TextRenderUtil.Get.TextureId, Color.White
						).
						Scale(TextSize).
						Translate(0, -0.5 * i + 0.8).
						Build()
					).ToList()
			);
		}
	}
}
