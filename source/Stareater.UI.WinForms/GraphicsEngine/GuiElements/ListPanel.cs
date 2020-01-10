using OpenTK;
using Stareater.GLData;
using Stareater.GLData.SpriteShader;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Stareater.GraphicsEngine.GuiElements
{
	class ListPanel : AGuiElement
	{
		private readonly GridPositionBuilder positionBuilder;
		private readonly GuiPanel container;
		private readonly GuiSlider slider;
		private readonly ValueReference<Vector2> scrollOffset = new ValueReference<Vector2>(new Vector2(0, 0));
		private float scrollableHeight = 0;

		public ListPanel(int columns, float elementWidth, float elementHeight, float elementSpacing) : base()
		{
			this.positionBuilder = new GridPositionBuilder(columns, elementWidth, elementHeight, elementSpacing);

			this.slider = new GuiSlider
			{
				Orientation = Orientation.Vertical,
				SlideCallback = onSlide
			};
			this.slider.Position.FixedSize(15, 45).ParentRelative(1, 1).UseMargins().StretchBottomTo(this, -1);

			this.container = new GuiPanel
			{
				HandlesMouse = false
			};
			this.container.Position.ParentRelative(-1, 1).UseMargins().StretchRightTo(this.slider, -1).StretchBottomTo(this, -1);
		}

		private readonly List<AGuiElement> mChildren = new List<AGuiElement>();
		public IEnumerable<AGuiElement> Children
		{
			private get => this.mChildren;
			set
			{
				if (scene != null)
					foreach (var child in this.mChildren)
						scene.RemoveElement(child);

				this.mChildren.Clear();
				this.positionBuilder.Restart();
				this.container.Clear();
				
				foreach (var child in value)
				{
					this.mChildren.Add(child);
					child.Position.ParentRelative(-1, 1).Then.Offset(this.scrollOffset);
					this.positionBuilder.Add(child.Position);
					this.container.AddChild(child);
				}

				if (scene != null)
					foreach (var child in mChildren)
						scene.AddElement(child, this.container);

				this.updateSlider();
			}
		}

		private BackgroundTexture mBackground = null;
		public BackgroundTexture Background
		{
			get { return this.mBackground; }
			set
			{
				this.apply(ref this.mBackground, value);
			}
		}

		public float Padding
		{
			set
			{
				this.slider.Margins = new Vector2(value, value);
				this.container.Margins = new Vector2(value, value);
			}
		}

		public override void Attach(AScene scene, AGuiElement parent)
		{
			base.Attach(scene, parent);
			scene.AddElement(this.slider, this);
			scene.AddElement(this.container, this);

			foreach (var child in mChildren)
				scene.AddElement(child, this.container);
		}

		public override bool OnMouseDown(Vector2 mousePosition)
		{
			return true;
		}

		public override void OnMouseScroll(Vector2 mousePosition, int delta)
		{
			this.slider.OnMouseScroll(mousePosition, delta);
		}

		protected override SceneObject makeSceneObject()
		{
			this.updateSlider();

			if (this.mBackground != null)
				return new SceneObjectBuilder().
					Clip(this.Position.ClipArea).
					StartSprite(this.Z0, this.mBackground.Sprite.Id, Color.White).
					Translate(this.Position.Center).
					AddVertices(SpriteHelpers.GuiBackground(this.mBackground, this.Position.Size.X, this.Position.Size.Y)).
					Build();
			else
				return null;
		}

		//TODO(v0.9) check if this override is needed
		protected override Vector2 measureContent()
		{
			foreach (var child in this.mChildren)
				child.Position.Recalculate();

			return new Vector2(
				this.mChildren.Max(x => x.Position.Center.X + x.Position.Size.X / 2) - this.mChildren.Min(x => x.Position.Center.X - x.Position.Size.X / 2),
				this.mChildren.Max(x => x.Position.Center.Y + x.Position.Size.Y / 2) - this.mChildren.Min(x => x.Position.Center.Y - x.Position.Size.Y / 2)
			);
		}

		private void onSlide(float state)
		{
			if (!this.slider.IsShown)
				return;
			
			this.scrollOffset.Value = new Vector2(0, state * this.scrollableHeight);

			foreach (var child in this.Children)
				child.Position.Recalculate();
			this.updateScene();
		}

		private void updateSlider()
		{
			var rows = (this.mChildren.Count + this.positionBuilder.Columns - 1) / this.positionBuilder.Columns;
			this.scrollableHeight =
				rows * this.positionBuilder.ElementHeight
				+ (rows - 1) * this.positionBuilder.ElementSpacing
				- this.container.Position.Size.Y;

			var showSlider = this.scrollableHeight > 0;
			if (this.IsShown && showSlider != this.slider.IsShown)
				if (!this.slider.IsShown)
					this.scene.ShowElement(this.slider);
				else
					this.scene.HideElement(this.slider);

			if (showSlider)
				this.slider.ScrollStep = (this.positionBuilder.ElementHeight + this.positionBuilder.ElementSpacing) / this.scrollableHeight;
		}
	}
}
