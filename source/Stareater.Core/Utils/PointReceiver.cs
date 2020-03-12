using System;

namespace Stareater.Utils
{
	public class PointReceiver
	{
		public double Weight { get; private set; }
		public double Limit { get; private set; }
		public Action<double> ReceiveAction { get; private set; }

		public PointReceiver(double weight, double limit, Action<double> receiveAction)
		{
			this.Weight = weight;
			this.Limit = limit;
			this.ReceiveAction = receiveAction;
		}
	}
}
