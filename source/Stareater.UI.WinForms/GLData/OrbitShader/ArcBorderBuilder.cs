using Stareater.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stareater.GLData.OrbitShader
{
	class ArcBorderBuilder
	{
		private readonly List<Circle> wholeCircles = new List<Circle>();
		private readonly Dictionary<Circle, List<ArcPoint>> arcPoints = new Dictionary<Circle, List<ArcPoint>>();

		public void AddCircles(IEnumerable<Circle> circles)
		{
			foreach (var circle in circles)
			{
				var arcs = new List<Arc>();
				bool enclosed = false;

				foreach (var other in circles.Where(x => x != circle))
				{
					var dist = (float)(circle.Center - other.Center).Length;

					if (dist > circle.Radius + other.Radius)
						continue;
					if (dist + circle.Radius < other.Radius)
					{
						enclosed = true;
						break;
					}

					var facing = (circle.Center - other.Center).Unit;

					arcs.Add(new Arc(
						circle,
						facing,
						-(dist * dist - other.Radius * other.Radius + circle.Radius * circle.Radius) / 2 / dist
					));
				}

				if (!enclosed && !arcs.Any())
					this.wholeCircles.Add(new Circle(circle.Center, circle.Radius));

				if (!enclosed && arcs.Any())
					arcPoints[circle] = arcsToPoints(arcs);
			}
		}

		private static int comparePoints(ArcPoint pointA, ArcPoint pointB)
		{
			var angleCompare = pointA.Angle.CompareTo(pointB.Angle);

			if (angleCompare != 0)
				return angleCompare;

			if (pointA.RightEnd != pointB.RightEnd)
				return pointA.RightEnd ? 1 : -1;
			else
				return 0;
		}

		private static List<ArcPoint> arcsToPoints(List<Arc> arcs)
		{
			var points = arcs.SelectMany(x => x.Points).ToList();
			points.Sort(comparePoints);

			//Find the start of an arc
			int startI = -1;
			for (int i = 0; i < points.Count; i++)
			{
				var previous = (i - 1 + points.Count) % points.Count;
				if (points[i].RightEnd && !points[previous].RightEnd)
				{
					startI = i;
					break;
				}
			}

			//All arcs are overlapping in the exclusive manner
			if (startI < 0)
				return new List<ArcPoint>();

			//Rotate the list
			points = points.Skip(startI).Concat(points.Take(startI)).ToList();
			
			//Remove excluded points from overlapping arcs
			for (int i = 0; i < points.Count - 1; i++)
				if (points[i].RightEnd == points[i + 1].RightEnd)
				{
					if (points[i].RightEnd)
						points.RemoveAt(i);
					else
						points.RemoveAt(i + 1);
					i--;
				}

			return points;
		}

		public int Count
		{
			get { return this.arcPoints.Count + this.wholeCircles.Count; }
		}

		public IEnumerable<ArcVertices> Vertices()
		{
			foreach(var circle in this.wholeCircles)
				yield return new ArcVertices(
					OrbitHelpers.Quad((float)circle.Radius),
					circle.Center, (float)circle.Radius
				);

			foreach (var circle in this.arcPoints)
			{
				var points = circle.Value;
				var data = new List<float>();

				//TODO(v0.8) handle tessellation
				for (int i = 0; i < points.Count; i += 2)
				{
					var rightPoint = points[i];
					var leftPoint = points[i + 1];
					var faceDirection = rightPoint.Point != -leftPoint.Point ? (rightPoint.Point + leftPoint.Point) : rightPoint.Point.PerpendicularLeft;

					if (faceDirection.PerpendicularRight.Dot(rightPoint.Point) < 0 || rightPoint.Point == leftPoint.Point)
						faceDirection = -faceDirection;
					faceDirection = faceDirection.Unit;
					var facing = faceDirection * circle.Key.Radius;
					var openness = faceDirection.Dot(rightPoint.Point);

					var midpointOpenness = (openness - 1) / 2 + 1;
					var midpointHeight = (float)Math.Sqrt(circle.Key.Radius * circle.Key.Radius - midpointOpenness * midpointOpenness);
					var facingPoint = facing * 1.5f;

					var outerPoint1 = (facing * midpointOpenness + facing.PerpendicularRight * midpointHeight) * 1.9f;
					data.AddRange(orbitVertex(rightPoint.Point * 1.9f));
					data.AddRange(new float[] { 0, 0, 0, 0 });
					data.AddRange(orbitVertex(outerPoint1));

					data.AddRange(new float[] { 0, 0, 0, 0 });
					data.AddRange(orbitVertex(facingPoint));
					data.AddRange(orbitVertex(outerPoint1));

					var outerPoint2 = (facing * midpointOpenness + facing.PerpendicularLeft * midpointHeight) * 1.9f;
					data.AddRange(orbitVertex(leftPoint.Point * 1.9f));
					data.AddRange(orbitVertex(outerPoint2));
					data.AddRange(new float[] { 0, 0, 0, 0 });

					data.AddRange(new float[] { 0, 0, 0, 0 });
					data.AddRange(orbitVertex(outerPoint2));
					data.AddRange(orbitVertex(facingPoint));
				}

				yield return new ArcVertices(data, circle.Key.Center, (float)circle.Key.Radius);
			}
		}

		private static IEnumerable<float> orbitVertex(Vector2D p)
		{
			yield return (float)p.X;
			yield return (float)p.Y;
			yield return (float)p.X;
			yield return (float)p.Y;
		}

		class Arc
		{
			public Circle Origin { get; private set; }
			public Vector2D Facing { get; private set; }
			public double Openness { get; private set; }

			public Arc(Circle origin, Vector2D facing, double openness)
			{
				this.Origin = origin;
				this.Facing = facing;
				this.Openness = openness;
			}

			public IEnumerable<ArcPoint> Points
			{
				get
				{
					var openningHeight = (float)Math.Sqrt(this.Origin.Radius * this.Origin.Radius - this.Openness * this.Openness);

					yield return new ArcPoint(this, this.Facing * this.Openness + this.Facing.PerpendicularRight * openningHeight, true);
					yield return new ArcPoint(this, this.Facing * this.Openness + this.Facing.PerpendicularLeft * openningHeight, false);
				}
			}
		}

		class ArcPoint
		{
			public Arc OriginalArc { get; private set; }
			public Vector2D Point { get; private set; }
			public bool RightEnd { get; private set; }

			public double Angle { get; private set; }

			public ArcPoint(Arc originalArc, Vector2D point, bool rightEnd)
			{
				this.OriginalArc = originalArc;
				this.Point = point;
				this.RightEnd = rightEnd;

				var direction = new Vector2D(this.Point.X, this.Point.Y).Unit;
				this.Angle = (direction.Y > 0) ? 1 - direction.X : 3 + direction.X;
			}
		}
	}
}
