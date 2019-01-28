using OpenTK;
using Stareater.GLData;
using Stareater.GLData.SpriteShader;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Stareater.GraphicsEngine.GuiElements
{
	class CycleButton<T> : AGuiElement
	{
		private const float PressOffsetX = 1.5f;
		private const float PressOffsetY = -3;

		private bool isPressed = false;
		private bool isHovered = false;

		public Action<T> CycleCallback { get; set; }
		public Func<T, TextureInfo> ItemImage { get; set; }

		private BackgroundTexture mBackgroundHover = null;
		public BackgroundTexture BackgroundHover
		{
			get { return this.mBackgroundHover; }
			set
			{
				this.apply(ref this.mBackgroundHover, value);
			}
		}

		private BackgroundTexture mBackgroundNormal = null;
		public BackgroundTexture BackgroundNormal
		{
			get { return this.mBackgroundNormal; }
			set
			{
				this.apply(ref this.mBackgroundNormal, value);
			}
		}

		private TextureInfo? forgroundImage = null;

		private float mPadding = 0;
		public float Padding
		{
			get { return this.mPadding; }
			set
			{
				this.apply(ref this.mPadding, value);
			}
		}

		private List<T> mItems = new List<T>();
		public IEnumerable<T> Items
		{
			set
			{
				this.mItems = value.ToList();
				this.updateForeground();
			}
		}

		private int selectedIndex = 0;
		public T Selection
		{
			set
			{
				this.selectedIndex = this.mItems.IndexOf(value);
				this.updateForeground();
			}
		}

		public override bool OnMouseDown(Vector2 mousePosition)
		{
			this.apply(ref this.isPressed, true);

			return true;
		}

		public override void OnMouseUp()
		{
			this.isPressed = false;
			if (this.mItems.Count == 0)
				return;

			this.selectedIndex = (this.selectedIndex + 1) % this.mItems.Count;
			this.CycleCallback(this.mItems[this.selectedIndex]);
			this.updateForeground();
		}

		public override void OnMouseDownCanceled()
		{
			this.apply(ref this.isPressed, false);
		}

		public override void OnMouseMove(Vector2 mousePosition)
		{
			this.apply(ref this.isHovered, true);
		}

		public override void OnMouseLeave()
		{
			this.apply(ref this.isHovered, false);
		}

		protected override SceneObject makeSceneObject()
		{
			var pressOffset = this.isPressed && this.isHovered ? new Vector2(PressOffsetX, PressOffsetY) : new Vector2(0, 0);

			var background = this.isHovered ? this.BackgroundHover : this.mBackgroundNormal;
			var soBuilder = new SceneObjectBuilder().
				Clip(this.Position.ClipArea). //TODO(v0.8) add press offset to clip area
				StartSprite(this.Z0, background.Sprite.Id, Color.White).
				Translate(this.Position.Center + pressOffset).
				AddVertices(SpriteHelpers.GuiBackground(background, this.Position.Size.X, this.Position.Size.Y));

			if (this.forgroundImage.HasValue)
				soBuilder.
					StartSimpleSprite(this.Z0 - this.ZRange / 2, this.forgroundImage.Value, Color.White).
					Scale(this.Position.Size - new Vector2(2 * this.mPadding, 2 * this.mPadding)).
					Translate(this.Position.Center);

			return soBuilder.Build();
		}

		private void updateForeground()
		{
			if (this.mItems.Count == 0)
				return;

			this.apply(ref this.forgroundImage, this.ItemImage(this.mItems[this.selectedIndex]));
		}
	}
}
