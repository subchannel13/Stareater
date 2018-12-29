using OpenTK;
using System;
using System.Collections.Generic;

namespace Stareater.GraphicsEngine.GuiElements
{
	class ElementPosition
	{
		public Vector2 Center { get; private set; }
		public Vector2 Size { get; private set; }
		public ClipWindow ClipArea { get; private set; }

		private readonly List<IPositioner> positioners = new List<IPositioner>();
		private readonly Func<float> contentWidth;
		private readonly Func<float> contentHeight;

		public ElementPosition(Func<float> contentWidth, Func<float> contentHeight)
		{
			this.contentWidth = contentWidth;
			this.contentHeight = contentHeight;
		}

		public void Recalculate(ElementPosition parentPosition)
		{
			foreach (var positioner in this.positioners)
				positioner.Recalculate(this, parentPosition);

			if (parentPosition != null)
				this.ClipArea = new ClipWindow(this.Center, this.Size, parentPosition.ClipArea);
			else
				this.ClipArea = new ClipWindow(this.Center, this.Size);
		}

		#region Position builders
		public ElementPosition FixedSize(float width, float height)
		{
			this.Size = new Vector2(width, height);

			return this;
		}

		public ElementPosition FixedCenter(float x, float y)
		{
			this.Center = new Vector2(x, y);

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

		public ElementPosition RelativeTo(GuiButton anchor, float xPortionAnchor, float yPortionAnchor, float xPortionThis, float yPortionThis, float marginX, float marginY)
		{
			this.positioners.Add(new RelativeToPositioner(anchor, xPortionAnchor, yPortionAnchor, xPortionThis, yPortionThis, marginX, marginY));

			return this;
		}
		#endregion

		private interface IPositioner
		{
			void Recalculate(ElementPosition element, ElementPosition parentPosition);
		}

		private class ParentRelativePositioner : IPositioner
		{
			private readonly float xPortion;
			private readonly float yPortion;
			private readonly float marginX;
			private readonly float marginY;

			public ParentRelativePositioner(float x, float y, float marginX, float marginY)
			{
				this.marginX = marginX;
				this.marginY = marginY;
				this.xPortion = x;
				this.yPortion = y;
			}

			public void Recalculate(ElementPosition element, ElementPosition parentPosition)
			{
				float windowX = parentPosition.Size.X / 2 - marginX - element.Size.X / 2;
				float windowY = parentPosition.Size.Y / 2 - marginY - element.Size.Y / 2;

				element.Center = new Vector2(
					windowX * this.xPortion + parentPosition.Center.X,
					windowY * this.yPortion + parentPosition.Center.Y
				);
			}
		}

		private class RelativeToPositioner : IPositioner
		{
			private readonly GuiButton anchor;
			private readonly float xPortionAnchor;
			private readonly float yPortionAnchor;
			private readonly float xPortionThis;
			private readonly float yPortionThis;
			private readonly float marginX;
			private readonly float marginY;

			public RelativeToPositioner(GuiButton anchor, float xPortionAnchor, float yPortionAnchor, float xPortionThis, float yPortionThis, float marginX, float marginY)
			{
				this.anchor = anchor;
				this.xPortionAnchor = xPortionAnchor;
				this.yPortionAnchor = yPortionAnchor;
				this.xPortionThis = xPortionThis;
				this.yPortionThis = yPortionThis;
				this.marginX = marginX;
				this.marginY = marginY;
			}

			public void Recalculate(ElementPosition element, ElementPosition parentPosition)
			{
				element.Center = new Vector2(
					this.anchor.Position.Center.X + (this.anchor.Position.Size.X + this.marginX) * this.xPortionAnchor / 2 - element.Size.X * this.xPortionThis / 2,
					this.anchor.Position.Center.Y + (this.anchor.Position.Size.Y + this.marginY) * this.yPortionAnchor / 2 - element.Size.Y * this.yPortionThis / 2
				);
			}
		}

		private class WrapContentPositioner : IPositioner
		{
			public void Recalculate(ElementPosition element, ElementPosition parentPosition)
			{
				element.Size = new Vector2(
					element.contentWidth(),
					element.contentHeight()
				);
			}
		}
	}
}
