using System;
using System.Drawing;
using System.Linq;
using OpenTK;
using Stareater.Localization;
using Stareater.GraphicsEngine;
using Stareater.GLData.SpriteShader;

namespace Stareater.GLRenderers
{
	class GameOverRenderer : AScene
	{
		private const float DefaultViewSize = 5;
		private const float TextZ = 0;
		private const float FarZ = 1;
		
		private SceneObject text = null;
		
		#region implemented abstract members of ARenderer
		public override void Activate()
		{
			this.UpdateScene(
				ref this.text,
				new SceneObject(
					new []{
						new PolygonData(
							TextZ,
							new SpriteData(Matrix4.CreateTranslation(0, 0.5f, 0), TextZ, TextRenderUtil.Get.TextureId, Color.Red),
							TextRenderUtil.Get.BufferText(
								LocalizationManifest.Get.CurrentLanguage["FormMain"]["GameOver"].Text(),
								-0.5f,
								Matrix4.Identity).ToList()
						)
					}
			));
		}
		
		protected override void FrameUpdate(double deltaTime)
		{ }
		
		protected override Matrix4 calculatePerspective()
		{
			var aspect = canvasSize.X / canvasSize.Y;
			return calcOrthogonalPerspective(aspect * DefaultViewSize, DefaultViewSize, FarZ, new Vector2());
		}		
		#endregion
	}
}
