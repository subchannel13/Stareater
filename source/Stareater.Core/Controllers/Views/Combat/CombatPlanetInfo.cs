using System;
using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy;
using Stareater.SpaceCombat;

namespace Stareater.Controllers.Views.Combat
{
	public class CombatPlanetInfo
	{
		internal readonly CombatPlanet Data;
		
		internal CombatPlanetInfo(CombatPlanet data)
		{
			this.Data = data;
		}
		
		public Vector2D Position 
		{
			get { return Data.Position; }
		}
		
		public PlayerInfo Owner 
		{
			get { return Data.Colony != null ? new PlayerInfo(Data.Colony.Owner) : null; }
		}
		
		public double Population 
		{
			get { return Data.Colony != null ? Data.Colony.Population : 0; }
		}
	}
}
