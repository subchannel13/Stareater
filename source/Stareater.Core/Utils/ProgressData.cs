using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Utils
{
	public struct ProgressData<T>
	{
		public double Percentage;
		public T Data;

		public ProgressData(double percentage, T data)
		{
			this.Data = data;
			this.Percentage = percentage;
		}
	}
}
