using System;

namespace Stareater.Utils
{
	public class PointReceiver<T>
	{
		public T Item { get; private set; }
		public double Weight { get; private set; }
		public Func<double> Limit { get; private set; }
		public Action<double> ReceiveAction { get; private set; }

		public PointReceiver(T item, double weight, Func<T, double> limit, Action<T, double> receiveAction)
		{
			this.Item = item;
			this.Weight = weight;
			this.Limit = () => limit(item);
			this.ReceiveAction = x => receiveAction(item, x);
		}
	}
}
