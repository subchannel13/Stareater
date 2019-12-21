using OpenTK;
using Stareater.GLData;
using Stareater.GraphicsEngine.GuiElements;
using Stareater.GuiUtils;
using Stareater.Utils;
using System;
using System.Collections.Generic;

namespace Stareater.GraphicsEngine.GuiPositioners
{
	class ElementPosition
	{
		/// <summary>
		/// GUI element center.
		/// </summary>
		public Vector2 Center { get; private set; }
		/// <summary>
		/// Full GUI element size (not half-width and half-height).
		/// </summary>
		public Vector2 Size { get; private set; }
		public ClipWindow ClipArea { get; private set; }

		public event Action OnReposition;

		private readonly List<IPositioner> positioners = new List<IPositioner>();
		private readonly Func<Vector2> contentSize;
		private ElementPosition parentPosition = null;
		private readonly RepeatGate calculationGate = new RepeatGate();

		public ElementPosition(Func<Vector2> contentSize)
		{
			this.contentSize = contentSize;
			this.ClipArea = new ClipWindow();
		}

		private void dependsOn(ElementPosition dependency)
		{
			dependency.OnReposition -= this.Recalculate;
			dependency.OnReposition += this.Recalculate;
		}

		public virtual void Attach(IGuispaceElement parent)
		{
			this.parentPosition = parent.Position;
			this.parentPosition.OnReposition += this.Recalculate;
		}

		public void Detach()
		{
			this.parentPosition.OnReposition -= this.Recalculate;
			this.parentPosition = null;
		}

		public void Recalculate()
		{
			while (this.calculationGate.TryEnter())
			{
				var oldCenter = this.Center;
				var oldSize = this.Size;
				var oldClipArea = this.ClipArea;

				foreach (var positioner in this.positioners)
					positioner.Recalculate(this, this.parentPosition);

				if (this.parentPosition != null)
					this.ClipArea = new ClipWindow(this.Center, this.Size, this.parentPosition.ClipArea);
				else
					this.ClipArea = new ClipWindow(this.Center, this.Size);

				if (this.Center != oldCenter || this.Size != oldSize || this.ClipArea != oldClipArea)
					this.OnReposition();
			}

			this.calculationGate.Finish();
		}

