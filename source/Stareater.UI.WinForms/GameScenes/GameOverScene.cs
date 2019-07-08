using System.Drawing;
using System.Linq;
using OpenTK;
using Stareater.Localization;
using Stareater.GraphicsEngine;
using Stareater.GLData.SpriteShader;
using Stareater.Controllers;
using Stareater.Utils.NumberFormatters;
using Stareater.GLData;

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
		private SceneObject scoresText = null;
		private SceneObject namesText = null;

		private ResultsController controller;

		#region AScene implemented
		protected override float guiLayerThickness => 1 / Layers;

		public override void Activate()
		{
			this.UpdateScene(
				ref this.headerText,
				new SceneObject(new PolygonData(
					TextZ,
					new SpriteData(Matrix4.CreateTranslation(0, 2f, 0), TextRenderUtil.Get.TextureId, Color.Red, null),
					TextRenderUtil.Get.BufferRaster(
						LocalizationManifest.Get.CurrentLanguage["FormMain"]["GameOver"].Text(),
						-0.5f,
						Matrix4.Identity).ToList()
				))
			);
		}

		protected override Matrix4 calculatePerspective()
		{
			var aspect = canvasSize.X / canvasSize.Y;
			return calcOrthogonalPerspective(aspect * DefaultViewSize, DefaultViewSize, FarZ, new Vector2());
		}		
		#endregion

		public void SetResults(ResultsController controller)
		{
			this.controller = controller;

			var scores = controller.Scores.OrderByDescending(x => x.VictoryPoints).ToList();
			var formatter = new DecimalsFormatter(0, 0);

			this.UpdateScene(
				ref this.scoresText,
				new SceneObject(scores.Select(
					(x, i) => new PolygonData(
						TextZ,
						new SpriteData(
							Matrix4.CreateScale(TextSize, TextSize, 1) * Matrix4.CreateTranslation(-0.2f, -0.5f * i + 0.8f, 0), 
							TextRenderUtil.Get.TextureId, 
							Color.White, 
							null
						),
						TextRenderUtil.Get.BufferRaster(
							formatter.Format(x.VictoryPoints),
							-1f,
							Matrix4.Identity
						).ToList()
				)))
			);

			this.UpdateScene(
				ref this.namesText,
				new SceneObject(scores.Select(
					(x, i) => new PolygonData(
						TextZ,
						new SpriteData(
							Matrix4.CreateScale(TextSize, TextSize, 1) * Matrix4.CreateTranslation(0, -0.5f * i + 0.8f, 0),
							TextRenderUtil.Get.TextureId, 
							Color.White, 
							null
						),
						TextRenderUtil.Get.BufferRaster(
							x.Player.Name,
							0f,
							Matrix4.Identity
						).ToList()
				)))
			);
		}
	}
}
