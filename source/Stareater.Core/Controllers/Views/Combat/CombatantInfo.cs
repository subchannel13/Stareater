using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Views.Ships;
using Stareater.GameData.Databases;
using Stareater.SpaceCombat;
using Stareater.GameLogic.Combat;
using Stareater.Utils;

namespace Stareater.Controllers.Views.Combat
{
	public class CombatantInfo
	{
		internal readonly Combatant Data;
		private readonly DesignStats stats;
		private readonly List<AbilityInfo> abilities;
		
		internal CombatantInfo(Combatant data, MainGame game, IEnumerable<Vector2D> validMoves)
		{
			this.Data = data;
			this.stats = game.Derivates[data.Owner].DesignStats[data.Ships.Design];
			this.ValidMoves = validMoves.ToList();
			
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
			get { return new DesignInfo(this.Data.Ships.Design, this.stats); }
		}
		
		public long Count
		{
			get { return this.Data.Ships.Quantity; }
		}

		public IEnumerable<Vector2D> ValidMoves { get; private set; }

		public IEnumerable<AbilityInfo> Abilities
		{
			get 
			{
				return abilities;
			}
		}
		
		public bool CloakedFor(PlayerInfo player)
		{
			if (player == null)
				throw new ArgumentNullException(nameof(player));

			return this.Data.CloakedFor.Contains(player.Data);
		}
		
		public int MovementEta
		{
			get 
			{ 
				if (this.Data.MovementPoints > 0)
					return 0;
				
				var rounded = Math.Ceiling(Math.Abs(this.Data.MovementPoints));
				return Math.Abs(this.Data.MovementPoints) != rounded ? (int)rounded : (int)(rounded + 1);
			}
		}
		
		public double MovementPoints
		{
			get 
			{ 
				return this.Data.MovementPoints;
			}
		}
		
		public double ArmorHp
		{
			get 
			{ 
				return this.Data.TopArmor;
			}
		}
		
		public double ArmorHpMax
		{
			get 
			{ 
				return this.stats.HitPoints;
			}
		}

		public double ShieldHp
		{
			get 
			{ 
				return this.Data.TopShields;
			}
		}
		
		public double ShieldHpMax
		{
			get 
			{ 
				return this.stats.ShieldPoints;
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
			if (lhs is null || rhs is null)
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(CombatantInfo lhs, CombatantInfo rhs) {
			return !(lhs == rhs);
		}
		#endregion
	}
}
