using System;
using System.Collections.Generic;
using System.Linq;

using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;

namespace Stareater.Utils
{
	/// <summary>
	/// Set of various helper methods.
	/// </summary>
	public static class Methods
	{
		/// <summary>
		/// A* search algorithm from one node to another.
		/// </summary>
		/// <typeparam name="T">Node type</typeparam>
		/// <param name="fromNode">Starting node</param>
		/// <param name="toNode">Goal node</param>
		/// <param name="heuristicFunc">Heuristic cost function for a node</param>
		/// <param name="costFunc">Real cost function for moving from one node to neighbouring node</param>
		/// <param name="neighboursFunc">Node neighbours</param>
		/// <returns>Nodes in the path</returns>
		public static IEnumerable<Move<T>> AStar<T>(T fromNode, T toNode, Func<T, double> heuristicFunc, Func<T, T, double> costFunc, Func<T, IEnumerable<T>> neighboursFunc)
		{
			if (fromNode.Equals(toNode))
				return new Move<T>[0];

			var cameFrom = new Dictionary<T, T>();
			var closedSet = new HashSet<T>();
			var openSet = new PriorityQueue<T>();
			var nodeScore = new Dictionary<T, double> { [fromNode] = 0 };
			openSet.Enqueue(fromNode, heuristicFunc(fromNode));
			T current;

			while (openSet.Count > 0)
			{
				current = openSet.Dequeue();
				if (current.Equals(toNode))
					break;

				closedSet.Add(current);
				foreach (var neighbor in neighboursFunc(current))
				{
					if (closedSet.Contains(neighbor))
						continue;

					var tentativeScore = nodeScore[current] + costFunc(current, neighbor);
					if (!openSet.Contains(neighbor))
						openSet.Enqueue(neighbor, tentativeScore + heuristicFunc(neighbor));
					else if (nodeScore.ContainsKey(neighbor) && tentativeScore >= nodeScore[neighbor])
						continue;

					cameFrom[neighbor] = current;
					nodeScore[neighbor] = tentativeScore;
				}
			}

			//Destination is unreachable
			if (!cameFrom.ContainsKey(toNode))
				return null;

			var path = new List<Move<T>>();
			current = toNode;
			while (!current.Equals(fromNode))
			{
				var previous = cameFrom[current];
				path.Add(new Move<T>(previous, current));
				current = previous;
			}

			path.Reverse();
			return path;
		}

		/// <summary>
		/// Limits a value to a range.
		/// </summary>
		/// <param name="x">A value to clamp.</param>
		/// <param name="min">Range lower bound (inclusive)</param>
		/// <param name="max">Range upper bound (inclusive)</param>
		/// <returns></returns>
		public static T Clamp<T>(T x, T min, T max)
		{
			if (Comparer<T>.Default.Compare(x, min) < 0)
				return min;

			return (Comparer<T>.Default.Compare(x, max) > 0) ? max : x;

		}
		
		/// <summary>
		/// Finds an element with highest fitness function value.
		/// </summary>
		/// <param name="source">Input collection of elements</param>
		/// <param name="fitnessFunc">Fitness function</param>
		/// <returns>Element with highest fitenss or null if collection is empty</returns>
		public static T FindBestOrDefault<T>(IEnumerable<T> source, Func<T, IComparable> fitnessFunc) where T : class
		{
			T best = null;
			IComparable bestValue = null;
				
			foreach (var item in source)
			{
				var itemValue = fitnessFunc(item);
				if (best == null || itemValue.CompareTo(bestValue) > 0)
				{
					best = item;
					bestValue = itemValue;
				}
			}

			return best;
		}

		/// <summary>
		/// Finds an element with highest fitness function value.
		/// </summary>
		/// <param name="source">Input collection of elements</param>
		/// <param name="fitnessFunc">Fitness function</param>
		/// <returns>Element with highest fitenss</returns>
		public static T FindBest<T>(IEnumerable<T> source, Func<T, IComparable> fitnessFunc)
		{
			T best = source.First();
			IComparable bestValue = fitnessFunc(best);

			foreach (var item in source.Skip(1))
			{
				var itemValue = fitnessFunc(item);
				if (itemValue.CompareTo(bestValue) > 0)
				{
					best = item;
					bestValue = itemValue;
				}
			}

			return best;
		}

