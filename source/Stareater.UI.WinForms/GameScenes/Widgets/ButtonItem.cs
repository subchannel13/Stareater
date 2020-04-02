using OpenTK;
using Stareater.GraphicsEngine.GuiElements;
using System;
using System.Windows.Forms;

namespace Stareater.GameScenes.Widgets
{
	class ButtonItem<T> : AListItem<T>
	{
		private bool mIsPressed = false;

		public ButtonItem(T data) : base(data)
		{ }

		public Action<T> OnSelect { get; set; }

		public override bool OnMouseDown(Vector2 mousePosition)
		{
			this.apply(ref this.mIsPressed, true);

			return true;
		}

		public override void OnMouseDownCanceled()
		{
			this.apply(ref this.mIsPressed, false);
		}

		public override void OnMouseUp(Keys modiferKeys)
		{
			this.OnSelect?.Invoke(this.Data);
		}

		protected override BackgroundTexture backgroundTexture()
		{
			if (this.isHovered)
				return this.backgroundHover;

			return this.backgroundUnselected;
		}
	}
}
