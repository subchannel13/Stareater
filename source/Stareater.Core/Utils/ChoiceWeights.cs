using System.Collections.Generic;

namespace Stareater.Utils
{
	public class ChoiceWeights<T>
	{
		private readonly Dictionary<T, double> weights = new Dictionary<T, double>();
		public double Total { get; private set; } //TODO(v0.8) check if it needs to be public
		
		public ChoiceWeights()
		{
			this.Total = 0;
		}
		
		public void Add(T item, double choiceWeight)
		{
			this.weights.Add(item, choiceWeight);
			this.Total += choiceWeight;
		}
		
		public double Relative(T item)
		{
			return this.weights[item] / this.Total;
		}
	}
}
