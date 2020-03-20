using Stareater.Galaxy;
using Stareater.SpaceCombat;
using Stareater.Utils;
using System;
using System.Collections.Generic;

namespace Stareater.Controllers.Views.Combat
{
	public class CombatPlanetInfo :IEquatable<CombatPlanetInfo>
	{
		internal readonly CombatPlanet Data;
		private readonly MainGame mainGame;
		
		internal CombatPlanetInfo(CombatPlanet data, MainGame mainGame)
		{
			this.Data = data;
			this.mainGame = mainGame;
		}

		public PlanetInfo Planet => new PlanetInfo(this.Data.PlanetData, this.mainGame);

		public ColonyInfo Colony => new ColonyInfo(this.Data.Colony, this.mainGame.Derivates[this.Data.Colony]);

		public int OrdinalPosition 
		{
			get { return Data.PlanetData.Position; }
		}
		
		public Vector2D Position 
		{
			get { return Data.Position; }
		}

		public PlanetType Type {
			get { return this.Data.PlanetData.Type; }
		}
		
		public PlayerInfo Owner 
		{
			get { return Data.Colony != null ? new PlayerInfo(Data.Colony.Owner) : null; }
		}
		
		public double Population 
		{
			get { return Data.Colony != null ? Data.Colony.Population : 0; }
		}

		public bool Equals(CombatPlanetInfo other)
		{
			if (other is null)
				return false;

			if (ReferenceEquals(this, other))
				return true;

			if (this.GetType() != other.GetType())
				return false;

			return EqualityComparer<Planet>.Default.Equals(this.Data.PlanetData, other.Data.PlanetData);
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as CombatPlanetInfo);
		}

		public override int GetHashCode()
		{
			return this.Data.PlanetData.GetHashCode();
		}

		public static bool operator ==(CombatPlanetInfo info1, CombatPlanetInfo info2)
		{
			if (info1 is null)
				return info2 is null;
			return info1.Equals(info2);
		}

		public static bool operator !=(CombatPlanetInfo info1, CombatPlanetInfo info2)
		{
			return !(info1 == info2);
		}
	}
}
