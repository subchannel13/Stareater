using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.StringTools
{
	class ThousandsFormatter
	{
		private static string[] Prefixes = new string[] { "", " k", " M", " G", " T", " P", " E", " Z", " Y" };

		private Tuple<int, double> prefixInfo = null;

		public ThousandsFormatter()
		{ }

		public ThousandsFormatter(params double[] numbersInContext)
		{
			this.prefixInfo = new Tuple<int, double>(int.MaxValue, 0);
			
			foreach (double number in numbersInContext) {
				var candidateInfo = greatestLowerPrefix(number);
				if (candidateInfo.Item1 < prefixInfo.Item1)
					this.prefixInfo = candidateInfo;
			}
		}

		public string Format(double number)
		{
			var prefixInfo = this.prefixInfo ?? greatestLowerPrefix(number);

			return (number / prefixInfo.Item2).ToString("0.##") + Prefixes[prefixInfo.Item1];
		}

		private static Tuple<int, double> greatestLowerPrefix(double number)
		{
			int prefixIndex = 0;
			double weight = 1;
			for (; prefixIndex < Prefixes.Length && number >= weight * 1000; prefixIndex++)
				weight *= 1000;

			return new Tuple<int, double>(prefixIndex, weight);
		}
	}
}
