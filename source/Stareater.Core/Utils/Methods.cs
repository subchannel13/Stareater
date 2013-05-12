using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Stareater.Utils
{
	/// <summary>
	/// Set of various helper methods.
	/// </summary>
	public static class Methods
	{
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
	}
}
