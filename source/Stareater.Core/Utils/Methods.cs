using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Utils
{
	public static class Methods
	{
		public static IEnumerable<double> ProgressReportHelper(double progressOffset, double progressScale,
			int inputSize, Action inputProcessor)
		{
			for (int i = 1; i <= inputSize; i++) {
				inputProcessor();
				yield return progressOffset + (progressScale * i) / inputSize;
			}
		}

		public static IEnumerable<double> ProgressReportHelper<T>(double progressOffset, double progressScale,
			ICollection<T> inputData, Action<T> inputProcessor)
		{
			int inputSize = inputData.Count;
			int i = 1;
			foreach (T item in inputData) {
				inputProcessor(item);
				yield return progressOffset + (progressScale * i) / inputSize;
				i++;
			}
		}

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
