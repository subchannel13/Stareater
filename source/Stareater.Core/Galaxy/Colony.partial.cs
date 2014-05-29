using System;
using System.Linq;
using Stareater.GameData;
using Stareater.Players;

namespace Stareater.Galaxy
{
	partial class Colony : AConstructionSite
	{
		public StarData Star
		{
			get {
				return Location.Star;
			}
		}
		
		public override SiteType Type
		{
			get { return SiteType.Colony; }
		}
	}
}
