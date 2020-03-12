using Stareater.GameData;
using Stareater.Players;
using Stareater.Utils.StateEngine;
using System.Collections.Generic;

namespace Stareater.Galaxy
{
	[StateTypeAttribute(saveTag: Tag)]
	class StellarisAdmin : AConstructionSite 
	{
		[StatePropertyAttribute]
		public Dictionary<int, double> IncomingMigrants { get; private set; }

		public StellarisAdmin(StarData star, Player owner) : base(new LocationBody(star), owner) 
		{
			this.IncomingMigrants = new Dictionary<int, double>();
		}

		private StellarisAdmin() 
		{ }

		public override SiteType Type
		{
			get { return SiteType.StarSystem; }
		}

		public const string Tag = "StellarisAdmin"; 
	}
}
