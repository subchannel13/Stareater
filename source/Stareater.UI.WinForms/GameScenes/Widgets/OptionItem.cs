using OpenTK;
using Stareater.GraphicsEngine;
using Stareater.GraphicsEngine.GuiElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Stareater.GameScenes.Widgets
{
	class OptionItem<T> : AListItem<T>
	{
		private bool selected = false;
		private bool isPressed = false;
		private HashSet<OptionItem<T>> group = new HashSet<OptionItem<T>>();

		public OptionItem(T data) : base(data)
		{ }

		public Action<T> OnSelect { get; set; }

		public void GroupWith(OptionItem<T> otherOption)
		{
			this.group.Remove(this);
			this.group = otherOption.group;
			this.group.Add(this);
		}

		public void Select()
		{
			this.group.FirstOrDefault(x => x.selected)?.deselect();
			this.selected = true;
			this.OnSelect(this.Data);

			this.updateScene();
		}

		public override void Attach(AScene scene, AGuiElement parent)
		{
			base.Attach(scene, parent);
			this.group.Add(this);
		}

		public override void Detach()
		{
			base.Detach();
			this.group.Remove(this);
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

		public override bool OnMouseMove(Vector2 mousePosition, Keys modiferKeys)
		{
			this.apply(ref this.isHovered, true);

			return true;
		}

		public override void OnMouseLeave()
		{
			this.apply(ref this.isHovered, false);
		}

		protected override BackgroundTexture backgroundTexture()
		{
			if (this.isHovered)
				return this.backgroundHover;
			if (this.selected)
				return this.backgroundSelected;

			return this.backgroundUnselected;
		}

		private void deselect()
		{
			this.selected = false;
			this.updateScene();
		}
	}
}
