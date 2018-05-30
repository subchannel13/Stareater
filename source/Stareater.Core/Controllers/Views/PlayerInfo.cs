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
			get { return this.Data.Name; }
		}
		
		public Color Color 
		{ 
			get { return this.Data.Color; }
		}

		public OrganizationInfo Organization
		{
			get { return new OrganizationInfo(this.Data.Organization); }
		}

		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			var other = obj as PlayerInfo;
			return other != null && object.Equals(this.Data, other.Data);
		}
		
		public override int GetHashCode()
		{
			return this.Data.GetHashCode();
		}
		
		public static bool operator ==(PlayerInfo lhs, PlayerInfo rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (lhs is null || rhs is null)
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
