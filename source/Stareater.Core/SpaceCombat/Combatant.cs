using System;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.SpaceCombat
{
	class Combatant
	{
		public Player Owner { get; private set; }
		public ShipGroup Ships { get; private set; }
		
		public int X, Y;
		public double Initiative;
		
		public Combatant(int x, int y, Player owner, ShipGroup ships)
		{
			this.X = x;
			this.Y = y;
			this.Owner = owner;
			this.Ships = ships;
		}
	}
}
