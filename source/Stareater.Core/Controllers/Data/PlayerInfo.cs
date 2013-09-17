using System;
using System.Drawing;
using Stareater.Players;

namespace Stareater.Controllers.Data
{
	public class PlayerInfo
	{
		private Player player;
		
		internal PlayerInfo(Player player)
		{
			this.player = player;
		}
		
		public string Name 
		{ 
			get { return player.Name; }
		}
		
		public Color Color 
		{ 
			get { return player.Color; }
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			PlayerInfo other = obj as PlayerInfo;
			if (other == null)
				return false;
			return object.Equals(this.player, other.player);
		}
		
		public override int GetHashCode()
		{
			return player.GetHashCode();
		}
		
		public static bool operator ==(PlayerInfo lhs, PlayerInfo rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(PlayerInfo lhs, PlayerInfo rhs)
		{
			return !(lhs == rhs);
		}
		#endregion

	}
}
