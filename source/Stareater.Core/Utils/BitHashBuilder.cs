using System;
using System.Collections.Generic;

namespace Stareater.Utils
{
	public class BitHashBuilder
	{
		private readonly List<uint> contents = new List<uint>();
		private uint current = 0;
		private uint factor = 1;
		
		public void Add(int value, int maxValue)
		{
			uint factorStep = 2;
			while (factorStep < (uint)maxValue)
				factorStep *= 2;

			if (uint.MaxValue / factorStep < factor)
			{
				factor = 1;
				contents.Add(current);
				current = 0;
			}

			current += (uint)value * factor;
			factor *= factorStep;
		}
		
		public BitHash Create()
		{
			if (factor > 1)
				contents.Add(current);
			
			return new BitHash(contents.ToArray());
		}
	}
}
