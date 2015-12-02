using System;

namespace Stareater.GUI.ShipDesigns
{
	public interface IShipComponentChoice
	{
		string Name { get; }
		string ImagePath { get; }
		
		void Select();
	}
}
