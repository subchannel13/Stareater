using System;
using System.Globalization;
using System.Text;

namespace Stareater.Utils.NumberFormatters
{
	public class DecimalsFormatter
	{
		private readonly string formatString;

		public DecimalsFormatter(int obligatoryDecimals, int optionalDecimals)
		{
			var formatBuilder = new StringBuilder("0.");
			for (int i = 0; i < obligatoryDecimals; i++)
				formatBuilder.Append('0');
			for (int i = 0; i < optionalDecimals; i++)
				formatBuilder.Append('#');

			this.formatString = formatBuilder.ToString();
		}

		public string Format(double number)
		{
			return number.ToString(formatString, NumberFormatInfo.InvariantInfo);
		}
		
		public string Format(double number, RoundingMethod method, int roundingPrecision)
		{
			double precisionFactor = Math.Pow(10, roundingPrecision);
			
			switch(method)
			{
				case RoundingMethod.Ceil:
					number = Math.Ceiling(number * precisionFactor) / precisionFactor;
					break;
				case RoundingMethod.Floor:
					number = Math.Floor(number * precisionFactor) / precisionFactor;
					break;
				case RoundingMethod.Midpoint:
					number = Math.Round(number, roundingPrecision, MidpointRounding.AwayFromZero);
					break;
			}
			return Format(number);
		}
		
		public static string Sign(double number)
		{
			return number > 0 ? "+" : number < 0 ? "-" : "";
		}
	}
}
