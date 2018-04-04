using Stareater.GraphicsEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using Stareater.GLData;
using System.Drawing;
using Stareater.GLData.SpriteShader;
using Stareater.GraphicsEngine.Animators;

namespace Stareater.GameScenes
{
	class IntroScene : AScene
	{
		private const int StarfieldRadius = 5;
		private const int StarfieldSeed = 101;
		private const float StarDisplacement = 0.7f;
		private const float StarSize = 0.03f;

		private readonly Vector2 StareaterPathDirection = new Vector2(-2, -1).Normalized();
		private readonly Vector2 StareaterPathOffset = new Vector2(0, 0.1f);
		private const float StareaterDistance = 0.3f;
		private const float FadeSpeed = -1;
		private const float FadeDelay = 0.5f;
		private const float FadeDelayIncrement = 0.1f;

		private const float FarZ = 1;
		private const float Layers = 4.0f;

		protected override float GuiLayerThickness => 1 / Layers;

		private const float StarColorZ = 3 / Layers;
		private const float StarSaturationZ = 2 / Layers;

		private SceneObject staticStarSprites = null;
		private IEnumerable<SceneObject> fadingStarSprites = null;
		private Action timeoutCallback;

		private double countDown = 10; //TODO(v0.7) remove, replace with animation event

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
			var random = new Random(StarfieldSeed);
			var staticStars = new List<Vector2>();
			var fadingStars = new List<Vector2>();
			var pathNormal = StareaterPathDirection.PerpendicularLeft;

			for (int y = -StarfieldRadius; y <= StarfieldRadius; y++)
				for (int x = -StarfieldRadius; x <= StarfieldRadius; x++)
				{
					var position = new Vector2(
						(x + (float)(random.NextDouble() - 0.5) * StarDisplacement) / StarfieldRadius / 2,
						(y + (float)(random.NextDouble() - 0.5) * StarDisplacement) / StarfieldRadius / 2
					);

                    if (Math.Abs(Vector2.Dot(pathNormal, position - StareaterPathOffset)) < 0.1 && PathPhase(position) < StareaterDistance)
						fadingStars.Add(position);
					else
						staticStars.Add(position);
				}

			this.UpdateScene(
				ref this.staticStarSprites,
				new SceneObjectBuilder().
					StartSprite(StarColorZ, GalaxyTextures.Get.StarColor.Id, Color.Yellow).
					AddVertices(
						staticStars.SelectMany(position => SpriteHelpers.TexturedRectVertexData(
							position,
							StarSize,
							StarSize,
							GalaxyTextures.Get.StarColor
					))).

					StartSprite(StarSaturationZ, GalaxyTextures.Get.StarGlow.Id, Color.White).
					AddVertices(
						staticStars.SelectMany(position => SpriteHelpers.TexturedRectVertexData(
							position,
							StarSize,
							StarSize,
							GalaxyTextures.Get.StarGlow
					))).

					Build()
			);

			this.UpdateScene(
				ref this.fadingStarSprites,
				fadingStars.OrderBy(p => PathPhase(p)).Select((position, i) => new SceneObjectBuilder().
					StartSimpleSprite(StarColorZ, GalaxyTextures.Get.StarColor, Color.Yellow).
					Scale(StarSize).
					Translate(position).

					StartSimpleSprite(StarSaturationZ, GalaxyTextures.Get.StarGlow, Color.White).
					Scale(StarSize).
					Translate(position).

					Build(polygons => new AnimationSequence(
						new AnimationDelay(i * FadeDelayIncrement + FadeDelay),
						new ParallelAnimation(
							new TweenAlpha(polygons[0], 1, 0, FadeSpeed),
							new TweenAlpha(polygons[1], 1, 0, FadeSpeed)
						)
					))
				).ToList()
			);
		}

		private float PathPhase(Vector2 position)
		{
			return Vector2.Dot(StareaterPathDirection, position - StareaterPathOffset);
        }
    }
}