		/// <summary>
		/// Finds an element with lowest fitness function value.
		/// </summary>
		/// <param name="source">Input collection of elements</param>
		/// <param name="fitnessFunc">Fitness function</param>
		/// <returns>Element with lowest fitenss</returns>
		public static T FindWorst<T>(IEnumerable<T> source, Func<T, IComparable> fitnessFunc)
		{
			T worst = source.First();
			IComparable worstValue = fitnessFunc(worst);

			foreach (var item in source.Skip(1))
			{
				var itemValue = fitnessFunc(item);
				if (itemValue.CompareTo(worstValue) < 0)
				{
					worst = item;
					worstValue = itemValue;
				}
			}

			return worst;
		}

		/// <summary>
		/// Returns the phase (fraction of completion) of a period.
		/// </summary>
		/// <param name="x">Input value such ase total elapsed time</param>
		/// <param name="period">Period length</param>
		/// <returns>Phase value between 0 (period just started, inclusive) and 1 (period finished, exclusive)</returns>
		public static double GetPhase(double x, double period)
		{
			return x / period - Math.Floor(x / period);
		}
		
		/// <summary>
		/// Calculate number of steps needed to move from origin (0, 0) to a certain position on a hexagonal grid.
		/// </summary>
		/// <param name="position">Target position</param>
		/// <returns>Number of steps</returns>
		public static double HexDistance(Vector2D position)
		{
			return HexDistance(position, new Vector2D());
		}
		
		/// <summary>
		/// Calculate number of steps needed to move from one tile to another on a hexagonal grid.
		/// </summary>
		/// <param name="positionA">Start tile position</param>
		/// <param name="positionB">Destination tile position</param>
		/// <returns>Distance in tiles</returns>
		public static double HexDistance(Vector2D positionA, Vector2D positionB)
		{
			var ay = positionA.Y - Math.Floor(positionA.X / 2);
			var by = positionB.Y - Math.Floor(positionB.X / 2);
			var az = -positionA.X - ay;
			var bz = -positionB.X - by;
			
			return Math.Max(Math.Abs(positionA.X - positionB.X), Math.Max(Math.Abs(ay - by), Math.Abs(az - bz)));
		}
		
		/// <summary>
		/// Returns coordinates of immediate neighbour of a tile on hexagonal grid. 
		/// </summary>
		/// <param name="position">Position of a tile</param>
		/// <returns>Positions of neighbours</returns>
		public static IEnumerable<Vector2D> HexNeighbours(Vector2D position)
		{
			var yOffset = (int)Math.Abs(position.X) % 2 == 0 ? 0 : 1;
				
			yield return position + new Vector2D(0, 1);
			yield return position + new Vector2D(1, 0 + yOffset);
			yield return position + new Vector2D(1, -1 + yOffset);
			yield return position + new Vector2D(0, -1);
			yield return position + new Vector2D(-1, -1 + yOffset);
			yield return position + new Vector2D(-1, 0 + yOffset);
		}
		
		/// <summary>
		/// Tests whether one rectangle ("outer") is completely contains other rectrangle ("inner").
		/// </summary>
		/// <param name="outerTopRight">Top right corner of "outer" rectangle</param>
		/// <param name="outerBottomLeft">Bottm left corner of "outer" rectangle</param>
		/// <param name="innerTopRight">Top right corner of "inner" rectangle</param>
		/// <param name="innerBottomLeft">Bottm left corner of "inner" rectangle</param>
		/// <returns></returns>
		public static bool IsRectEnveloped(Vector2D outerTopRight, Vector2D outerBottomLeft, Vector2D innerTopRight, Vector2D innerBottomLeft)
		{
			return outerTopRight.X >= innerTopRight.X && outerTopRight.Y >= innerTopRight.Y &&
				outerBottomLeft.X <= innerBottomLeft.X && outerBottomLeft.Y <= innerBottomLeft.Y;
		}
		
