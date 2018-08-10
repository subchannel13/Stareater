using System.Linq;
using Stareater.Galaxy;

namespace Stareater.Controllers.Views
{
	//TODO(later) convert to general star info even if not colonized
	public class StellarisInfo
	{
		internal StellarisAdmin Stellaris { get; private set; }
		
		public PlayerInfo Owner { get; private set; }
		public double Population { get; private set; }

		internal StellarisInfo(StellarisAdmin stellaris, MainGame game)
		{
			this.Stellaris = stellaris;
			this.Owner = new PlayerInfo(stellaris.Owner);
			
			this.Population = game.States.Colonies.
				AtStar[stellaris.Location.Star].
				Where(x => x.Owner == stellaris.Owner).
				Sum(x => x.Population);
		}
		
		public StarInfo HostStar
		{
			get { return new StarInfo(this.Stellaris.Location.Star); }
		}

		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			var other = obj as StellarisInfo;
			return other != null && object.Equals(this.Stellaris, other.Stellaris);
		}

		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				if (Stellaris != null)
					hashCode += 1000000007 * Stellaris.GetHashCode();
			}
			return hashCode;
		}

		public static bool operator ==(StellarisInfo lhs, StellarisInfo rhs) {
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (lhs is null || rhs is null)
				return false;
			return lhs.Equals(rhs);
		}

		public static bool operator !=(StellarisInfo lhs, StellarisInfo rhs) {
			return !(lhs == rhs);
		}
		#endregion
	}
}
