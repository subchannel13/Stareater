using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using Stareater.Controllers.Views.Ships;
using Stareater.GameLogic;
using Stareater.SpaceCombat;

namespace Stareater.Controllers.Views.Combat
{
	public class CombatantInfo
	{
		internal readonly Combatant Data;
		private readonly DesignStats stats;
		private IEnumerable<Vector2D> validMoves;
		private readonly List<AbilityInfo> abilities;
		
		internal CombatantInfo(Combatant data, MainGame game, IEnumerable<Vector2D> validMoves)
		{
			this.Data = data;
			this.stats = game.Derivates.Of(data.Owner).DesignStats[data.Ships.Design];
			this.validMoves = validMoves.ToList();
			
			this.abilities = new List<AbilityInfo>(this.stats.Abilities.Select((x, i) => new AbilityInfo(x, i, data.AbilityCharges[i])));
		}
		
		public Vector2D Position
		{ 
			get { return this.Data.Position; }
		}		
		
		public PlayerInfo Owner
		{ 
			get { return new PlayerInfo(this.Data.Owner); }
		}
		
		public DesignInfo Design
		{ 
			get { return new DesignInfo(this.Data.Ships.Design, stats); }
		}
		
		public long Count
		{
			get { return this.Data.Ships.Quantity; }
		}
		
		public IEnumerable<Vector2D> ValidMoves
		{
			get { return this.validMoves; }
		}
		
		public IEnumerable<AbilityInfo> Abilities
		{
			get 
			{
				return abilities;
			}
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			var other = obj as CombatantInfo;
			return other != null && object.Equals(this.Data, other.Data);
		}

		public override int GetHashCode()
		{
			return Data.GetHashCode();
		}

		public static bool operator ==(CombatantInfo lhs, CombatantInfo rhs) {
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(CombatantInfo lhs, CombatantInfo rhs) {
			return !(lhs == rhs);
		}
		#endregion
	}
}
