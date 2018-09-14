using Stareater.Utils;
using Stareater.Utils.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stareater.GLData.OrbitShader
{
	class ArcBorderBuilder
	{
		private const float DotLimit = -1e-12f;

		private readonly Dictionary<Vector2D, Circle> wholeCircles = new Dictionary<Vector2D, Circle>();
		private readonly Dictionary<Circle, Queue<ArcPoint>> arcPoints = new Dictionary<Circle, Queue<ArcPoint>>();

		public void AddCircles(IEnumerable<Circle> circles)
		{
			var filteredCircles = new QuadTree<Circle>();
			foreach(var circle in circles)
			{
				var radius = new Vector2D(2 * circle.Radius, 2 * circle.Radius);
				var isRelevant = filteredCircles.
					Query(circle.Center, radius).
					All(other => (other.Center - circle.Center).Length + circle.Radius > other.Radius);

				if (isRelevant)
					filteredCircles.Add(circle, circle.Center, radius);
			}

			foreach (var circle in filteredCircles.GetAll())
			{
				var arcs = new Queue<Arc>();

				foreach (var other in filteredCircles.Query(circle.Center, new Vector2D(2 * circle.Radius, 2 * circle.Radius)))
				{
					var distLine = other.Center - circle.Center;
					var dist = (float)distLine.Length;

					if (dist > circle.Radius + other.Radius || dist + other.Radius < circle.Radius || dist == 0)
						continue;

					arcs.Enqueue(new Arc(
						circle,
						(circle.Center - other.Center).Unit,
						-(dist * dist - other.Radius * other.Radius + circle.Radius * circle.Radius) / 2 / dist,
						distLine
					));
				}

				if (arcs.Any())
					arcPoints[circle] = arcsToPoints(arcs);
				else
					this.wholeCircles.Add(circle.Center, new Circle(circle.Center, circle.Radius));
			}
		}

		private static Queue<ArcPoint> arcsToPoints(Queue<Arc> arcs)
		{
			var acceptedArcs = new List<ArcInterval> { new ArcInterval(arcs.Dequeue()) };

			while(arcs.Any())
			{
				var newArcs = new List<ArcInterval>();
				var newPoints = new ArcInterval(arcs.Dequeue());
				var extendedPoints = new[]
				{
					newPoints,
					newPoints.PeriodicPrevious,
					newPoints.PeriodicNext
				};

				// Calculate intersection with previous arcs
				foreach (var interval in extendedPoints)
				{
					if (acceptedArcs.All(arc => arc.MinAngle > interval.MaxAngle || arc.MaxAngle < interval.MinAngle))
						continue;

					var compareArcs = acceptedArcs.Concat(new[] { interval }).ToList();
					newArcs.Add(new ArcInterval(
						Methods.FindBest(compareArcs, arc => -arc.Left.Angle).Left,
						Methods.FindBest(compareArcs, arc => arc.Right.Angle).Right
					));
				}
				acceptedArcs = newArcs;
			}

			return new Queue<ArcPoint>(acceptedArcs.SelectMany(arc => new[] { arc.Right, arc.Left }));
		}

		public int Count
		{
			get { return this.arcPoints.Count + this.wholeCircles.Count; }
		}

		public IEnumerable<ArcVertices> Vertices()
		{
			foreach(var circle in this.wholeCircles.Values)
				yield return new ArcVertices(
					OrbitHelpers.Quad((float)circle.Radius),
					circle.Center, (float)circle.Radius
				);

			foreach (var circle in this.arcPoints)
			{
				var data = new List<float>();

				while (circle.Value.Any())
				{
					var rightPoint = circle.Value.Dequeue();
					var leftPoint = circle.Value.Dequeue();

					var facing = (rightPoint.Point + leftPoint.Point).Unit;

					if (rightPoint.Point == -leftPoint.Point)
						facing = rightPoint.Point.PerpendicularLeft.Unit;
					else if (facing.Cross(rightPoint.Point) > 0)
						facing = -facing;

					multipleArcVertices(rightPoint, facing, data);
					multipleArcVertices(leftPoint, facing, data);
				}

				yield return new ArcVertices(data, circle.Key.Center, (float)circle.Key.Radius);
			}
		}

		private void multipleArcVertices(ArcPoint point, Vector2D facing, List<float> outputData)
		{
			var circle = point.Parent.Parent;
			var left = point.EdgeFacing * circle.Radius;
			var right = point.EdgeFacing * point.EdgeFacing.Dot(point.Point);
			var top = point.Parent.Parent.Radius * (point.RightEnd ?
				point.EdgeFacing.PerpendicularRight :
				point.EdgeFacing.PerpendicularLeft);
			
			var endShape = new LinkedList<Vector2D>(new[]{
				right + top,
				right,
				left,
				left + top,
			});

			var facingNormal = point.RightEnd ? facing.PerpendicularRight : facing.PerpendicularLeft;
			for (var current = endShape.First; current != null; /* no step */)
			{
				var next = current.Next ?? endShape.First;
				var added = false;
				if ((current.Value.Dot(facingNormal) < DotLimit) != (next.Value.Dot(facingNormal) < DotLimit))
				{
					var line = next.Value - current.Value;
					var height = current.Value.Dot(facingNormal);
					var speed = -height / line.Dot(facingNormal);
					endShape.AddAfter(current, current.Value + line * speed);
					added = true;
				}

				if (current.Value.Dot(facingNormal) < DotLimit)
				{
					var realNext = current.Next;
					endShape.Remove(current);
					current = realNext;
				}
				else
					current = current.Next;

				if (added)
					current = current.Next;
			}

			var triangles = new List<Vector2D[]>();
			for (var current = endShape.First.Next.Next; current != null; current = current.Next)
				triangles.Add(makeTriangle(endShape.First.Value, current.Previous.Value, current.Value));

			var facingSide = facing.Cross(point.EdgeFacing);
			if (point.RightEnd && facingSide < 0 || !point.RightEnd && facingSide > 0)
				triangles.Add(makeTriangle(new Vector2D(0, 0), facing * circle.Radius * 1.5f, point.EdgeFacing * circle.Radius * 1.5f));

			outputData.AddRange(triangles.SelectMany(x => x).SelectMany(x => orbitVertex(x)));
		}

		private static IEnumerable<float> orbitVertex(Vector2D p)
		{
			yield return (float)p.X;
			yield return (float)p.Y;
			yield return (float)p.X;
			yield return (float)p.Y;
		}

		private Vector2D[] makeTriangle(Vector2D point1, Vector2D point2, Vector2D point3)
		{
			var triangle = new[] { point1, point2, point3 };

			var edge1 = triangle[1] - triangle[0];
			var edge2 = triangle[2] - triangle[0];
			if (edge1.Cross(edge2) > 0)
			{
				var temp = triangle[1];
				triangle[1] = triangle[2];
				triangle[2] = temp;
			}

			return triangle;
		}

		class Arc
		{
			public Circle Parent { get; private set; }
			public Vector2D Facing { get; private set; }
			public double Openness { get; private set; }

			private readonly Vector2D centerCenterLine;

			public Arc(Circle parent, Vector2D facing, double openness, Vector2D centerCenterLine)
			{
				this.Parent = parent;
				this.Facing = facing;
				this.Openness = openness;
				this.centerCenterLine = centerCenterLine;
			}

			public float OpenningHeight
			{
				get
				{
					return (float)Math.Sqrt(this.Parent.Radius * this.Parent.Radius - this.Openness * this.Openness);
				}
			}

			public IEnumerable<ArcPoint> Points
			{
				get
				{
					var height = this.OpenningHeight;
					var upperPoint = this.Facing * this.Openness + this.Facing.PerpendicularRight * height;
					var lowerPoint = this.Facing * this.Openness + this.Facing.PerpendicularLeft * height;

					yield return new ArcPoint(this, upperPoint, true, ((upperPoint - centerCenterLine).Unit - upperPoint.Unit).Unit);
					yield return new ArcPoint(this, lowerPoint, false, ((lowerPoint - centerCenterLine).Unit - lowerPoint.Unit).Unit);
				}
			}
		}

		class ArcPoint
		{
			public Arc Parent { get; private set; }
			public Vector2D Point { get; private set; }
			public bool RightEnd { get; private set; }
			public Vector2D EdgeFacing { get; private set; }

			public double Angle { get; private set; }
			
			public ArcPoint(Arc parent, Vector2D point, bool rightEnd, Vector2D edgeFacing)
			{
				this.Parent = parent;
				this.Point = point;
				this.RightEnd = rightEnd;
				this.EdgeFacing = edgeFacing;

				var direction = new Vector2D(this.Point.X, this.Point.Y).Unit;
				this.Angle = (direction.Y > 0) ? 1 - direction.X : 3 + direction.X;
			}

			public ArcPoint(ArcPoint original, double angleOffset)
			{
				this.Parent = original.Parent;
				this.Point = original.Point;
				this.RightEnd = original.RightEnd;
				this.EdgeFacing = original.EdgeFacing;

				this.Angle = original.Angle + angleOffset;
			}
		}

		class ArcInterval
		{
			public ArcPoint Left { get; private set; }
			public ArcPoint Right { get; private set; }

			public ArcInterval(Arc parent)
			{
				var points = parent.Points.ToArray();

				this.Left = points[1];
				this.Right = points[0];

				if (this.Right.Angle > this.Left.Angle)
					this.Right = new ArcPoint(this.Right, -4);
			}

			public double MinAngle
			{
				get { return this.Right.Angle; }
			}

			public double MaxAngle
			{
				get { return this.Left.Angle; }
			}

			public ArcInterval(ArcInterval original, double angleOffset)
			{
				this.Left = new ArcPoint(original.Left, angleOffset);
				this.Right = new ArcPoint(original.Right, angleOffset);
			}

			public ArcInterval(ArcPoint left, ArcPoint right)
			{
				this.Left = left;
				this.Right = right;
			}

			public ArcInterval PeriodicPrevious
			{
				get { return new ArcInterval(this, -4); }
			}

			public ArcInterval PeriodicNext
			{
				get { return new ArcInterval(this, 4); }
			}
		}
	}
}