using System;
using System.Drawing;

namespace Stareater.GUI.ShipDesigns
{
	public interface IShipComponentType
	{
		string Name { get; }
		Image Image { get; }
		
		void Dispatch();
	}
}
