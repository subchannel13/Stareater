using OpenTK;
using Stareater.GLData;

namespace Stareater.GraphicsEngine.GuiElements
{
	class BackgroundTexture
	{
		private Vector2 textureSize;

		public TextureInfo Sprite { get; private set; }
		public float PaddingLeft { get; private set; }
		public float PaddingRight { get; private set; }
		public float PaddingTop { get; private set; }
		public float PaddingBottom { get; private set; }

		public BackgroundTexture(TextureInfo sprite, int padding)
		{
			this.Sprite = sprite;
			this.PaddingLeft = padding;
			this.PaddingRight = padding;
			this.PaddingTop = padding;
			this.PaddingBottom = padding;
			this.textureSize = GalaxyTextures.Get.Size; //TODO(v0.8) generalize texture management
		}

		public TextureInfo LeftTexture
		{
			get
			{
				return new TextureInfo(this.Sprite.Id, new Vector2[] {
					this.Sprite.Coordinates[0],
					this.Sprite.Coordinates[0] + new Vector2(this.PaddingLeft / this.textureSize.X, 0),
					this.Sprite.Coordinates[3] + new Vector2(this.PaddingLeft / this.textureSize.X, 0),
					this.Sprite.Coordinates[3],
				});
			}
		}

		public TextureInfo RightTexture
		{
			get
			{
				return new TextureInfo(this.Sprite.Id, new Vector2[] {
					this.Sprite.Coordinates[1] - new Vector2(this.PaddingRight / this.textureSize.X, 0),
					this.Sprite.Coordinates[1],
					this.Sprite.Coordinates[2],
					this.Sprite.Coordinates[2] - new Vector2(this.PaddingRight / this.textureSize.X, 0),
				});
			}
		}

		public TextureInfo TopTexture
		{
			get
			{
				return new TextureInfo(this.Sprite.Id, new Vector2[] {
					this.Sprite.Coordinates[3] + new Vector2(this.PaddingLeft / this.textureSize.X, -this.PaddingTop / this.textureSize.Y),
					this.Sprite.Coordinates[2] + new Vector2(-this.PaddingRight / this.textureSize.X, -this.PaddingTop / this.textureSize.Y),
					this.Sprite.Coordinates[2] + new Vector2(-this.PaddingRight / this.textureSize.X, 0),
					this.Sprite.Coordinates[3] + new Vector2(this.PaddingLeft / this.textureSize.X, 0),
				});
			}
		}

		public TextureInfo BottomTexture
		{
			get
			{
				return new TextureInfo(this.Sprite.Id, new Vector2[] {
					this.Sprite.Coordinates[0] + new Vector2(this.PaddingLeft / this.textureSize.X, 0),
					this.Sprite.Coordinates[1] - new Vector2(this.PaddingRight / this.textureSize.X, 0),
					this.Sprite.Coordinates[1] - new Vector2(this.PaddingRight / this.textureSize.X, -this.PaddingBottom / this.textureSize.Y),
					this.Sprite.Coordinates[0] + new Vector2(this.PaddingLeft / this.textureSize.X, this.PaddingBottom / this.textureSize.Y),
				});
			}
		}

		public TextureInfo CenterTexture
		{
			get
			{
				return new TextureInfo(this.Sprite.Id, new Vector2[] {
					this.Sprite.Coordinates[0] + new Vector2(this.PaddingLeft / this.textureSize.X, this.PaddingBottom / this.textureSize.Y),
					this.Sprite.Coordinates[1] + new Vector2(-this.PaddingRight / this.textureSize.X, this.PaddingBottom / this.textureSize.Y),
					this.Sprite.Coordinates[2] - new Vector2(this.PaddingRight / this.textureSize.X, this.PaddingTop / this.textureSize.Y),
					this.Sprite.Coordinates[3] + new Vector2(this.PaddingLeft / this.textureSize.X, -this.PaddingTop / this.textureSize.Y),
				});
			}
		}
	}
}
