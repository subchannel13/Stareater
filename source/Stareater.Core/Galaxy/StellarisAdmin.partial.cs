using System;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.GameData;
using Stareater.Players;
using Stareater.Utils.Collections;

namespace Stareater.Galaxy
{
	partial class StellarisAdmin : AConstructionSite
	{
		public override SiteType Type
		{
			get { return SiteType.StarSystem; }
		}
		
		protected override IkadnBaseObject saveLocation(ObjectIndexer indexer)
		{
			return new IkonComposite(LocationTag).Add(IdKey, new IkonInteger(indexer.IndexOf(Location.Star)));
		}
		
		#region Saving keys
		private const string LocationTag = "Star";
		private const string IdKey = "id";
 		#endregion
	}
}
