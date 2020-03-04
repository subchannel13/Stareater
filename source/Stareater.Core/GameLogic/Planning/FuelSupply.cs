using Stareater.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.GameLogic.Planning
{
	class FuelSupply
	{
		private readonly Vector2D distanceComponents;
		private readonly Vector2D destination;
		private readonly Vector2D source;
		private readonly Vector2D lineStart;
		private readonly Vector2D lineEnd;

		public FuelSupply(Vector2D destination, Vector2D source, Vector2D lineStart, Vector2D lineEnd)
		{
			this.distanceComponents = calcDistanceComponents(source, destination, lineStart, lineEnd);
			this.destination = destination;
			this.source = source;
			this.lineStart = lineStart;
			this.lineEnd = lineEnd;
		}

		public double Distance { get => this.distanceComponents.Length; }

		public FuelSupply Improve(Vector2D newSource)
		{
			if (this.isCloser(newSource))
				return new FuelSupply(this.destination, newSource, this.lineStart, this.lineEnd);

			return this;
		}

		private bool isCloser(Vector2D center)
		{
			return this.distanceComponents.Length > (this.destination - center).Length;
		}

		public static FuelSupply Intersection(FuelSupply oldSuplyLeft, FuelSupply oldSuplyRight, Vector2D newSource)
		{
			if (oldSuplyLeft.isCloser(newSource) == oldSuplyRight.isCloser(newSource))
				return null;

			var newDist = calcDistanceComponents(newSource, oldSuplyLeft.destination, oldSuplyLeft.lineStart, oldSuplyLeft.lineEnd);
			var oldComponents = oldSuplyLeft.distanceComponents;
			var t = (newDist.Dot(newDist) - oldComponents.Dot(oldComponents)) / (oldComponents.X - newDist.X) / 2;

			return new FuelSupply(
				oldSuplyLeft.destination + t * (oldSuplyLeft.lineEnd - oldSuplyLeft.lineStart).Unit,
				newSource,
				oldSuplyLeft.lineStart, oldSuplyLeft.lineEnd
			);
		}

		private static Vector2D calcDistanceComponents(Vector2D fromPoint, Vector2D toPoint, Vector2D lineStart, Vector2D lineEnd)
		{
			var lineDir = (lineEnd - lineStart).Unit;
			var delta = toPoint - fromPoint;

			return new Vector2D(
				delta.Dot(lineDir),
				Math.Abs(delta.Dot(lineDir.PerpendicularLeft))
			);
		}
	}
}
