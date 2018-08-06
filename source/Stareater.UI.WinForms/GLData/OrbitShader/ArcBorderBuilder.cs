using Stareater.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stareater.GLData.OrbitShader
{
	class ArcBorderBuilder
	{
		private readonly List<Circle> wholeCircles = new List<Circle>();
		private readonly Dictionary<Circle, Queue<ArcPoint>> arcPoints = new Dictionary<Circle, Queue<ArcPoint>>();

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

		private static Queue<ArcPoint> arcsToPoints(List<Arc> arcs)
		{
			var points = new LinkedList<ArcPoint>(arcs.
				SelectMany(x => x.Points).
				OrderBy(x => x.Angle).
				ThenBy(x => x.RightEnd)
			);

			//Find the start of an arc
			var originalFirst = points.First.Value;
			while (!points.First.Value.RightEnd || points.Last.Value.RightEnd)
			{
				//Rotate the list
				var first = points.First.Value;
				points.RemoveFirst();
				points.AddLast(first);

				//Check if rotation went full circle, if so then
				//all arcs are overlapping in the exclusive manner
				if (points.First.Value == originalFirst)
					return new Queue<ArcPoint>();
			}

			//Remove excluded points from overlapping arcs
			for (var current = points.First; current != points.Last; /* no step */)
				if (current.Value.RightEnd == current.Next.Value.RightEnd)
				{
					if (current.Value.RightEnd)
					{
						current = current.Next;
						points.Remove(current.Previous);
					}
					else
						points.Remove(current.Next);
				}
				else
					current = current.Next;

			return new Queue<ArcPoint>(points);
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
				while (points.Any())
				{
					var rightPoint = points.Dequeue();
					var leftPoint = points.Dequeue();

					if (rightPoint.OriginalArc == leftPoint.OriginalArc)
						singleArcVertices(rightPoint.OriginalArc, data);
					else
						multipleArcVertices(rightPoint, leftPoint, data);
				}

				yield return new ArcVertices(data, circle.Key.Center, (float)circle.Key.Radius);
			}
		}

		private void singleArcVertices(Arc arc, List<float> outputData)
		{
			var left = arc.Facing * arc.Origin.Radius;
			var right = arc.Facing * arc.Openness;
			var top = arc.Facing.PerpendicularRight * arc.Origin.Radius;

			outputData.AddRange(orbitVertex(left + top));
			outputData.AddRange(orbitVertex(right + top));
			outputData.AddRange(orbitVertex(right - top));

			outputData.AddRange(orbitVertex(right - top));
			outputData.AddRange(orbitVertex(left - top));
			outputData.AddRange(orbitVertex(left + top));
		}

		private void multipleArcVertices(ArcPoint rightPoint, ArcPoint leftPoint, List<float> outputData)
		{
			var circle = rightPoint.OriginalArc.Origin;
			var faceDirection = rightPoint.Point != -leftPoint.Point ? (rightPoint.Point + leftPoint.Point) : rightPoint.Point.PerpendicularLeft;

			if (faceDirection.PerpendicularRight.Dot(rightPoint.Point) < 0 || rightPoint.Point == leftPoint.Point)
				faceDirection = -faceDirection;
			faceDirection = faceDirection.Unit;
			var facing = faceDirection * circle.Radius;
			var openness = faceDirection.Dot(rightPoint.Point);

			var midpointOpenness = (openness - 1) / 2 + 1;
			var midpointHeight = (float)Math.Sqrt(circle.Radius * circle.Radius - midpointOpenness * midpointOpenness);
			var facingPoint = facing * 1.5f;

			var outerPoint1 = (facing * midpointOpenness + facing.PerpendicularRight * midpointHeight) * 1.9f;
			outputData.AddRange(orbitVertex(rightPoint.Point * 1.9f));
			outputData.AddRange(new float[] { 0, 0, 0, 0 });
			outputData.AddRange(orbitVertex(outerPoint1));

			outputData.AddRange(new float[] { 0, 0, 0, 0 });
			outputData.AddRange(orbitVertex(facingPoint));
			outputData.AddRange(orbitVertex(outerPoint1));

			var outerPoint2 = (facing * midpointOpenness + facing.PerpendicularLeft * midpointHeight) * 1.9f;
			outputData.AddRange(orbitVertex(leftPoint.Point * 1.9f));
			outputData.AddRange(orbitVertex(outerPoint2));
			outputData.AddRange(new float[] { 0, 0, 0, 0 });

			outputData.AddRange(new float[] { 0, 0, 0, 0 });
			outputData.AddRange(orbitVertex(outerPoint2));
			outputData.AddRange(orbitVertex(facingPoint));
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
