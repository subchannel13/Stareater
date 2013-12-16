using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NGenerics.DataStructures.Mathematical;

namespace Stareater.Utils
{
	/// <summary>
	/// Set of various helper methods.
	/// </summary>
	public static class Methods
	{
		/// <summary>
		/// Limits a value to a range.
		/// </summary>
		/// <param name="x">A value to clamp.</param>
		/// <param name="min">Range lower bound (inclusive)</param>
		/// <param name="max">Range upper bound (inclusive)</param>
		/// <returns></returns>
		public static double Clamp(double x, double min, double max)
		{
			if (x < min)
				return min;

			if (x > max)
				return max;

			return x;
		}
		
		/// <summary>
		/// Limits a value to a range.
		/// </summary>
		/// <param name="x">A value to clamp.</param>
		/// <param name="min">Range lower bound (inclusive)</param>
		/// <param name="max">Range upper bound (inclusive)</param>
		/// <returns></returns>
		public static int Clamp(int x, int min, int max)
		{
			if (x < min)
				return min;

			if (x > max)
				return max;

			return x;
		}

		/// <summary>
		/// Linear interpolation between two numbers.
		/// </summary>
		/// <param name="x">Interpolation weight, 0 = left, 1 = right</param>
		/// <param name="left">Left value</param>
		/// <param name="right">Right value</param>
		/// <returns></returns>
		public static double Lerp(double x, double left, double right)
		{
			return x * (right - left) + left;
		}
		
		/// <summary>
		/// Test whether a line segment intersects other line segments.
		/// </summary>
		/// <param name="line">Line to test</param>
		/// <param name="otherLines">Set of lines to test against</param>
		/// <param name="Epsilon">Tolerance when testing for parallelism</param>
		/// <returns></returns>
		public static bool LineIntersects(Tuple<Vector2D, Vector2D> line, IEnumerable<Tuple<Vector2D, Vector2D>> otherLines, double Epsilon)
		{
			Vector2D x0 = line.Item1;
			Vector2D v0 = line.Item2 - x0;
			Vector2D n0 = new Vector2D(-v0.Y, v0.X);
			double v0magSquare = v0.X * v0.X + v0.Y * v0.Y;

			foreach (var usedEdge in otherLines) {
				Vector2D x1 = usedEdge.Item1;
				Vector2D v1 = usedEdge.Item2 - x1;

				if (Math.Abs(v0.CrossProduct(v1).Z) < Epsilon)
					if ((x0 - x1).Magnitude() < Epsilon)
						return true;
					else
						continue;

				double t1 = n0.DotProduct(x0 - x1) / n0.DotProduct(v1);
				double t0 = v0.DotProduct(x1 + v1 * t1 - x0) / v0magSquare;

				if (t0 > 0 && t0 < 1 && t1 > 0 && t1 < 1)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Creates instances of all classes assignable to T, in a given DLL file.
		/// </summary>
		/// <typeparam name="T">Base type of desired classes.</typeparam>
		/// <param name="fileName">Path to DLL file.</param>
		/// <returns>Instances</returns>
		public static IEnumerable<T> LoadFromDLL<T>(string fileName)
		{
			Type targetType = typeof(T);
			foreach (var type in Assembly.LoadFile(fileName).GetTypes())
				if (targetType.IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
					yield return (T)Activator.CreateInstance(type);
		}

		/// <summary>
		/// Generates sequence of precentages (real number from [0, 1]) for reporting progress.
		/// </summary>
		/// <param name="progressOffset">Starting percentage</param>
		/// <param name="progressScale">Size of returned range (difference between lowest and highest returned number)</param>
		/// <param name="inputSize">Cardinality of returned sequence</param>
		/// <returns>Sequence of precentages</returns>
		public static IEnumerable<double> ProgressReportHelper(double progressOffset, double progressScale,
			int inputSize)
		{
			for (int i = 1; i <= inputSize; i++)
				yield return progressOffset + (progressScale * i) / inputSize;
		}

		/// <summary>
		/// Generates sequence of precentages (real number from [0, 1]) for reporting progress and data that has to be processed.
		/// </summary>
		/// <typeparam name="T">Type of data for processing.</typeparam>
		/// <param name="progressOffset">Starting percentage</param>
		/// <param name="progressScale">Size of returned range (difference between lowest and highest returned number)</param>
		/// <param name="inputData">Collection where each element has to be processed</param>
		/// <returns>Sequence of precentages and data</returns>
		public static IEnumerable<ProgressData<T>> ProgressReportHelper<T>(double progressOffset, double progressScale,
			ICollection<T> inputData)
		{
			int inputSize = inputData.Count;
			int i = 1;
			foreach (T item in inputData) {
				yield return new ProgressData<T>(progressOffset + (progressScale * i) / inputSize, item);
				i++;
			}
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

			if (x == last && skipLast)
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
	}
}
