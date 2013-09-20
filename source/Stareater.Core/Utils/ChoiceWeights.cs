using System;
using System.Collections.Generic;

namespace Stareater.Utils
{
	public class ChoiceWeights<T>
	{
		private Dictionary<T, double> weights = new Dictionary<T, double>();
		public double Total { get; private set; }
		
		public ChoiceWeights()
		{
			this.Total = 0;
		}
		
		public void Add(T item, double choiceWeight)
		{
			weights.Add(item, choiceWeight);
			Total += choiceWeight;
		}
		
		public double Relative(T item)
		{
			return weights[item] / Total;
		}
	}
}