		public void Propagate()
		{
			this.OnReposition?.Invoke();
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

		public ElementPosition Offset(float x, float y)
		{
			this.positioners.Add(new OffsetPositiner(x, y));

			return this;
		}

		public OutsidePosition ParentRelative(float x, float y)
		{
			var positioner = new ParentRelativePositioner(x, y);
			this.positioners.Add(positioner);

			return new OutsidePosition(this, positioner);
		}

		public WrapPosition WrapContent()
		{
			var positioner = new WrapContentPositioner();
			this.positioners.Add(positioner);

			return new WrapPosition(this, positioner);
		}

		public OutsidePosition RelativeTo(IGuispaceElement anchor)
		{
			return this.RelativeTo(anchor, 0, 0, 0, 0);
		}

		public OutsidePosition RelativeTo(IGuispaceElement anchor, float xPortionAnchor, float yPortionAnchor, float xPortionThis, float yPortionThis)
		{
			var positioner = new RelativeToPositioner(anchor.Position, xPortionAnchor, yPortionAnchor, xPortionThis, yPortionThis);
			this.positioners.Add(positioner);
			this.dependsOn(anchor.Position);

			return new OutsidePosition(this, positioner);
		}

		public ElementPosition StretchBottomTo(IGuispaceElement anchor, float yPortionAnchor, float marginY)
		{
			this.positioners.Add(new StretchBottomToPositioner(anchor.Position, yPortionAnchor, marginY));
			this.dependsOn(anchor.Position);

			return this;
		}

		public ElementPosition StretchRightTo(IGuispaceElement anchor, float xPortionAnchor, float marginX)
		{
			this.positioners.Add(new StretchRightToPositioner(anchor.Position, xPortionAnchor, marginX));
			this.dependsOn(anchor.Position);

			return this;
		}

		public ElementPosition TooltipNear(Vector2 sourcePoint, float sourceMargin, float parentMargin)
		{
			this.positioners.Add(new TooltipPositioner(sourcePoint, sourceMargin, parentMargin));

			return this;
		}
		#endregion

		private class OffsetPositiner : IPositioner
		{
			private Vector2 offset;

			public OffsetPositiner(float x, float y)
			{
				this.offset = new Vector2(x, y);
			}

			public void Recalculate(ElementPosition element, ElementPosition parentPosition)
			{
				element.Center += this.offset;
			}
		}

		private class ParentRelativePositioner : IOutsidePositioner
		{
			private readonly float xPortion;
			private readonly float yPortion;
			private float marginX;
			private float marginY;

			public ParentRelativePositioner(float x, float y)
			{
				this.xPortion = x;
				this.yPortion = y;
			}

			public void Margins(float marginX, float marginY)
			{
				this.marginX = marginX;
				this.marginY = marginY;
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

		private class RelativeToPositioner : IOutsidePositioner
		{
			private readonly ElementPosition anchor;
			private readonly float xPortionAnchor;
			private readonly float yPortionAnchor;
			private readonly float xPortionThis;
			private readonly float yPortionThis;
			private float marginX;
			private float marginY;

			public RelativeToPositioner(ElementPosition anchor, float xPortionAnchor, float yPortionAnchor, float xPortionThis, float yPortionThis)
			{
				this.anchor = anchor;
				this.xPortionAnchor = xPortionAnchor;
				this.yPortionAnchor = yPortionAnchor;
				this.xPortionThis = xPortionThis;
				this.yPortionThis = yPortionThis;
			}

			public void Margins(float marginX, float marginY)
			{
				this.marginX = marginX;
				this.marginY = marginY;
			}

			public void Recalculate(ElementPosition element, ElementPosition parentPosition)
			{
				element.Center = new Vector2(
					this.anchor.Center.X + (this.anchor.Size.X + this.marginX) * this.xPortionAnchor / 2 - element.Size.X * this.xPortionThis / 2,
					this.anchor.Center.Y + (this.anchor.Size.Y + this.marginY) * this.yPortionAnchor / 2 - element.Size.Y * this.yPortionThis / 2
				);
			}
		}

		//Todo(v0.9) try to unify stretch positioners
		private class StretchBottomToPositioner : IPositioner
		{
			private readonly ElementPosition anchor;
			private readonly float yPortionAnchor;
			private readonly float marginY;

			public StretchBottomToPositioner(ElementPosition anchor, float yPortionAnchor, float marginY)
			{
				this.anchor = anchor;
				this.yPortionAnchor = yPortionAnchor;
				this.marginY = marginY;
			}

			public void Recalculate(ElementPosition element, ElementPosition parentPosition)
			{
				var top = element.Center.Y + element.Size.Y / 2;
				var bottom = this.anchor.Center.Y + yPortionAnchor * (this.anchor.Size.Y / 2 - this.marginY);

				element.Center = new Vector2(element.Center.X, (top + bottom) / 2);
				element.Size = new Vector2(element.Size.X, top - bottom);
			}
		}

		private class StretchRightToPositioner : IPositioner
		{
			private readonly ElementPosition anchor;
			private readonly float xPortionAnchor;
			private readonly float marginX;

			public StretchRightToPositioner(ElementPosition anchor, float xPortionAnchor, float marginX)
			{
				this.anchor = anchor;
				this.xPortionAnchor = xPortionAnchor;
				this.marginX = marginX;
			}

			public void Recalculate(ElementPosition element, ElementPosition parentPosition)
			{
				var widthDelta = this.anchor.Center.X + xPortionAnchor * (this.anchor.Size.X / 2 - this.marginX) -
					(element.Center.X + element.Size.X / 2);

				element.Center = new Vector2(element.Center.X + widthDelta / 2, element.Center.Y);
				element.Size = new Vector2(element.Size.X + widthDelta, element.Size.Y);
			}
		}

		private class TooltipPositioner : IPositioner
		{
			private readonly Vector2 sourcePoint;
			private readonly float sourceMargin;
			private readonly float parentMargin;

			public TooltipPositioner(Vector2 sourcePoint, float sourceMargin, float parentMargin)
			{
				this.sourcePoint = sourcePoint;
				this.sourceMargin = sourceMargin;
				this.parentMargin = parentMargin;
			}

			public void Recalculate(ElementPosition element, ElementPosition parentPosition)
			{
				var leftBound = -parentPosition.Size.X / 2 + this.parentMargin;
				var rigthBound = Math.Max(parentPosition.Size.X / 2 - this.parentMargin - element.Size.X / 2, leftBound);

				var centerX = Methods.Clamp(this.sourcePoint.X + element.Size.X / 2, leftBound, rigthBound) + parentPosition.Center.X;
				var centerUpY = this.sourcePoint.Y + element.Size.Y / 2 + this.sourceMargin;

				if (centerUpY - parentPosition.Center.Y + element.Size.Y / 2 <= parentPosition.Size.Y / 2)
					element.Center = new Vector2(centerX, centerUpY);
				else
					element.Center = new Vector2(centerX, this.sourcePoint.Y - element.Size.Y / 2 - this.sourceMargin);
			}
		}

		private class WrapContentPositioner : IWrapPositioner
		{
			private float paddingX;
			private float paddingY;

			public void Padding(float paddingX, float paddingY)
			{
				this.paddingX = paddingX;
				this.paddingY = paddingY;
			}

			public void Recalculate(ElementPosition element, ElementPosition parentPosition)
			{
				element.Size = element.contentSize() + new Vector2(this.paddingX * 2, this.paddingY * 2);
			}
		}
	}
}
