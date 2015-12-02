using System;

namespace Stareater.GUI
{
	public class ShipComponent<T> : IShipComponent
	{
		#region IShipComponentChoice implementation
		public string Name { get; private set; }
		public string ImagePath { get; private set; }
		#endregion
		
		public T Item { get; private set; }
		
		public ShipComponent(string name, string imagePath, T item)
		{
			this.Name = name;
			this.ImagePath = imagePath;
			this.Item = item;
		}
	}
}
