using System;

namespace Stareater.GUI.ShipDesigns
{
	public interface IShipComponentType
	{
		string Name { get; }
		string ImagePath { get; }
		
		void Select();
	}
}
