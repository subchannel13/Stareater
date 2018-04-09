using Stareater.GraphicsEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using Stareater.GLData;
using System.Drawing;
using Stareater.GraphicsEngine.Animators;
using Stareater.Localization;

namespace Stareater.GameScenes
{
	class IntroScene : AScene
	{
		private const int StarfieldRadius = 5;
		private const int StarfieldSeed = 101;
		private const float StarDisplacement = 0.7f;
		private const float StarSize = 0.03f;
		private const float StarSizeVariance = 0.4f;
		private const float StareaterSize = 0.2f;
		private const float TitleSize = 0.06f;

		private readonly Vector2 StareaterPathDirection = new Vector2(-2, -1).Normalized();
		private readonly Vector2 StareaterPathOffset = new Vector2(0, 0.1f);
		private const float StareaterDistance = 0.3f;
		private const float FadeSpeed = -1;
		private const float FadeDelay = 0.5f;
		private const float FadeDelayIncrement = 0.1f;

		private const float FarZ = 1;
		private const float Layers = 8.0f;

		protected override float GuiLayerThickness => 1 / Layers;

		private const float StarColorZ = 5 / Layers;
		private const float StarSaturationZ = 4 / Layers;
		private const float StareaterZ = 3 / Layers;
		private const float TitleZ = 2 / Layers;

		private IEnumerable<SceneObject> staticStarSprites = null;
		private IEnumerable<SceneObject> fadingStarSprites = null;
		private SceneObject stareaterOutlineSprite = null;
		private SceneObject stareaterTitleSprite = null;
		private Action timeoutCallback;

		public IntroScene(Action timeoutCallback)
		{
			this.timeoutCallback = timeoutCallback;
		}

		protected override Matrix4 calculatePerspective()
		{
			return calcOrthogonalPerspective(canvasSize.X / canvasSize.Y, 1, FarZ, new Vector2());
		}

		public override void Activate()
		{
			var random = new Random(StarfieldSeed);
			var staticStars = new List<Star>();
			var fadingStars = new List<Star>();
			var pathNormal = StareaterPathDirection.PerpendicularLeft;

			var title = LocalizationManifest.Get.CurrentLanguage["FormMainMenu"]["Title"].Text(); //TODO(v0.7) move to formMain
			TextRenderUtil.Get.Prepare(new string[] { title }); //TODO(v0.7) remove the need for preparation

			for (int y = -StarfieldRadius; y <= StarfieldRadius; y++)
				for (int x = -StarfieldRadius; x <= StarfieldRadius; x++)
				{
					var position = new Vector2(
						(x + (float)(random.NextDouble() - 0.5) * StarDisplacement) / StarfieldRadius / 2,
						(y + (float)(random.NextDouble() - 0.5) * StarDisplacement) / StarfieldRadius / 2
					);
					var star = new Star(
						position,
						starColor(random),
						(float)random.NextDouble() * StarSizeVariance + 1 - StarSizeVariance
					);

                    if (Math.Abs(Vector2.Dot(pathNormal, position - StareaterPathOffset)) < 0.1 && pathPhase(position) < StareaterDistance)
						fadingStars.Add(star);
					else
						staticStars.Add(star);
				}

			this.UpdateScene(
				ref this.staticStarSprites,
				staticStars.Select(star => new SceneObjectBuilder().
					StartSimpleSprite(StarColorZ, GalaxyTextures.Get.StarColor, star.Color).
					Scale(star.Size * StarSize).
					Translate(star.Position).

					StartSimpleSprite(StarSaturationZ, GalaxyTextures.Get.StarGlow, Color.White).
					Scale(star.Size * StarSize).
					Translate(star.Position).

					Build()
				).ToList()
			);

			this.UpdateScene(
				ref this.fadingStarSprites,
				fadingStars.OrderBy(star => pathPhase(star.Position)).Select((star, i) => new SceneObjectBuilder().
					StartSimpleSprite(StarColorZ, GalaxyTextures.Get.StarColor, star.Color).
					Scale(star.Size * StarSize).
					Translate(star.Position).

					StartSimpleSprite(StarSaturationZ, GalaxyTextures.Get.StarGlow, Color.White).
					Scale(star.Size * StarSize).
					Translate(star.Position).

					Build(polygons => new AnimationSequence(
						new AnimationDelay(i * FadeDelayIncrement + FadeDelay),
						new ParallelAnimation(
							new TweenAlpha(polygons[0], 1, 0, FadeSpeed),
							new TweenAlpha(polygons[1], 1, 0, FadeSpeed)
						)
					))
				).ToList()
			);

			var delay = (fadingStars.Count - 1) * FadeDelayIncrement + FadeDelay - FadeSpeed;
            this.UpdateScene(
				ref this.stareaterOutlineSprite,
				new SceneObjectBuilder().
					StartSimpleSprite(StareaterZ, GalaxyTextures.Get.IntroStareaterOutline, Color.FromArgb(0, Color.White)).
                    Scale(StareaterSize).
					Translate(new Vector2(-0.2f, 0)).
					
					Build(polygons => new AnimationSequence(
						new AnimationDelay(delay),
						new TweenAlpha(polygons[0], 0, 1, -FadeSpeed)
					))
            );

			delay += -FadeSpeed;
            this.UpdateScene(
				ref this.stareaterTitleSprite,
				new SceneObjectBuilder().
					StartSimpleSprite(StareaterZ, GalaxyTextures.Get.IntroStareaterUnderline, Color.FromArgb(0, Color.White)).
					Scale(StareaterSize, StareaterSize / 4).
					Translate(new Vector2(-0.2f, StareaterSize / 2)).

					StartSprite(TitleZ, TextRenderUtil.Get.TextureId, Color.FromArgb(0, Color.White)).
					Scale(TitleSize).
					Translate(-0.2f, StareaterSize / 2 + TitleSize * 1.1f).
					AddVertices(TextRenderUtil.Get.BufferText(title, -0.5f, Matrix4.Identity)).

					Build(polygons => new AnimationSequence(
						new AnimationDelay(delay),
						new ParallelAnimation(
							new TweenAlpha(polygons[0], 0, 1, -FadeSpeed),
							new TweenAlpha(polygons[1], 0, 1, -FadeSpeed)
                        ),
						new CallbackAnimation(this.timeoutCallback)
					))
			);
		}

		private float pathPhase(Vector2 position)
		{
			return Vector2.Dot(StareaterPathDirection, position - StareaterPathOffset);
        }

		private Color starColor(Random random)
		{
			var roll = random.NextDouble();
			
			if (roll < 0.6)
				return Color.FromArgb(255, 32, 32);
			if (roll < 0.8)
				return Color.FromArgb(255, 224, 0);

			return Color.FromArgb(0, 160, 255);
		}

		private struct Star
		{
			public Vector2 Position;
			public Color Color;
			public float Size;

			public Star(Vector2 position, Color color, float size)
			{
				this.Position = position;
				this.Color = color;
				this.Size = size;
			}
		}
	}
}
