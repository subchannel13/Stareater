using Stareater.GraphicsEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using Stareater.GLData;
using System.Drawing;
using Stareater.GraphicsEngine.Animators;
using Stareater.Localization;
using Stareater.GraphicsEngine.GuiElements;

namespace Stareater.GameScenes
{
	class IntroScene : AScene
	{
		private const int StarfieldRadius = 5;
		private const int StarfieldSeed = 101;
		private const int PaddingSeed = 997;
		private const float StarDisplacement = 0.7f;
		private const float StarSize = 0.03f;
		private const float StarSizeVariance = 0.4f;
		private const float StareaterSize = 0.2f;
		private const float TitleSize = 0.06f;

		private readonly Vector2 StareaterPathDirection = new Vector2(-2, -1).Normalized();
		private readonly Vector2 StareaterPathOffset = new Vector2(0, 1);
		private const float StareaterDistance = 3f;
		private const float StareaterWidth = 1.75f;
		private const float FadeSpeed = -1;
		private const float FadeDelay = 0.5f;
		private const float FadeDelayIncrement = 0.1f;
		private const float AppearDelay = 0.5f;
		private const float AppearSpeed = 1;

		private const float FarZ = 1;
		private const float Layers = 8.0f;

		protected override float GuiLayerThickness => 1 / Layers;
		private GuiText cancelText = null;

		private const float StarColorZ = 5 / Layers;
		private const float StarSaturationZ = 4 / Layers;
		private const float StareaterZ = 3 / Layers;
		private const float TitleZ = 2 / Layers;

		private IEnumerable<SceneObject> staticStarSprites = null;
		private IEnumerable<SceneObject> fadingStarSprites = null;
		private IEnumerable<SceneObject> paddingStarsSprite = null;
		private SceneObject stareaterOutlineSprite = null;
		private SceneObject stareaterTitleSprite = null;
		private Action timeoutCallback;
		private bool finished = false;

		public IntroScene(Action timeoutCallback)
		{
			this.timeoutCallback = timeoutCallback;
			this.cancelText = new GuiText()
			{
				Text = LocalizationManifest.Get.CurrentLanguage["Intro"]["cancelTip"].Text(),
				TextColor = Color.FromArgb(0, Color.Gray),
				TextSize = 16,
				Animation = textPolygons => new AnimationSequence(
					new AnimationDelay(1),
					new TweenAlpha(textPolygons, 0, 1, 0.5)
				)
			};
			this.cancelText.Position.WrapContent().ParentRelative(-1, -1, 5, 5);

			this.AddElement(this.cancelText);
		}

		protected override Matrix4 calculatePerspective()
		{
			var aspect = canvasSize.X / canvasSize.Y;

            if (aspect >= 1)
				return calcOrthogonalPerspective(aspect, 1, FarZ, new Vector2());
			else
				return calcOrthogonalPerspective(1, 1 / aspect, FarZ, new Vector2());
		}

		protected override void onResize()
		{
			if (canvasSize.X == 0 || canvasSize.Y == 0)
				return;

			var tallAspect = canvasSize.X < canvasSize.Y;
			var aspect = !tallAspect ? canvasSize.X / canvasSize.Y : canvasSize.Y / canvasSize.X;
			var pad = (int)Math.Ceiling(StarfieldRadius * (aspect - 1));
			var stars = new List<Vector2>();

			for (int i = 1; i <= pad; i++)
				for (int y = -StarfieldRadius; y <= StarfieldRadius; y++)
				{
					stars.Add(new Vector2(-StarfieldRadius - i, y));
					stars.Add(new Vector2(StarfieldRadius + i, y));
				}

			if (tallAspect)
				stars = new List<Vector2>(stars.Select(v => new Vector2(v.Y, v.X)));

			var random = new Random(PaddingSeed);
			this.UpdateScene(
				ref this.paddingStarsSprite,
				stars.Select(position => makeStar(position, random).Build()).ToList()
			);
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
					var position = new Vector2(x, y);
					var pathDistance = Math.Abs(Vector2.Dot(pathNormal, position - StareaterPathOffset));

					if (pathDistance < StareaterWidth && pathPhase(position) < StareaterDistance)
						fadingStars.Add(position);
					else
						staticStars.Add(position);
				}

			this.UpdateScene(
				ref this.staticStarSprites,
				staticStars.Select(position => 
					makeStar(position, random).
					Build()
				).ToList()
			);

