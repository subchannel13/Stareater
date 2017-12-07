using OpenTK;
using System.Collections.Generic;

namespace Stareater.GraphicsEngine.GuiElements
{
	class ElementPosition
	{
		public Vector2 Center { get; private set; }
		public Vector2 Size { get; private set; }

		private IGuiElement targetElement;
		private List<IPositioner> positioners = new List<IPositioner>();
		private float lastParentWidth = 0;
		private float lastParentHeight = 0;

		public ElementPosition(IGuiElement targetElement)
		{
			this.targetElement = targetElement;
		}

		public void Recalculate(float parentWidth, float parentHeight)
		{
			if (parentHeight == this.lastParentHeight && parentWidth == this.lastParentWidth)
				return;

			this.lastParentWidth = parentWidth;
			this.lastParentHeight = parentHeight;

			foreach (var positioner in this.positioners)
				positioner.Recalculate(this, parentWidth, parentHeight);
		}

		#region Position builders
		public ElementPosition FixedSize(float width, float height)
		{
			this.Size = new Vector2(width, height);

			return this;
		}

		public ElementPosition ParentRelative(float x, float y, float marginX, float marginY)
		{
			this.positioners.Add(new ParentRelativePositioner(x, y, marginX, marginY));

			return this;
		}
		public ElementPosition WrapContent()
		{
			this.positioners.Add(new WrapContentPositioner());

			return this;
		}
		#endregion

		private float contentWidth()
		{
			return this.targetElement.ContentWidth();
		}

		private float contentHeight()
		{
			return this.targetElement.ContentHeight();
		}

		private interface IPositioner
		{
			void Recalculate(ElementPosition element, float parentWidth, float parentHeight);
		}

		private class ParentRelativePositioner : IPositioner
		{
			private float xPortion;
			private float yPortion;
			private float marginX;
			private float marginY;

			public ParentRelativePositioner(float x, float y, float marginX, float marginY)
			{
				this.marginX = marginX;
				this.marginY = marginY;
				this.xPortion = x;
				this.yPortion = y;
			}

			public void Recalculate(ElementPosition element, float parentWidth, float parentHeight)
			{
				float windowX = parentWidth - 2 * marginX - element.Size.X / 2;
				float windowY = parentHeight - 2 * marginY - element.Size.Y / 2;

				element.Center = new Vector2(
					this.marginX + windowX * this.xPortion,
					this.marginY + windowY * this.yPortion
				);
			}
		}

		private class WrapContentPositioner : IPositioner
		{
			public void Recalculate(ElementPosition element, float parentWidth, float parentHeight)
			{
				element.Size = new Vector2(
					element.contentWidth(),
					element.contentHeight()
				);
			}
		}
	}
}
