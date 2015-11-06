using System;
using System.Collections.Generic;

namespace Stareater.Utils
{
	public class BitHashBuilder
	{
		private readonly List<uint> contents = new List<uint>();
		private readonly List<uint> maxValues = new List<uint>();
		private uint current = 0;
		private uint factor = 1;
		
		public void Add(int value, int maxValue)
		{
			if (uint.MaxValue / (uint)maxValue < factor) {
				factor = 1;
				contents.Add(current);
				current = 0;
			}

			current += (uint)value * factor;

			uint factorStep = 2;
			while (factorStep < (uint)maxValue)
				factorStep *= 2;
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
