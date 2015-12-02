using System;

namespace Stareater.GUI.ShipDesigns
{
	public class ShipComponentChoice<T> : IShipComponentChoice
	{
		public string Name { get; private set; }
		public string ImagePath { get; private set; }
		
		private readonly T item;
		private readonly Action<T> selectAction;
		
		public ShipComponentChoice(string name, string imagePath, T item, Action<T> selectAction)
		{
			this.item = item;
			this.selectAction = selectAction;
			this.Name = name;
			this.ImagePath = imagePath;
		}

		public void Select()
		{
			this.selectAction(item);
		}
	}
}
