using System;
using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy;
using Stareater.SpaceCombat;

namespace Stareater.Controllers.Views.Combat
{
	public class CombatPlanetInfo
	{
		private readonly CombatPlanet data;
		
		internal CombatPlanetInfo(CombatPlanet data)
		{
			this.data = data;
		}
		
		public Vector2D Position 
		{
			get { return data.Position; }
		}
		
		public PlayerInfo Owner 
		{
			get { return data.Colony != null ? new PlayerInfo(data.Colony.Owner) : null; }
		}
	}
}
