using OpenTK;
using Stareater.GLData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Stareater.GraphicsEngine.GuiElements
{
	class SelectableImage<T> : AGuiElement
	{
		private readonly T userData;
		private float padding = 0;

		private bool selected = false;
		private bool isPressed = false;
		private bool isHovered = false;
		private HashSet<SelectableImage<T>> group = new HashSet<SelectableImage<T>>();
		//TODO(v0.9) remove from the group on detach

		public Action<T> SelectCallback { get; set; }

		public SelectableImage(T userData)
		{
			this.userData = userData;
			this.group.Add(this);
		}

		public void GroupWith(SelectableImage<T> otherSelector)
		{
			this.group = otherSelector.group;
			this.group.Add(this);
		}

		public void Select()
		{
			this.group.FirstOrDefault(x => x.selected)?.deselect();
			this.selected = true;
			this.SelectCallback(this.userData);

			this.updateScene();
		}

		private TextureInfo? mForgroundImage = null;
		public TextureInfo? ForgroundImage
		{
			get { return this.mForgroundImage; }
			set
			{
				this.apply(ref this.mForgroundImage, value);
			}
		}

		private Color mForgroundImageColor = Color.White;
		public Color ForgroundImageColor
		{
			get { return this.mForgroundImageColor; }
			set
			{
				this.apply(ref this.mForgroundImageColor, value);
			}
		}

		private TextureInfo? mSelectorImage = null;
		public TextureInfo? SelectorImage
		{
			get { return this.mSelectorImage; }
			set
			{
				this.apply(ref this.mSelectorImage, value);
			}
		}

		public float Padding
		{
			set
			{
				this.apply(ref this.padding, value);
			}
		}

		public override bool OnMouseDown(Vector2 mousePosition)
		{
			this.apply(ref this.isPressed, true);

			return true;
		}

		public override void OnMouseUp(Keys modiferKeys)
		{
			this.isPressed = false;
			this.Select();
		}

		public override void OnMouseDownCanceled()
		{
			this.apply(ref this.isPressed, false);
		}

		public override void OnMouseMove(Vector2 mousePosition, Keys modiferKeys)
		{
			this.apply(ref this.isHovered, true);
		}

		public override void OnMouseLeave()
		{
			this.apply(ref this.isHovered, false);
		}

		protected override SceneObject makeSceneObject()
		{
			var soBuilder = new SceneObjectBuilder();
			if (this.mForgroundImage.HasValue)
				soBuilder.StartSimpleSprite(this.Z0, this.mForgroundImage.Value, this.mForgroundImageColor).
					Scale(this.Position.Size - new Vector2(this.padding, this.padding)).
					Translate(this.Position.Center);

			if (selected && this.mSelectorImage.HasValue)
				soBuilder.StartSimpleSprite(this.Z0 - this.ZRange / 2, this.mSelectorImage.Value, Color.White).
					Scale(this.Position.Size).
					Translate(this.Position.Center);

			//TODO(v0.9) hover over unselected image

			return soBuilder.Build();
		}

		private void deselect()
		{
			this.selected = false;
			this.updateScene();
		}
	}
}
