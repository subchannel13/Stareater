using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Utils.NumberFormatters
{
	class DecimalsFormatter
	{
		private string formatString;

		public DecimalsFormatter(int obligatoryDecimals, int optionalDecimals)
		{
			StringBuilder formatBuilder = new StringBuilder("0.");
			for (int i = 0; i < obligatoryDecimals; i++)
				formatBuilder.Append('0');
			for (int i = 0; i < optionalDecimals; i++)
				formatBuilder.Append('#');

			this.formatString = formatBuilder.ToString();
		}

		public string Format(double number)
		{
			return number.ToString(formatString);
		}
	}
}
