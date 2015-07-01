using System;
using System.Drawing;
using Stareater.Players;

namespace Stareater.Controllers.Views
{
	public class PlayerInfo
	{
		internal Player Data { get; private set; }
		
		internal PlayerInfo(Player player)
		{
			this.Data = player;
		}
		
		public string Name 
		{ 
			get { return Data.Name; }
		}
		
		public Color Color 
		{ 
			get { return Data.Color; }
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			var other = obj as PlayerInfo;
			return other != null && object.Equals(this.Data, other.Data);
		}
		
		public override int GetHashCode()
		{
			return Data.GetHashCode();
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
