using System;

namespace Stareater.Utils.NumberFormatters
{
	public static class RomanFromatter
	{
		public static string Fromat(int number)
		{
			if ((number < 0) || (number > 3999)) return number.ToStringInvariant();

	        if (number >= 1000) return new string('M', number / 1000) + Fromat(number % 1000);
	
	        if (number >= 900) return "CM" + Fromat(number - 900);
	        if (number >= 500) return "D" + Fromat(number - 500);
	        if (number >= 400) return "CD" + Fromat(number - 400);
	        if (number >= 100) return new string('C', number / 100) + Fromat(number % 100);
	        
	        if (number >= 90) return "XC" + Fromat(number - 90);
	        if (number >= 50) return "L" + Fromat(number - 50);
	        if (number >= 40) return "XL" + Fromat(number - 40);
	        if (number >= 10) return new string('X', number / 10) + Fromat(number % 10);
	        
	        if (number >= 9) return "IX" + Fromat(number - 9);
	        if (number >= 5) return "V" + Fromat(number - 5);
	        if (number >= 4) return "IV" + Fromat(number - 4);
			
	        return number >= 1 ? new string('I', number) : string.Empty;
		}
	}
}
