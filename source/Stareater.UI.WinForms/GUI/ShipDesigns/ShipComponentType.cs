using System;
using System.Drawing;

namespace Stareater.GUI.ShipDesigns
{
	public class ShipComponentType<T> : IShipComponentType
	{
		public string Name { get; private set; }
		public Image Image { get; private set; }
		
		private readonly T item;
		private readonly Action<T> dispatchAction;
		
		public ShipComponentType(string name, Image image, T item, Action<T> selectAction)
		{
			this.item = item;
			this.dispatchAction = selectAction;
			this.Name = name;
			this.Image = image;
		}

		public void Dispatch()
		{
			this.dispatchAction(item);
		}
	}
}
