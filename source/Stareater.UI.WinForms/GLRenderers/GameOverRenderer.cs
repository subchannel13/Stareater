using System;
using System.Drawing;
using OpenTK;
using Stareater.Localization;
using Stareater.GLData;
using Stareater.GraphicsEngine;

namespace Stareater.GLRenderers
{
	class GameOverRenderer : AScene
	{
		private const float DefaultViewSize = 5;
		private const float FarZ = 1;
		
		private TextDrawable textDrawable = null;
		
		#region implemented abstract members of ARenderer
		protected override void FrameUpdate(double deltaTime)
		{
			if (this.textDrawable == null)
				this.textDrawable = new TextDrawable(
					new SpriteGlProgram.ObjectData(
						Matrix4.CreateTranslation(0, 0.5f, 0), 0, TextRenderUtil.Get.TextureId, Color.Red),
					-0.5f);
			
			this.textDrawable.Draw(this.projection, LocalizationManifest.Get.CurrentLanguage["FormMain"]["GameOver"].Text());
		}
		
		protected override Matrix4 calculatePerspective()
		{
			var aspect = canvasSize.X / canvasSize.Y;
			return calcOrthogonalPerspective(aspect * DefaultViewSize, DefaultViewSize, FarZ, new Vector2());
		}		
		#endregion
	}
}
