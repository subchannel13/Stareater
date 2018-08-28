using OpenTK;
using Stareater.GLData;
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

		private TextureInfo? mBackgroundHover = null;
		public TextureInfo? BackgroundHover
		{
			get { return this.mBackgroundHover; }
			set
			{
				Apply(ref this.mBackgroundHover, value);
			}
		}

		private TextureInfo? mBackgroundNormal = null;
		public TextureInfo? BackgroundNormal
		{
			get { return this.mBackgroundNormal; }
			set
			{
				Apply(ref this.mBackgroundNormal, value);
			}
		}

		private TextureInfo? mBackgroundToggled = null;
		public TextureInfo? BackgroundToggled
		{
			get { return this.mBackgroundToggled; }
			set
			{
				Apply(ref this.mBackgroundToggled, value);
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
			Apply(ref this.isHovered, !this.isOutside(mousePosition));
		}

		protected override SceneObject MakeSceneObject()
		{
			var background = this.mBackgroundNormal.Value;
			if (this.isHovered)
				background = this.mBackgroundHover.Value;
			if (this.isToggled)
				background = this.mBackgroundToggled.Value;

			var soBuilder = new SceneObjectBuilder().
				StartSimpleSprite(this.Z, background, Color.White).
				Scale(this.Position.Size.X, this.Position.Size.Y).
				Translate(this.Position.Center);

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