		/// <summary>
		/// Tests whether one rectangle ("B") is completely outside other rectangle ("A").
		/// </summary>
		/// <param name="aTopRight">Top right corner of rectangle "A"</param>
		/// <param name="aBottomLeft">Bottm left corner of rectangle "A"</param>
		/// <param name="bTopRight">Top right corner of rectangle "B"</param>
		/// <param name="bBottomLeft">Bottm left corner of rectangle "B"</param>
		/// <returns></returns>
		public static bool IsRectOutside(Vector2D aTopRight, Vector2D aBottomLeft, Vector2D bTopRight, Vector2D bBottomLeft)
		{
			return aTopRight.X < bBottomLeft.X || aTopRight.Y < bBottomLeft.Y ||
				aBottomLeft.X > bTopRight.X || aBottomLeft.Y > bTopRight.Y;
		}

		/// <summary>
		/// Linear interpolation between two numbers.
		/// </summary>
		/// <param name="x">Interpolation weight, 0 = left, 1 = right</param>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
		/// <returns>Interpolated value</returns>
		public static double Lerp(double x, double left, double right)
		{
			return x * (right - left) + left;
		}

		/// <summary>
		/// Mean square of a sequence representing errors (difference from desired value).
		/// </summary>
		/// <param name="errors">Sequence of error values</param>
		/// <returns>Mean square error</returns>
		public static double MeanSquareError(params double[] errors)
		{
			return errors.Average(x => x * x);
		}

		/// <summary>
		/// Generates an arithmetic sequence.
		/// </summary>
		/// <param name="first">First element of the sequence</param>
		/// <param name="last">Last element of the sequence</param>
		/// <param name="step">Step, difference between the numbers</param>
		/// <param name="skipLast">Whether the last element should be included in the sequence or not</param>
		/// <returns>The sequence</returns>
		public static IEnumerable<int> Range(int first, int last, int step, bool skipLast = true)
		{
			int x = first;
			for (; x < last; x += step)
				yield return x;

			if (x == last && !skipLast)
				yield return x;
		}

		/// <summary>
		/// Yields indices of elements that satisfy condition.
		/// </summary>
		/// <typeparam name="T">Type of elements in the source</typeparam>
		/// <param name="source">Ordered source of elements</param>
		/// <param name="condition">Condition to satisfy</param>
		/// <returns>Sequence of indices</returns>
		public static IEnumerable<int> SelectIndices<T>(IEnumerable<T> source, Predicate<T> condition)
		{
			int i = 0;
			foreach (T item in source) {
				if (condition(item))
					yield return i;
				i++;
			}
		}
		
		/// <summary>
		/// Retrives and converts the value from the IkonComposite or returns default value if the key is not present.
		/// </summary>
		/// <param name="composite">IKON associative table</param>
		/// <param name="key">Key for the value to retrieve</param>
		/// <param name="defaultValue">Default value in case the key is not present it the table</param>
		/// <returns>The requested value or default value.</returns>
		public static T ToOrDefault<T>(this IkonComposite composite, string key, T defaultValue)
		{
			return composite.Keys.Contains(key) ? 
				composite[key].To<T>() :
				defaultValue;
		}
		
		/// <summary>
		/// Retrives and converts the value from the IkonComposite or returns default value if the key is not present.
		/// </summary>
		/// <param name="composite">IKON associative table</param>
		/// <param name="key">Key for the value to retrieve</param>
		/// <param name="valueTransform">Value transformation function</param>
		/// <param name="defaultValue">Default value in case the key is not present it the table</param>
		/// <returns>The requested value or default value.</returns>
		public static T ToOrDefault<T>(this IkonComposite composite, string key, Func<Ikadn.IkadnBaseObject, T> valueTransform, T defaultValue)
		{
			return composite.Keys.Contains(key) ? 
				valueTransform(composite[key]) :
				defaultValue;
		}

		public static double WeightedPointDealing<T>(double points, IEnumerable<PointReceiver<T>> pointReceiver)
		{
			if (!pointReceiver.Any())
				return points;

			var weightSum = pointReceiver.Sum(x => x.Weight);
			var startingPoints = points;
			foreach(var receiver in pointReceiver)
			{
				var investment = Math.Min(startingPoints * receiver.Weight / weightSum, receiver.Limit());
				receiver.ReceiveAction(investment);
				points -= investment;
			}

			return points;
		}
	}
}
