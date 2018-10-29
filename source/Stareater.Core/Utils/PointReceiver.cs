using System;

namespace Stareater.Utils
{
	public class PointReceiver<T>
	{
		public T Item { get; private set; }
		public double Weight { get; private set; }
		public Func<double> Limit { get; private set; }
		public Action<double> ReceiveAction { get; private set; }

		public PointReceiver(T item, double weight, Func<double> limit, Action<double> receiveAction)
		{
			this.Item = item;
			this.Weight = weight;
			this.Limit = limit;
			this.ReceiveAction = receiveAction;
		}
	}
}
