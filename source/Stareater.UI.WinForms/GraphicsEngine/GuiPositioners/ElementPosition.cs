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
		private float marginX;
		private float marginY;
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
			if (this.parentPosition == null && this.positioners.Count > 0)
				return;

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

		public Vector2 Margins 
		{
			get => new Vector2(this.marginX, this.marginY);
			set
			{
				this.marginX = value.X;
				this.marginY = value.Y;
				this.Recalculate();
			}
		}

		public ElementPosition Offset(float x, float y)
		{
			this.positioners.Add(new ConstantOffsetPositiner(x, y));

			return this;
		}

		public ElementPosition Offset(ValueReference<Vector2> offset)
		{
			this.positioners.Add(new OffsetPositiner(offset));

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

		public ElementPosition StretchBottomTo(IGuispaceElement anchor, float yPortionAnchor)
		{
			this.positioners.Add(new StretchToPositioner(
				anchor.Position,
				new Vector2(0, yPortionAnchor), new Vector2(0, 1),
				new Vector2(0, 1)
			));
			this.dependsOn(anchor.Position);

			return this;
		}

		public ElementPosition StretchRightTo(IGuispaceElement anchor, float xPortionAnchor)
		{
			this.positioners.Add(new StretchToPositioner(
				anchor.Position,
				new Vector2(xPortionAnchor, 0), new Vector2(-1, 0),
				new Vector2(1, 0)
			));
			this.dependsOn(anchor.Position);

			return this;
		}

		public ElementPosition TooltipNear(Vector2 sourcePoint, float sourceMargin, float parentMargin)
		{
			this.positioners.Add(new TooltipPositioner(sourcePoint, sourceMargin, parentMargin));

			return this;
		}
		#endregion

		private class ConstantOffsetPositiner : IPositioner
		{
			private Vector2 offset;

			public ConstantOffsetPositiner(float x, float y)
			{
				this.offset = new Vector2(x, y);
			}

			public void Recalculate(ElementPosition element, ElementPosition parentPosition)
			{
				element.Center += this.offset;
			}
		}

		private class OffsetPositiner : IPositioner
		{
			private readonly ValueReference<Vector2> offset;

			public OffsetPositiner(ValueReference<Vector2> offset)
			{
				this.offset = offset;
			}

			public void Recalculate(ElementPosition element, ElementPosition parentPosition)
			{
				element.Center += this.offset.Value;
			}
		}

		private class ParentRelativePositioner : IOutsidePositioner
		{
			private readonly float xPortion;
			private readonly float yPortion;
			private bool hasMargins = false;

			public ParentRelativePositioner(float x, float y)
			{
				this.xPortion = x;
				this.yPortion = y;
			}

			public void UseMargins()
			{
				this.hasMargins = true;
			}

			public void Recalculate(ElementPosition element, ElementPosition parentPosition)
			{
				float marginX = hasMargins ? element.marginX : 0;
				float marginY = hasMargins ? element.marginY : 0;
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
			private bool hasMargins = false;

			public RelativeToPositioner(ElementPosition anchor, float xPortionAnchor, float yPortionAnchor, float xPortionThis, float yPortionThis)
			{
				this.anchor = anchor;
				this.xPortionAnchor = xPortionAnchor;
				this.yPortionAnchor = yPortionAnchor;
				this.xPortionThis = xPortionThis;
				this.yPortionThis = yPortionThis;
			}

			public void UseMargins()
			{
				this.hasMargins = true;
			}

			public void Recalculate(ElementPosition element, ElementPosition parentPosition)
			{
				float marginX = hasMargins ? element.marginX : 0;
				float marginY = hasMargins ? element.marginY : 0;

				element.Center = new Vector2(
					this.anchor.Center.X + (this.anchor.Size.X + marginX) * this.xPortionAnchor / 2 - element.Size.X * this.xPortionThis / 2,
					this.anchor.Center.Y + (this.anchor.Size.Y + marginY) * this.yPortionAnchor / 2 - element.Size.Y * this.yPortionThis / 2
				);
			}
		}

		private class StretchToPositioner : IPositioner
		{
			private readonly ElementPosition anchor;
			private readonly Vector2 anchorPortion;
			private readonly Vector2 oppositePortion;
			private readonly Vector2 marginFilter;
			private readonly Vector2 strechAxis;
			private readonly Vector2 preservedAxis;

			public StretchToPositioner(ElementPosition anchor, Vector2 anchorPortion, Vector2 oppositePortion, Vector2 marginFilter)
			{
				this.anchor = anchor;
				this.anchorPortion = anchorPortion;
				this.oppositePortion = oppositePortion;
				this.marginFilter = marginFilter;

				this.strechAxis = abs(oppositePortion);
				this.preservedAxis = abs(this.strechAxis.PerpendicularLeft);
			}

			public void Recalculate(ElementPosition element, ElementPosition parentPosition)
			{
				var margin = this.marginFilter * new Vector2(element.marginX, element.marginY);
				var oppositeEnd = element.Center + element.Size * this.oppositePortion / 2;
				var anchorEnd = this.anchor.Center + this.anchorPortion * (this.anchor.Size / 2 - margin);

				element.Center = element.Center * this.preservedAxis + (oppositeEnd + anchorEnd) * this.strechAxis / 2;
				element.Size = element.Size * this.preservedAxis + abs(oppositeEnd - anchorEnd) * this.strechAxis;
			}

			private static Vector2 abs(Vector2 v) => new Vector2(Math.Abs(v.X), Math.Abs(v.Y));
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
			private ValueReference<Vector2> padding = new ValueReference<Vector2>();

			public void Padding(ValueReference<Vector2> padding)
			{
				this.padding = padding;
			}

			public void Recalculate(ElementPosition element, ElementPosition parentPosition)
			{
				element.Size = element.contentSize() + 2 * this.padding.Value;
			}
		}
	}
}
