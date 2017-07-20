using System;
using System.Drawing;

namespace Stareater.GUI.ShipDesigns
{
	public class ShipComponentType<T> : IShipComponentType
	{
		public string Name { get; private set; }
		public Image Image { get; private set; }
		public double AmountLimit { get; private set; }

		private readonly T item;
		private readonly Action<T> dispatchAction;
		
		public ShipComponentType(string name, Image image, double amountLimit, T item, Action<T> selectAction)
		{
			this.item = item;
			this.dispatchAction = selectAction;
			this.Name = name;
			this.Image = image;
			this.AmountLimit = amountLimit;
		}

		public void Dispatch()
		{
			this.dispatchAction(item);
		}
	}
}
