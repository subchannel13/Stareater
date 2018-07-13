using System;
using Stareater.Utils.NumberFormatters;

namespace Stareater.GuiUtils
{
	public static class NumberInput
	{
		public static long? DecodeQuantity(string text)
		{
			long result;

			text = text.Trim();
			double? prefixedValue = ThousandsFormatter.TryParse(text);
			if (prefixedValue.HasValue)
				result = (long)prefixedValue.Value;
			else if (text.ToLower().Contains("e")) {
				double resultScientific;
				if (double.TryParse(text, out resultScientific))
					result = (long)resultScientific;
				else
					return null;
			}
			else if (!long.TryParse(text, out result))
					return null;

			return (result < 0) ? null : new long?(result);
		}
	}
}
