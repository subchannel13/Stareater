using System;
using Stareater.Players;

namespace Stareater.SpaceCombat
{
	class Combatant
	{
		public int X, Y;
		public Player Owner { get; private set; }
		
		public Combatant(int x, int y, Player owner)
		{
			this.X = x;
			this.Y = y;
			this.Owner = owner;
		}
	}
}
