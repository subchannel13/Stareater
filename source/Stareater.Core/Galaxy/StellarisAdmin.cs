using Stareater.GameData;
using Stareater.Players;
using Stareater.Utils.StateEngine;

namespace Stareater.Galaxy
{
	[StateType(saveTag: Tag)]
	class StellarisAdmin : AConstructionSite 
	{
		public StellarisAdmin(StarData star, Player owner) : base(new LocationBody(star), owner) 
		{ } 

		private StellarisAdmin() 
		{ }

		public override SiteType Type
		{
			get { return SiteType.StarSystem; }
		}

		public const string Tag = "StellarisAdmin"; 
	}
}
