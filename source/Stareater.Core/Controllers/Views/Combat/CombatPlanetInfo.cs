using System;
using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy;

namespace Stareater.Controllers.Views.Combat
{
	public class CombatPlanetInfo
	{
		private readonly Vector2D position;
		private readonly PlayerInfo owner;
		
		internal CombatPlanetInfo(Vector2D position, Colony colony)
		{
			this.position = position;
			this.owner = colony != null ? new PlayerInfo(colony.Owner) : null;
		}
		
		public Vector2D Position 
		{
			get { return position; }
		}
		
		public PlayerInfo Owner 
		{
			get { return owner; }
		}
	}
}
