using Stareater.GraphicsEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using Stareater.GLData;
using System.Drawing;
using Stareater.GLData.SpriteShader;

namespace Stareater.GameScenes
{
	class IntroScene : AScene
	{
		private const int StarfieldRadius = 5;
		private const int StarfieldSeed = 101;
		private const float StarDisplacement = 0.7f;
		private const float StarSize = 0.03f;

		private const float FarZ = 1;
		private const float Layers = 4.0f;

		protected override float GuiLayerThickness => 1 / Layers;

		private const float StarColorZ = 3 / Layers;
		private const float StarSaturationZ = 2 / Layers;

		private SceneObject starSprites = null;
		private Action timeoutCallback;

		private double countDown = 3;

		public IntroScene(Action timeoutCallback)
		{
			this.timeoutCallback = timeoutCallback;
		}

		protected override Matrix4 calculatePerspective()
		{
			return calcOrthogonalPerspective(canvasSize.X / canvasSize.Y, 1, FarZ, new Vector2());
		}

		protected override void FrameUpdate(double deltaTime)
		{
			if (this.countDown <= 0)
				return;

			this.countDown -= deltaTime;
			if (this.countDown <= 0)
				this.timeoutCallback();
        }

		public override void Activate()
		{
			var stars = new List<Vector2>();
			var random = new Random(StarfieldSeed);

			for (int y = -StarfieldRadius; y <= StarfieldRadius; y++)
				for (int x = -StarfieldRadius; x <= StarfieldRadius; x++)
					stars.Add(new Vector2(
						(x + (float)(random.NextDouble() - 0.5) * StarDisplacement) / StarfieldRadius / 2,
						(y + (float)(random.NextDouble() - 0.5) * StarDisplacement) / StarfieldRadius / 2
					));

			this.UpdateScene(
				ref this.starSprites,
				new SceneObjectBuilder().
					StartSprite(StarColorZ, GalaxyTextures.Get.StarColor.Id, Color.Yellow).
					AddVertices(
						stars.SelectMany(position => SpriteHelpers.TexturedRectVertexData(
							position,
							StarSize,
							StarSize,
							GalaxyTextures.Get.StarColor
					))).

					StartSprite(StarSaturationZ, GalaxyTextures.Get.StarGlow.Id, Color.White).
					AddVertices(
						stars.SelectMany(position => SpriteHelpers.TexturedRectVertexData(
							position,
							StarSize,
							StarSize,
							GalaxyTextures.Get.StarGlow
					))).

					Build()
			);
		}
    }
}
