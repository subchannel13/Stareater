using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.AppData;
using Stareater.Localization;

namespace Stareater.GLRenderers
{
	class GameOverRenderer : AScene
	{
		private const float DefaultViewSize = 5;
		private const float FarZ = -1;
		
		#region implemented abstract members of ARenderer
		public override void Draw(double deltaTime)
		{
			GL.Color4(Color.Red);
			GL.Translate(0, 0.5, 0);
			TextRenderUtil.Get.RenderText(LocalizationManifest.Get.CurrentLanguage["FormMain"]["GameOver"].Text(), -0.5f);
		}
		
		public override void ResetLists()
		{
			//no op
		}
		
		protected override Matrix4 calculatePerspective()
		{
			var aspect = canvasSize.X / canvasSize.Y;
			return orthogonalPerspective(aspect * DefaultViewSize, DefaultViewSize, FarZ, new Vector2());
		}		
		#endregion
	}
}
