using OpenTK;
using Stareater.GLData;
using Stareater.GLData.SpriteShader;
using System;
using System.Drawing;

namespace Stareater.GraphicsEngine.GuiElements
{
	class ToggleButton : AGuiElement
	{
		private bool isToggled = false;
		private bool isHovered = false;

		public Action<bool> ToggleCallback { get; set; }

		public ToggleButton(bool initialState) : base()
		{
			this.isToggled = initialState;
		}

		private BackgroundTexture mBackgroundHover = null;
		public BackgroundTexture BackgroundHover
		{
			get { return this.mBackgroundHover; }
			set
			{
				apply(ref this.mBackgroundHover, value);
			}
		}

		private BackgroundTexture mBackgroundNormal = null;
		public BackgroundTexture BackgroundNormal
		{
			get { return this.mBackgroundNormal; }
			set
			{
				apply(ref this.mBackgroundNormal, value);
			}
		}

		private BackgroundTexture mBackgroundToggled = null;
		public BackgroundTexture BackgroundToggled
		{
			get { return this.mBackgroundToggled; }
			set
			{
				apply(ref this.mBackgroundToggled, value);
			}
		}

		private BackgroundTexture mForgroundImage = null;
		public BackgroundTexture ForgroundImage
		{
			get { return this.mForgroundImage; }
			set
			{
				apply(ref this.mForgroundImage, value);
			}
		}

		public override bool OnMouseDown(Vector2 mousePosition)
		{
			if (this.isOutside(mousePosition))
				return false;

			this.isToggled = !this.isToggled;
			this.updateScene();

			return true;
		}

		public override bool OnMouseUp(Vector2 mousePosition)
		{
			if (this.isOutside(mousePosition))
				return false;

			this.ToggleCallback(this.isToggled);
			return true;
		}

		public override void OnMouseMove(Vector2 mousePosition)
		{
			apply(ref this.isHovered, !this.isOutside(mousePosition));
		}

		protected override SceneObject makeSceneObject()
		{
			var background = this.mBackgroundNormal;
			if (this.isHovered)
				background = this.mBackgroundHover;
			if (this.isToggled)
				background = this.mBackgroundToggled;

			var soBuilder = new SceneObjectBuilder().
				Clip(this.Position.ClipArea).
				StartSprite(this.Z0, background.Sprite.Id, Color.White).
				Translate(this.Position.Center).
				AddVertices(SpriteHelpers.GuiBackground(background, this.Position.Size.X, this.Position.Size.Y));

			if (this.mForgroundImage != null)
				soBuilder.
					StartSprite(this.Z0 - this.ZRange / 2, this.mForgroundImage.Sprite.Id, Color.White).
					Translate(this.Position.Center).
					AddVertices(SpriteHelpers.GuiBackground(this.mForgroundImage, this.Position.Size.X, this.Position.Size.Y));

			return soBuilder.Build();
		}

		private bool isOutside(Vector2 mousePosition)
		{
			var innerPoint = mousePosition - this.Position.Center;
			return Math.Abs(innerPoint.X) > this.Position.Size.X / 2 ||
				Math.Abs(innerPoint.Y) > this.Position.Size.Y / 2;
		}
	}
}
