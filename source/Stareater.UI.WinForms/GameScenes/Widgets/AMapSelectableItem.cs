using OpenTK;
using Stareater.AppData;
using Stareater.GLData;
using Stareater.GLData.SpriteShader;
using Stareater.GraphicsEngine;
using Stareater.GraphicsEngine.GuiElements;
using System.Drawing;
using System.Windows.Forms;

namespace Stareater.GameScenes.Widgets
{
	abstract class AMapSelectableItem<T> : AGuiElement
	{
		public const float Width = 150;
		public const float Height = 40;
		const float TextHeight = 10;
		const float Padding = 5;

		protected readonly BackgroundTexture backgroundHover = new BackgroundTexture(GalaxyTextures.Get.ToggleHover, 7);
		protected readonly BackgroundTexture backgroundSelected = new BackgroundTexture(GalaxyTextures.Get.ToggleToggled, 7);
		protected readonly BackgroundTexture backgroundUnselected = new BackgroundTexture(GalaxyTextures.Get.ToggleNormal, 7);

		public T Data { get; private set; }
		protected bool isHovered = false;
		private Vector2 textSize;
		
		protected AMapSelectableItem(T data)
		{
			this.Data = data;
			this.Position.FixedSize(Width, Height);
		}

		private Color? mImageBackground = null;
		public Color? ImageBackground
		{
			get => this.mImageBackground;
			set
			{
				this.apply(ref this.mImageBackground, value);
			}
		}

		private Sprite[] mImages = null;
		public Sprite[] Images
		{
			get { return this.mImages; }
			set
			{
				this.apply(ref this.mImages, value);
			}
		}

		public TextureInfo Image
		{
			set
			{
				this.apply(ref this.mImages, new[] { new Sprite(value, Color.White) });
			}
		}

		private string mText = null;
		public string Text
		{
			get { return this.mText; }
			set
			{
				this.textSize = TextRenderUtil.Get.SizeOf(value);
				this.apply(ref this.mText, value);
			}
		}

		public override bool OnMouseMove(Vector2 mousePosition, Keys modiferKeys)
		{
			this.apply(ref this.isHovered, true);

			return true;
		}

		public override void OnMouseLeave()
		{
			this.apply(ref this.isHovered, false);
		}

		protected override SceneObject makeSceneObject()
		{
			var background = this.backgroundTexture();

			var soBuilder = new SceneObjectBuilder().
				PixelSize(1 / SettingsWinforms.Get.GuiScale).
				Clip(this.Position.ClipArea).
				StartSprite(this.Z0, background.Sprite.Id, Color.White).
				Translate(this.Position.Center).
				AddVertices(SpriteHelpers.GuiBackground(background, this.Position.Size.X, this.Position.Size.Y));

			var imageSize = Height - 2 * Padding;
			var imageX = this.Position.Center.X - this.Position.Size.X / 2 + Padding + imageSize / 2;
			var totalLayers = (this.mImages != null ? this.mImages.Length : 0) + 2;

			if (this.mImageBackground.HasValue)
				soBuilder.
					StartSimpleSprite(this.Z0 - this.ZRange / totalLayers, GalaxyTextures.Get.Blank, this.mImageBackground.Value).
					Scale(imageSize).
					Translate(imageX, this.Position.Center.Y);

			if (this.mImages != null && this.mImages.Length > 0)
				for (int i = 0; i < this.mImages.Length; i++)
					soBuilder.StartSimpleSprite(this.Z0 - (2 + i) * this.ZRange / totalLayers, this.mImages[i].Texture, this.mImages[i].ModulationColor).
						Scale(imageSize).
						Translate(imageX, this.Position.Center.Y);

			if (!string.IsNullOrWhiteSpace(this.Text))
				soBuilder.StartText(this.Text, 0, 0, this.Z0 - this.ZRange / totalLayers, this.ZRange / 2, Color.White).
					Scale(TextHeight).
					Translate(imageX + Padding + imageSize / 2, this.Position.Center.Y + this.textSize.Y / 2f * TextHeight);

			return soBuilder.Build();
		}

		protected abstract BackgroundTexture backgroundTexture();
	}
}
