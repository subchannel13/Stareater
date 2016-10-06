using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using Stareater.AppData;
using Stareater.Localization;

namespace Stareater.GLRenderers
{
	class GameOverRenderer : AScene
	{
		private const double DefaultViewSize = 5;
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
		
		protected override void setupPerspective()
		{
			double aspect = canvasSize.X / (double)canvasSize.Y;
			const double semiRadius = 0.5 * DefaultViewSize;

			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(
				-aspect * semiRadius, aspect * semiRadius,
				-semiRadius, semiRadius, 
				0, -FarZ);

			GL.MatrixMode(MatrixMode.Modelview);
		}		
		#endregion
	}
}