			this.UpdateScene(
				ref this.fadingStarSprites,
				fadingStars.OrderBy(position => pathPhase(position)).Select((position, i) => 
					makeStar(position, random).
					Build(polygons => new AnimationSequence(
						new AnimationDelay(FadeDelayIncrement * (i + random.NextDouble() - 0.5) + FadeDelay),
						new ParallelAnimation(
							new TweenAlpha(polygons[0], 1, 0, FadeSpeed),
							new TweenAlpha(polygons[1], 1, 0, FadeSpeed)
						)
					))
				).ToList()
			);

			var delay = (fadingStars.Count - 1) * FadeDelayIncrement + FadeDelay - FadeSpeed;
			var stareaterPosition = (StareaterPathOffset + StareaterPathDirection * StareaterDistance) / StarfieldRadius / 2 - StareaterPathDirection * StareaterSize / 2;
			this.UpdateScene(
				ref this.stareaterOutlineSprite,
				new SceneObjectBuilder().
					StartSimpleSprite(StareaterZ, GalaxyTextures.Get.IntroStareaterOutline, Color.FromArgb(0, Color.White)).
                    Scale(StareaterSize).
					Translate(stareaterPosition).
					
					Build(polygons => new AnimationSequence(
						new AnimationDelay(delay),
						new TweenAlpha(polygons[0], 0, 1, AppearSpeed)
					))
            );

			delay += AppearDelay;
			var underlinePosition = stareaterPosition + new Vector2(0, StareaterSize / 2);
			var title = LocalizationManifest.Get.CurrentLanguage["Intro"]["Title"].Text();

			this.UpdateScene(
				ref this.stareaterTitleSprite,
				new SceneObjectBuilder().
					StartSimpleSprite(StareaterZ, GalaxyTextures.Get.IntroStareaterUnderline, Color.FromArgb(0, Color.White)).
					Scale(StareaterSize, StareaterSize / 4).
					Translate(underlinePosition).

					StartSprite(TitleZ, TextRenderUtil.Get.TextureId, Color.FromArgb(0, Color.White)).
					Scale(TitleSize).
					Translate(underlinePosition + new Vector2(0, TitleSize * 1.1f)).
					AddVertices(TextRenderUtil.Get.BufferText(title, -0.5f, Matrix4.Identity)).

					Build(polygons => new AnimationSequence(
						new AnimationDelay(delay),
						new ParallelAnimation(
							new TweenAlpha(polygons[0], 0, 1, AppearSpeed),
							new TweenAlpha(polygons[1], 0, 1, AppearSpeed)
                        ),
						new CallbackAnimation(this.onAnimationFinish)
					))
			);
		}

		protected override void onKeyPress(char c)
		{
			if (c == (int)System.Windows.Forms.Keys.Escape)
				this.onAnimationFinish();
		}

		protected override void onMouseClick(Vector2 mousePoint)
		{
			this.onAnimationFinish();
		}

		private float pathPhase(Vector2 position)
		{
			return Vector2.Dot(StareaterPathDirection, position - StareaterPathOffset);
        }

		private static SceneObjectBuilder makeStar(Vector2 cellPosition, Random random)
		{
			var position = new Vector2(
				(cellPosition.X + (float)(random.NextDouble() - 0.5) * StarDisplacement) / StarfieldRadius / 2,
				(cellPosition.Y + (float)(random.NextDouble() - 0.5) * StarDisplacement) / StarfieldRadius / 2
			);

			var colorRoll = random.NextDouble();
			var color = Color.FromArgb(255, 32, 32);

			if (colorRoll > 0.6)
				color = Color.FromArgb(255, 224, 0);
			if (colorRoll > 0.8)
				color = Color.FromArgb(0, 160, 255);

			var size = (float)random.NextDouble() * StarSizeVariance + 1 - StarSizeVariance;

			return new SceneObjectBuilder().
				StartSimpleSprite(StarColorZ, GalaxyTextures.Get.StarColor, color).
				Scale(size * StarSize).
				Translate(position).

				StartSimpleSprite(StarSaturationZ, GalaxyTextures.Get.StarGlow, Color.White).
				Scale(size * StarSize).
				Translate(position);
        }

		private void onAnimationFinish()
		{
			if (this.finished)
				return;
			this.finished = true;

			var animatedObjects = new List<SceneObject>(this.fadingStarSprites);
			animatedObjects.Add(this.stareaterOutlineSprite);
			animatedObjects.Add(this.stareaterTitleSprite);
			
			foreach (var sceneObject in animatedObjects)
				sceneObject.Animator.FastForward();
			this.RemoveElement(this.cancelText);
			this.timeoutCallback();
		}
	}
}
