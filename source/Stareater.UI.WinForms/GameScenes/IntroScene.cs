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
		private const float FarZ = 1;
		private const float Layers = 4.0f;

		protected override float GuiLayerThickness => 1 / Layers;

		private const float StarColorZ = 3 / Layers;
		private const float StarSaturationZ = 2 / Layers;

		private SceneObject starSprites = null;

		protected override Matrix4 calculatePerspective()
		{
			return calcOrthogonalPerspective(canvasSize.X / canvasSize.Y, 1, FarZ, new Vector2());
		}

		//TODO(v0.7) remove mandatory override from base class
		protected override void FrameUpdate(double deltaTime)
		{
			//no operation
		}

		//TODO(v0.7) extract constants
		public override void Activate()
		{
			const int R = 5;
			var stars = new List<Vector2>();
			var random = new Random(101);

			for (int y = -R; y <= R; y++)
				for (int x = -R; x <= R; x++)
					stars.Add(new Vector2(
						x / 10f + (float)(random.NextDouble() - 0.5) / 15f,
						y / 10f + (float)(random.NextDouble() - 0.5) / 15f
					));

			this.UpdateScene(
				ref this.starSprites,
				new SceneObjectBuilder().
					StartSprite(StarColorZ, GalaxyTextures.Get.StarColor.Id, Color.Yellow).
					AddVertices(
						stars.SelectMany(position => SpriteHelpers.TexturedRectVertexData(
							position,
							0.03f,
							0.03f,
							GalaxyTextures.Get.StarColor
					))).

					StartSprite(StarSaturationZ, GalaxyTextures.Get.StarGlow.Id, Color.White).
					AddVertices(
						stars.SelectMany(position => SpriteHelpers.TexturedRectVertexData(
							position,
							0.03f,
							0.03f,
							GalaxyTextures.Get.StarGlow
					))).

					Build()
			);
		}
    }
}
