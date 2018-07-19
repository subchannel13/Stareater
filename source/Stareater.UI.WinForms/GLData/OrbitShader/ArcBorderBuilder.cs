using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stareater.GLData.OrbitShader
{
	class ArcBorderBuilder
	{
		private readonly Dictionary<Arc, List<ArcPoint>> arcPoints = new Dictionary<Arc, List<ArcPoint>>();

		public void AddCircles<T>(IEnumerable<T> data, Func<T, Vector2> centerFunc, Func<T, float> radiusFunc)
		{
			var circles = data.
				Select(x => new Arc(centerFunc(x), new Vector2(radiusFunc(x), 0), -radiusFunc(x))).
				ToList();

			foreach (var circle in circles)
			{
				var arcs = new List<Arc>();
				bool enclosed = false;
				foreach (var otherCircle in circles.Where(x => x != circle))
				{
					var dist = (circle.Center - otherCircle.Center).Length;
					var circleRadius = circle.Radius;
					var otherRadius = otherCircle.Radius;

					if (dist > circleRadius + otherRadius)
						continue;
					if (dist + circleRadius < otherRadius)
					{
						arcs.Clear();
						enclosed = true;
						break;
					}

					arcs.Add(new Arc(
						circle.Center,
						(circle.Center - otherCircle.Center).Normalized() * circleRadius,
						-(dist * dist - otherRadius * otherRadius + circleRadius * circleRadius) / 2 / dist
					));
				}

				if (!enclosed && !arcs.Any())
					arcs.Add(new Arc(circle.Center, circle.Facing, circle.Openness));

				if (!arcs.Any())
					continue;

				var points = arcs.SelectMany(x => x.Points).ToList();
				points.Sort(comparePoints);
				int startI = -1;
				for (int i = 0; i < points.Count; i++)
				{
					var prev = (i + points.Count - 1) % points.Count;
					if (points[i].RightEnd && !points[prev].RightEnd)
					{
						startI = i;
						break;
					}
				}

				if (startI < 0)
					continue;

				points = points.Skip(startI).Concat(points.Take(startI)).ToList();
				for (int i = 0; i < points.Count; i++)
				{
					if (i + 1 < points.Count && points[i].RightEnd == points[i + 1].RightEnd)
					{
						if (points[i].RightEnd)
							points.RemoveAt(i);
						else
							points.RemoveAt(i + 1);
						i--;
					}
				}

				arcPoints[circle] = points;
			}
		}

		public int Count
		{
			get { return this.arcPoints.Count; }
		}

		public IEnumerable<ArcVertices> Vertices()
		{
			foreach (var circle in this.arcPoints)
			{
				var points = circle.Value;
				var data = new List<float>();

				for (int i = 0; i < points.Count; i += 2)
				{
					var rightPoint = points[i];
					var leftPoint = points[i + 1];
					var faceDirection = rightPoint.Point != -leftPoint.Point ? (rightPoint.Point + leftPoint.Point) : rightPoint.Point.PerpendicularLeft;

					if (Vector2.Dot(faceDirection.PerpendicularRight, rightPoint.Point) < 0 || rightPoint.Point == leftPoint.Point)
						faceDirection = -faceDirection;
					faceDirection = faceDirection.Normalized();
					var facing = faceDirection * rightPoint.OriginalArc.Radius;
					var openness = Vector2.Dot(faceDirection, rightPoint.Point);

					var midpointOpenness = (openness - 1) / 2 + 1;
					var midpointHeight = (float)Math.Sqrt(facing.LengthSquared - midpointOpenness * midpointOpenness);
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

				yield return new ArcVertices(data, circle.Key.Center, circle.Key.Radius);
			}
		}

		private static IEnumerable<float> orbitVertex(Vector2 p)
		{
			yield return p.X;
			yield return p.Y;
			yield return p.X;
			yield return p.Y;
		}

		private int comparePoints(ArcPoint pointA, ArcPoint pointB)
		{
			var angleCompare = pointA.Angle.CompareTo(pointB.Angle);

			if (angleCompare != 0)
				return angleCompare;

			if (pointA.RightEnd != pointB.RightEnd)
				return pointA.RightEnd ? 1 : -1;
			else
				return 0;
		}

		class Arc
		{
			public Vector2 Center { get; private set; }
			public Vector2 Facing { get; private set; }
			public float Openness { get; private set; }

			public Arc(Vector2 center, Vector2 facing, float openness)
			{
				this.Center = center;
				this.Facing = facing;
				this.Openness = openness;
			}

			public float Radius
			{
				get { return this.Facing.Length; }
			}

			public IEnumerable<ArcPoint> Points
			{
				get
				{
					var openningHeight = (float)Math.Sqrt(this.Facing.LengthSquared - this.Openness * this.Openness);
					var faceDirection = this.Facing.Normalized();

					yield return new ArcPoint(this, faceDirection * this.Openness + faceDirection.PerpendicularRight * openningHeight, true);
					yield return new ArcPoint(this, faceDirection * this.Openness + faceDirection.PerpendicularLeft * openningHeight, false);
				}
			}
		}

		class ArcPoint
		{
			public Arc OriginalArc { get; private set; }
			public Vector2 Point { get; private set; }
			public bool RightEnd { get; private set; }

			public float Angle { get; private set; }

			public ArcPoint(Arc originalArc, Vector2 point, bool rightEnd)
			{
				this.OriginalArc = originalArc;
				this.Point = point;
				this.RightEnd = rightEnd;

				var direction = this.Point.Normalized();
				this.Angle = (direction.Y > 0) ? 1 - direction.X : 3 + direction.X;
			}
		}
	}
}
